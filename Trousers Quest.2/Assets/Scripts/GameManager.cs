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
	static int musicaActual;

	//STATS
	public static int estadoPersonaje;
	public static int hp, poder;
	[HideInInspector]
	public static int vidaMaxima;
	static public bool[] logros = new bool[10];
	static int tipoEnemigo;

	//GUARDADO
	[HideInInspector]
	public static int combateX, combateY;
	[HideInInspector]
	public static int guardadoX, guardadoY;
	public GameObject jugador;

	//CANVAS
	public GameObject canvas;
	Text textoVida, textoPoder;
	public GameObject Fondos;

    //LOGRO NPCS
    public static bool [] NPcsHablados = new bool [27];
   
    //1.AWAKE
    void Awake () 
	{
		instance = this;
		compAudio = GetComponent<AudioSource>();

		//Pillamos las cosas del canvas
		if (canvas != null)
		{
			textoVida = canvas.transform.GetChild(1).GetComponent<Text>();
			textoPoder = canvas.transform.GetChild(2).GetComponent<Text>();
		}

        ActualizaVol(volu);

		//MÚSICA A REPRODUCIR
		//1.Música normal
		if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Interacción"))
			PlayMusic(musicaActual);
		//2.Música de combate
		else if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Combate"))
		{
			//Contra el boss
			if (estadoPersonaje == 5)
			{
				PlayMusic(1);
				Fondos.transform.GetChild(1).gameObject.SetActive(true);
			}
			//Normal 
			else
			{
				PlayMusic(0);
				Fondos.transform.GetChild(0).gameObject.SetActive(true);
			}
		}

		//3.Música del menú
		else if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Menú"))
		{
			PlayMusic(0);
			if (File.Exists("partida"))
				LeePartida();
			else
			{
				estadoPersonaje = 0;
				hp = 25;
				poder = 20;
			}
			vidaMaxima = hp;
		}

		//4.Música de créditos
		else 
			PlayMusic(0);
	}
	
	//2.UPDATE
	void Update () 
	{
		if (canvas != null)
			ActualizaGUI();

            ActualizaVol(volu);
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
		compAudio.clip = Musica[num];
		compAudio.Play();
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


	//12.VUELVE AL ÚLTIMO CHECKPOINT VISITADO
	public void MuereEnCombate()
	{
		hp = vidaMaxima;
		combateX = 0;
		combateY = 0;
	}

	//13.ACTUALIZA EL GUI
	public void ActualizaGUI()
	{
		textoVida.text = "" + vidaMaxima;
		textoPoder.text = "" + poder;
	}


	//14.LEE PARTIDA
	public void LeePartida() 
	{
		StreamReader entrada = new StreamReader("partida");

		//Stats
		estadoPersonaje = int.Parse(entrada.ReadLine());
		hp = int.Parse(entrada.ReadLine());
		poder = int.Parse(entrada.ReadLine());

		//Posición
		guardadoX = int.Parse(entrada.ReadLine());
		guardadoY = int.Parse(entrada.ReadLine());

		//Logros
		string s = entrada.ReadLine();
		for (int i = 0; i < 10; i++)
		{
			if (s[i] == '1')
				logros[i] = true;
			else
				logros[i] = false;
		}
		musicaActual = int.Parse(entrada.ReadLine());
		entrada.Close();
	}

    //15.COMIENZA EL JUEGO
    public void StartGame()
    {
		SceneManager.LoadScene("Interacción");
    }

    //16.SALIR DEL JUEGO
    public void Salir()
    {
        Application.Quit();
    }

    //17.CONTROLADOR BARRA VOLUMEN
    public void VariarVol()
    {
        FindObjectOfType<SliderAudio>().GetComponent<SliderAudio>().SubmitSliderSetting();
    }

	//18.GUARDA PARTIDA
	public void GuardaPartida() 
	{
		//Guardado de las posiciones por si seguimos jugando
		guardadoX = (int)jugador.transform.position.x;
		guardadoY = (int)jugador.transform.position.y;

		//Escribimos los datos
		StreamWriter salida = new StreamWriter("partida");
		salida.WriteLine(estadoPersonaje);
		salida.WriteLine(hp);
		salida.WriteLine(poder);
		salida.WriteLine(guardadoX);
		salida.WriteLine(guardadoY);
		//Logros
		for (int i = 0; i < 10; i++)
		{
			if (logros[i])
				salida.Write(1);
			else
				salida.Write(0);
		}
		salida.WriteLine();

		salida.WriteLine(musicaActual);
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

	//21.CONSIGUE UN LOGRO
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

	//22.DEVUELVE EL TIPO DE ENEMIGO QUE ES
	public int TipoEnemigo() 
	{
		return tipoEnemigo;
	}

	//23.DICE EL TIPO DE ENEMIGO QUE ES
	public void SetEnemigo(int i) 
	{
		tipoEnemigo = i;
	}

	//24.CARGA LA ESCENA DEL MENÚ TRAS LOS CRÉDITOS
	public void VolverDeCreditos() 
	{
		SceneManager.LoadScene("Menú");
	}

	public void SetMusica(int i) 
	{
		musicaActual = i;
	}
}