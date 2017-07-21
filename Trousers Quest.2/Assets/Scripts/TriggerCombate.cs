using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerCombate : MonoBehaviour {

	public int enemigo;
	static bool[] destruido = new bool[11];

    void Start()
    {
		for (int i = 0; i < 11; i++)
			if (destruido == null)
				destruido[i] = true;
		
		if (destruido[int.Parse(gameObject.name) - 1])
			Destroy(gameObject);
    }

	void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<PlayerController>())
        {
           GameManager.menuX = other.gameObject.transform.position.x;
           GameManager.menuY = other.gameObject.transform.position.y;

			GameManager.instance.SetEnemigo(enemigo);
			destruido[int.Parse(gameObject.name) -1 ] = true;
			SceneManager.LoadScene("Combate");

        }
    }
}
