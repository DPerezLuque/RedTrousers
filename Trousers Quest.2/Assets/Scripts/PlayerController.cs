using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{

    public float vel = 100f;
    public float distancia = .1f;
    RaycastHit hit;
    Animator anim;
    float altura;
    [HideInInspector]
    public float velOr;
    public GameObject Salida;
    static float posXSalida, posYSalida;
    bool activar = true;
    GameObject npcDesactivar;
    GameObject casa;
    bool interactuar = true;
	float tiempoAndar;

	[HideInInspector]public AudioSource compAudio;
	public AudioClip[] sonidos = new AudioClip[2];
    //LOGRO CASAS
    static bool[] casasVisitadas = new bool [7];
    void Awake()
    {
        GameManager.spaguetti = false;
        for(int i = 0; i < casasVisitadas.Length; i++)
        {
            casasVisitadas[i] = false;
        }
    }
    //1.START
    void Start()
    {
		compAudio = GetComponent<AudioSource>();
		anim = GetComponent<Animator>();
		anim.SetInteger("Estado", GameManager.instance.EstadoPersonaje());
        altura = this.transform.localScale.y / 12;
        velOr = vel;
        if(Salida != null)
        {
            posXSalida = Salida.transform.position.x;
            posYSalida = Salida.transform.position.y; 
        }
		transform.position = new Vector3(GameManager.menuX, GameManager.menuY, 0f); 
    }

    //2.UPDATE
    void Update()
    {
        Movimiento();
		tiempoAndar += Time.deltaTime;
		if (anim.GetBool("Andando") && tiempoAndar > 0.3)
		{
			SonidoAndar();
			tiempoAndar = 0;
		}
      
    }

    //3.INTERACTÚA CON OBJETOS/NPCs
    void Interactuar()
    {
        
        if (Input.GetKey(KeyCode.Space) && interactuar)
        {
			
            
            Collider2D hitCol = Physics2D.OverlapCircle(new Vector2(transform.position.x, transform.position.y),0.5f);
			//se realiza una búsqueda de los objetos que se pueden coger
			if (hitCol != null)
			{
                interactuar = false;
                if (hitCol.gameObject.GetComponent<Interactuable>())
				{
					hitCol.gameObject.GetComponent<Interactuable>().Interactuado();
					this.GetComponent<PlayerController>().enabled = false;
				}
				else if (hitCol.gameObject.GetComponent<NPCRandom>())
				{
					hitCol.gameObject.GetComponent<NPCRandom>().Activado();
					this.GetComponent<PlayerController>().enabled = false;
				}
			}
            
            Invoke("Activar", 0.3f);
        }
       
    }

	//4.MOVIMIENTO
	void Movimiento()
	{
		Debug.DrawRay(new Vector3(transform.position.x, transform.position.y - altura, 0f), transform.up, Color.red);
		//Arriba, abajo, izda, dcha
		anim.SetBool("MismaDir", false);

		if (Input.GetKey(KeyCode.W))
		{
			if (anim.GetInteger("Dirección") == 1 && anim.GetBool("Andando"))
				anim.SetBool("MismaDir", true);

			anim.SetBool("Andando", true);
			anim.SetInteger("Dirección", 1);
			transform.rotation = Quaternion.AngleAxis(0f, new Vector3(0f, 0f, 1f));

			if (!Physics.Raycast(new Vector3(transform.position.x, transform.position.y - altura, 0f), transform.up, out hit, distancia) || hit.collider.isTrigger)
				transform.Translate(new Vector3(0f, vel * Time.deltaTime, 0f));


		}
		else if (Input.GetKey(KeyCode.S))
		{
			if (anim.GetInteger("Dirección") == 2 && anim.GetBool("Andando"))
				anim.SetBool("MismaDir", true);

			anim.SetBool("Andando", true);
			anim.SetInteger("Dirección", 2);
			transform.rotation = Quaternion.AngleAxis(180f, new Vector3(0f, 0f, 1f));

			if (!Physics.Raycast(new Vector3(transform.position.x, transform.position.y - altura, 0f), transform.up, out hit, distancia) || hit.collider.isTrigger)
				transform.Translate(new Vector3(0f, vel * Time.deltaTime, 0f));
		}

		else if (Input.GetKey(KeyCode.A))
		{
			if (anim.GetInteger("Dirección") == 3 && anim.GetBool("Andando"))
				anim.SetBool("MismaDir", true);

			anim.SetBool("Andando", true);
			anim.SetInteger("Dirección", 3);
			transform.rotation = Quaternion.AngleAxis(90f, new Vector3(0f, 0f, 1f));

			if (!Physics.Raycast(new Vector3(transform.position.x, transform.position.y - altura, 0f), transform.up, out hit, distancia) || hit.collider.isTrigger)
				transform.Translate(new Vector3(0f, vel * Time.deltaTime, 0f));
		}

		else if (Input.GetKey(KeyCode.D))
		{
			if (anim.GetInteger("Dirección") == 4 && anim.GetBool("Andando"))
				anim.SetBool("MismaDir", true);

			anim.SetBool("Andando", true);
			anim.SetInteger("Dirección", 4);
			transform.rotation = Quaternion.AngleAxis(-90f, new Vector3(0f, 0f, 1f));

			if (!Physics.Raycast(new Vector3(transform.position.x, transform.position.y - altura, 0f), transform.up, out hit, distancia) || hit.collider.isTrigger)
				transform.Translate(new Vector3(0f, vel * Time.deltaTime, 0f));
		}
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            Interactuar();
        }
		else
		{
			anim.SetBool("Andando", false);
			anim.SetBool("MismaDir", false);
		}

		if (Input.GetKeyDown(KeyCode.I))
		{
            FindObjectOfType<Inventario>().SendMessage("Activado",activar);
            activar = !activar;
        }

		else if (Input.GetKeyDown(KeyCode.Escape))
		{
			GameManager.menuX = transform.position.x;
			GameManager.menuY = transform.position.y;
			GameManager.instance.GuardaPartida();
			SceneManager.LoadScene("Menú");
		}


		//Correr
		if (Input.GetKeyDown(KeyCode.LeftShift))
		{
			vel = velOr + vel;
		}

		else if (Input.GetKeyUp(KeyCode.LeftShift))
		{
			vel = velOr;
		}
	}

    //Salir/Entrar de Casas
    void OnTriggerStay2D(Collider2D other)
    {
        
        if (other.GetComponent<Traslado>())
        {
                if (!other.gameObject.GetComponent<Traslado>().salida)
                {
				GameManager.instance.PlayMusic(6);

                casa = other.gameObject; 
                GameObject.FindGameObjectWithTag("NPCS").transform.GetChild(other.GetComponent<Traslado>().numCasa).gameObject.SetActive(true);
                npcDesactivar = GameObject.FindGameObjectWithTag("NPCS").transform.GetChild(other.GetComponent<Traslado>().numCasa).gameObject;
                this.transform.position = new Vector2(posXSalida, posYSalida);
                casasVisitadas[other.GetComponent<Traslado>().numCasa] = true;
                int i = 0;
                bool todasVisitadas = true;
                while(i < casasVisitadas.Length && todasVisitadas)
                {
                    if (!casasVisitadas[i])
                    {
                        todasVisitadas = false;
                    }
                    i++;
                }
                if (todasVisitadas)
                {
                    GameManager.instance.ConsigueLogro(4);
                }
                }
                else
                {
				    GameManager.instance.PlayMusic(GameManager.instance.MusicaActual());
                    npcDesactivar.SetActive(false);
                    this.transform.position = new Vector2(casa.transform.position.x, casa.transform.position.y);
                }
        }
    }
    void Activar()
    {
        interactuar = true;
    }

	void SonidoAndar() 
	{
		compAudio.PlayOneShot(sonidos[0], 2f);
	}



}
