using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Interactuable : MonoBehaviour {

    //SUPER SPAGUETTI LOOOKO
    public TextAsset archivoTexto;      //archivo a leer
    public string[] lineasDialogo;
    int i = 0;       //contador
    public char indicador;
    public Text texto;
    bool interactuado = false;
    public GameObject Player;
    public GameObject Panel;
    public bool esNPC;
    public int numNPC;
    public bool AumentaLevel;
    bool done = false;
    //LogroPapelera
    public bool esPapelera;
    // Use this for initialization
    void Start () {
 
        if(this.GetComponent<BoxCollider>())
        this.GetComponent<BoxCollider>().isTrigger = true;
        if(this.GetComponent<BoxCollider2D>())
        this.GetComponent<BoxCollider2D>().isTrigger = true;
        //separa por líneas y las guarda en el array
        lineasDialogo = archivoTexto.text.Split('\n');
        Panel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            if (interactuado)
            {
                Player.GetComponent<PlayerController>().compAudio.PlayOneShot
                        (Player.GetComponent<PlayerController>().sonidos[1]);
                if (lineasDialogo[i][0] == '*')
                {
                    texto.text = " ";
                    if (Panel != null)
                        Panel.SetActive(false);
                    interactuado = false;
                    Invoke("Activar", 0.1f);
                    if (this.GetComponent<Activariggers>())
                    {
                        if (this.GetComponent<Activariggers>().Finish)
                        {
                            SceneManager.LoadScene("Créditos");
                        }
                    }
                }
                else
                {
                    texto.text = lineasDialogo[i];
                }
                i++;
            }
        }
    }
    public void Interactuado()
    {

        if (!interactuado)
        {
            i = 0;
            Player.GetComponent<PlayerController>().compAudio.PlayOneShot
				  (Player.GetComponent<PlayerController>().sonidos[1]);
            Player.GetComponent<PlayerController>().enabled = false;
            if(Panel != null)
            Panel.SetActive(true);
            //indicador = numero de texto que corresponda        
            bool prueba = false;
            interactuado = true;
            while (i < lineasDialogo.Length && !prueba)
            {
                if (lineasDialogo[i][0] != indicador)
                {
                    i++;
                }
                else
                {
                    prueba = true;
                }
            }
                i++;
                texto.text = lineasDialogo[i];
        }
        //Comprueba si has hablado con todos los NPC del juego
        if (esNPC)
        {
            GameManager.NPcsHablados[numNPC] = true;
            int i = 0;
            bool todosHablados = true;
            while(i < GameManager.NPcsHablados.Length && todosHablados)
            {
                if (!GameManager.NPcsHablados[i])
                {
                    todosHablados = false;
                }
                i++;
            }
            if (todosHablados)
            {
                GameManager.instance.ConsigueLogro(2);
            }

            print("Npcs hablados?" + todosHablados);
        }

        //LogroPapelera
        if (esPapelera)
        {
            GameManager.instance.ConsigueLogro(8);
            print("LogroDone");
        }

        if (AumentaLevel && !done)
        {
            GameManager.instance.AumentaEstado();
            done = true;
            print("estadi" + GameManager.instance.EstadoPersonaje());
        }
    }
    void Activar()
    {
        Player.GetComponent<PlayerController>().enabled = true;
        CancelInvoke();
    }
      
}
