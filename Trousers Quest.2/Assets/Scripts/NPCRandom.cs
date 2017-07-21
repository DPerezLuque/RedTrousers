using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class NPCRandom : MonoBehaviour {

    public Text texto;
    public GameObject Panel;
    public string[] lineasDialogo;
    public TextAsset archivoTexto;
    bool interactuado = false;
    public GameObject Player;
    static int cont = 0;
    // Use this for initialization
    void Start () {

        lineasDialogo = archivoTexto.text.Split('\n');
        if (this.GetComponent<BoxCollider2D>())
        {
            GetComponent<BoxCollider2D>().isTrigger = true;
        }
       
    }
	
	// Update is called once per frame
	void Update () {


        if (interactuado && Input.GetKeyDown(KeyCode.Space))
        {
            Panel.SetActive(false);
            interactuado = false;
            Player.GetComponent<PlayerController>().enabled = true;
        }
		
	}

    public void Activado()
    {
        Panel.SetActive(true);
        texto.text = lineasDialogo[Random.Range(0, 30)];
        Invoke("Activar", 0.2f);
        cont++;
        print(cont);
        //LOGRO CANSINO
        if(cont >= 30)
        {
            GameManager.instance.ConsigueLogro(6);
        }
    }

    void Activar()
    {
        interactuado = true; ;
    }
}
