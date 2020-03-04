using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneracionObjetos : MonoBehaviour
{
    //Creo que arreglo para los distintos objetos que serán el nivel
    public GameObject[] objetos;

    // Start is called before the first frame update
    void Start()
    {
        //Creo un número entre el cero y la cantidad de objetos que hay dentro del arreglo
        int aleatorio = Random.Range(0, objetos.Length);
        //Se instancia un objeto con el indice del número aleatorio, una posicion dicha por transform y una rotacion dicha por Quaternion
        GameObject instancia=(GameObject) Instantiate(objetos[aleatorio], transform.position, Quaternion.identity);
        instancia.transform.parent = transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
