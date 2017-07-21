using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventario : MonoBehaviour {

    public GameObject interfaz;
    bool aux;

    void Start()
    {
        interfaz.SetActive(false);
    }

	void Activado(ref bool activar)
    {
        interfaz.transform.GetChild(2).gameObject.SetActive(false);
        interfaz.gameObject.SetActive(activar);
        for(int i = 0; i <= GameManager.instance.EstadoPersonaje(); i++)
        {
            interfaz.transform.GetChild(0).GetChild(i).gameObject.SetActive(activar);     
        }
        interfaz.transform.GetChild(1).GetChild(GameManager.instance.EstadoPersonaje()).gameObject.SetActive(activar);
        aux = !activar;
        //desactivar vel personaje cuando tengamos fondo
        //espero que diego no lea esto
        //diego se come los mocos jujujuj
        //Meter a  miniman en los creditos
    }
}
