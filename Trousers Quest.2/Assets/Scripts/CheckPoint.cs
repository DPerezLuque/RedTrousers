using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Interactuable))]
public class CheckPoint : MonoBehaviour {

    void Start()
    {
        this.GetComponent<BoxCollider2D>().isTrigger = true;
    }
    void OnTriggerStay2D (Collider2D other)
    {
        if (other.GetComponent<PlayerController>() && Input.GetKey(KeyCode.Space))
        {
            this.GetComponent<Interactuable>().Interactuado();
            other.GetComponent<PlayerController>().enabled = false;
            GameManager.instance.CheckPoint(this.gameObject);
			GameManager.instance.GuardaPartida();
        }
    }
}
