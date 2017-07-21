using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
[RequireComponent(typeof(Interactuable))]
[RequireComponent(typeof(BoxCollider2D))]

public class Activariggers : MonoBehaviour {

    public int numTrigger;
    static bool done = false;      //SPAGUETTI TOTAL
    public bool Finish;
    void Update()
    {
        if (numTrigger == GameManager.estadoPersonaje)
        {
            this.GetComponent<BoxCollider2D>().enabled = true;          //esto no me mola nada pero no cambiamos de escena tiio
            this.GetComponent<BoxCollider2D>().isTrigger = true;
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<PlayerController>())
        {
            this.GetComponent<Interactuable>().Interactuado();
            if (!Finish)
            {
                this.GetComponent<BoxCollider2D>().enabled = false;
                Destroy(this);
            }
        
        }
    }
}
