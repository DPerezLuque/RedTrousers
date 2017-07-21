using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.UI;

public class GameManager : MonoBehaviour 
{
	//GM
	public static GameManager instance;

	//MÚSICA
	public AudioClip [] Musica;
	private AudioSource compAudio;
	public static float volu = 1f;
	int musicaActual;

	//STATS
	public static int estadoPersonaje;
	public static int hp, poder;
	[HideInInspector]
	public static int vidaMaxima;
	static public bool[] logros = new bool[10];
	static int tipoEnemigo;

	//GUARDADO
	public Transform posPersonaje;
	static GameObject ultimoCheck;
	[HideInInspector]
	public static float menuX, menuY;
	[HideInInspector]
	public static float posX, posY, combateX, combateY;

	//CANVAS
	public GameObject canvas;
	Text textoVida, textoPoder;
	public GameObject Fondos;


	//??
    public static bool  spaguetti = true;

    //LOGRO NPCS
    public static bool [] NPcsHablados = new bool [27];
   
    //1.AWAKE
    void Awake () 
	{
        //estadoPersonaje = 4;
		instance = this;
		musicaActual = estadoPersonaje;
		compAudio = GetComponent<AudioSource>();
		//tipoEnemigo = 0;
		if (canvas != null)
		{
			textoVida = canvas.transform.GetChild(1).GetComponent<Text>();
			textoPoder = canvas.transform.GetChild(2).GetComponent<Text>();
		}

        ActualizaVol(volu);
		if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Interacción"))
			PlayMusic(musicaActual);
		else if (SceneManager.GetActiveScene() != SceneManager.GetSceneByName("Combate"))
			PlayMusic(0);
		else 
		{
			if (estadoPersonaje == 5)
			{
				PlayMusic(1);
				Fondos.transform.GetChild(1).gameObject.SetActive(true);
			}
			else 
			{
				PlayMusic(0);
				Fondos.transform.GetChild(0).gameObject.SetActive(true);
			}
		}
		

	}
	
	//2.UPDATE
	void Update () 
	{
		if (canvas != null)
			ActualizaGUI();

            ActualizaVol(volu);
        Debug.Log(EstadoPersonaje());

	}

	//3.DEVUELVE EL PODER DEL PERSONAJE
	public int Poder() 
	{
		return poder;
	}

	//4.DEVUELVE LA VIDA DEL PERSONAJE
	public int Vida()
	{
		return hp;
	}

	//5.QUITA VIDA AL PERSONAJE
	public void QuitaVida(int vida) 
	{
		hp -= vida;
	}

	//6.REPRODUCE UNA CANCIÓN
	public void PlayMusic(int num) 
	{
		compAudio.Stop();
		//compAudio.PlayOneShot(Musica[num], compAudio.volume);
		compAudio.clip = Musica[num];
		compAudio.Play();
		if(num!=6)
			musicaActual = num;
	}

	//7.DEVUELVE LA CANCIÓN ACTUAL
	public int MusicaActual() 
	{
		return musicaActual;
	}

	//8.SUMA VIDA AL JUGADOR
	public void SumaVida(int vida) 
	{
		hp += vida;
	}

	//9.DEVUELVE LA VIDA MÁXIMA ACTUAL DEL JUGADOR
	public int VidaMaxima()
	{
		return vidaMaxima;
	}

	//10.DEVUELVE EL ESTADO DEL PERSONAJE
	public int EstadoPersonaje() 
	{
		return estadoPersonaje;
	}

	//11.CHECKPOINTS
	public void CheckPoint(GameObject Check) 
	{
		ultimoCheck = Check;
		hp = vidaMaxima;
	}

	//12.VUELVE AL ÚLTIMO CHECKPOINT VISITADO
	public void VuelveACheckpoint(GameObject jugador) 
	{
		if (ultimoCheck != null)
		{
         jugador.transform.position = ultimoCheck.transform.position;
		}
		else
			jugador.transform.position = new Vector3(0f,0f,0f);

	}

	//13.ACTUALIZA EL GUI
	public void ActualizaGUI()
	{
		textoVida.text = "" + vidaMaxima;
		textoPoder.text = "" + poder;
	}

    //14.COMIENZA EL JUEGO
    public void StartGame()
    {
		hp = 35;
		poder = 20;
		vidaMaxima = hp;
        SceneManager.LoadScene("Interacción");
		/*if (File.Exists("partida"))
		{
			StreamReader entrada = new StreamReader("partida");
			estadoPersonaje = int.Parse(entrada.ReadLine());
			hp=int.Parse(entrada.ReadLine());
			poder=int.Parse(entrada.ReadLine());

			float x = int.Parse(entrada.ReadLine());
			posPersonaje.position=new Vector3(x, int.Parse(entrada.ReadLine()), 0);

			string s = entrada.ReadLine();
			for (int i = 0; i < 10; i++)
			{
				if (s[i] == '1')
					logros[i] = true;
				else
					logros[i] = false;
			}

			entrada.Close();
		}
		else*/
			estadoPersonaje = 0;
    }

    //15.SALIR DEL JUEGO
    public void Salir()
    {
        Application.Quit();
    }

    //16.CONTROLADOR BARRA VOLUMEN
    public void VariarVol()
    {
        FindObjectOfType<SliderAudio>().GetComponent<SliderAudio>().SubmitSliderSetting();
    }

    //17.RESPAWN
    public void Respawn()
    {
        FindObjectOfType<PlayerController>().gameObject.transform.position = ultimoCheck.transform.position;
    }

	//18.GUARDA PARTIDA
	public void GuardaPartida() 
	{
		if (File.Exists("partida"))
		   File.Delete("partida");
		
		StreamWriter salida = new StreamWriter("partida");
		salida.WriteLine(estadoPersonaje);
		salida.WriteLine(hp);
		salida.WriteLine(poder);
		salida.WriteLine(posPersonaje.position.x);
		salida.WriteLine(posPersonaje.position.y);
		for (int i = 0; i < 10; i++)
		{
			if (logros[i])
				salida.Write(1);
			else
				salida.Write(0);
		}

		salida.Close();
	}

    //19.ACTUALIZAR VOLUMEN
     public void ActualizaVol(float volumen)
    {
        if(this != null)
        GetComponent<AudioSource>().volume = volumen;
        volu = volumen;
    }

	//20.AUMENTA EL NIVEL
	public void AumentaEstado() 
	{
		estadoPersonaje++;
		vidaMaxima += 10;
		poder += 5;
		hp = vidaMaxima;
		if (estadoPersonaje == 5)
			ConsigueLogro(1);
	}

	public void ConsigueLogro(int i) 
	{
		logros[i] = true;
		Debug.Log("Logro " + i + " conseguido");

		//Logro de diamante
		int j = 0;
		while (j<9 && logros[i])
			j++;
		if (j == 8)
			logros[9] = true;
	}

	public int TipoEnemigo() 
	{
		return tipoEnemigo;
	}

	public void SetEnemigo(int i) 
	{
		tipoEnemigo = i;
	}

	public void DevuelveJugador() 
	{
        /*GameObject player = FindObjectOfType<PlayerController>().gameObject;
        if(player != null)
        {
            player.transform.position = new Vector2(combateX, combateY);

        }*/
        GameObject go =  GameObject.FindGameObjectWithTag("Player");
        if (go != null)
        {
            go.transform.position = new Vector2(combateX, combateY);
        }
        else
        {
            print("es nuuull looooka");
        }
	}

	public void VolverDeCreditos() 
	{
		SceneManager.LoadScene("Menú");
	}


}