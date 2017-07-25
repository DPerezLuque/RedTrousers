using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemigo : MonoBehaviour 
{
	//STATS
	[HideInInspector]
	public int saludInicial;
	[HideInInspector]
	public int hp, poder;

	//TIPO DE ENEMIGO
	public enum TipoEnemigo { Polilla, Polillo, Dragón, Snowman , Robot, RobotBoss, Boss};
	[HideInInspector]public int numEnemigo;
	public TipoEnemigo enemigo;

	//CANVAS
	Transform barraVida;
	float longInicial;
	[HideInInspector]
	public TextMesh dañorec, numVida;
	[HideInInspector]
	public AudioSource sonido;


	//1.START
	void Start () 
	{
		//Le ponemos stats dependiendo del enemigo que sea
		switch (enemigo) 
		{
			case TipoEnemigo.Polilla:
				numEnemigo = 0;
				hp = 40;
				poder = 15;
				break;
			case TipoEnemigo.Polillo:
				numEnemigo = 1;
				hp = 50;
				poder = 17;
				break;
			case TipoEnemigo.Dragón:
				numEnemigo = 2;
				hp = 70;
				poder = 17;
				break;
			case TipoEnemigo.Snowman:
				numEnemigo = 3;
				hp = 60;
				poder = 22;
				break;
			case TipoEnemigo.Robot:
				numEnemigo = 4;
				hp = 75;
				poder = 30;
				break;
			case TipoEnemigo.RobotBoss:
				numEnemigo = 5;
				hp = 85;
				poder = 35;
				break;
			default:
				numEnemigo = 6;
				hp = 100;
				poder = 50;
				break;
		}
		
		dañorec = transform.GetChild(2).gameObject.GetComponent<TextMesh>();
		barraVida = transform.GetChild(0).GetChild(0);
		longInicial = barraVida.localScale.x;
		saludInicial = hp;

		numVida = transform.GetChild(3).gameObject.GetComponent<TextMesh>();
		numVida.text = hp + "/" + saludInicial;
		sonido = GetComponent<AudioSource>();
	}
	
	//2.UPDATE
	void Update () 
	{
		//Renderizado de la vida
		if(hp>0)
			barraVida.localScale = new Vector3(longInicial/saludInicial * hp, barraVida.localScale.y, 1f);
		else
			barraVida.localScale = new Vector3(0f, 0f, 1f);
	}
}
