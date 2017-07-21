using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class JugadorCombate : MonoBehaviour {

	//Stats
	int saludInicial;

	//Canvas
	[HideInInspector]public Transform barraVida;
	float longInicial;
	[HideInInspector]public TextMesh dañorec, numVida;

	//Animaciones y música
	public AudioClip[] Sonidos;
	Animator anim;

	//1.START
	void Start () 
	{
		barraVida = transform.GetChild(0).GetChild(0);
		longInicial = barraVida.localScale.x;
		dañorec = transform.GetChild(2).gameObject.GetComponent<TextMesh>();

		saludInicial = GameManager.instance.VidaMaxima();
		anim = GetComponent<Animator>();
		anim.SetInteger("Estado", GameManager.instance.EstadoPersonaje());

		numVida = transform.GetChild(3).gameObject.GetComponent<TextMesh>();
		numVida.text = GameManager.instance.Vida() + "/" + GameManager.instance.VidaMaxima();
	}
	
	//2.UPDATE
	void Update () 
	{
		int hp = GameManager.instance.Vida();
		barraVida.localScale = new Vector3(longInicial / saludInicial * hp, barraVida.localScale.y, 1f);
	}
}
