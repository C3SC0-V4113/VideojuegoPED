using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneracionNivel : MonoBehaviour
{
    public GameObject personaje;
    public GameObject triggerFinal;
    //Marca la posicion donde se gerará la primera habitación de un arreglo de 4 posiciones
    //Es decir, 4 posibles habitaciones en el fondo
    public Transform[] PosicionInicio;
    //Almacena en un arreglo las distintas habitaciones creadas
    //INDICES
    //0--CuartoTodos
    //1--IzqDerArriba
    //2--IzqDerAbajo
    //3--IzqDer
    public GameObject[] cuartos;
    //Elije la direccion segun un número
    private int direccion;
    //Elije la cantidad de movimiento que tendrá el vector en el eje seleccionado
    //Ya que no ha sido inicializado, se selecciona en Unity graficamente
    public float CantidadMovimiento;

    //Seccion para el tiempo
    //Se da una variable de tipo float como limite
    private float TiempoEntreCuartos;
    public float InicioTiempoEntreCuartos = 0.25f;

    //Variables que guardan los extremos en coordenadas
    public float minX;
    public float maxX;
    public float maxY;

    //booleano que sirve para detener la generación;
    public bool DetenerGeneracion;

    //CapadeMascara para que el circulo y las habitaciones se encuentren en la misma capa
    public LayerMask cuarto;

    private int ContadorArriba=0;

    void Start()
    {
        int PosicionInicioAleatoria = Random.Range(0, PosicionInicio.Length);
        transform.position = PosicionInicio[PosicionInicioAleatoria].position;
        Debug.Log("Posicion Escogida: "+ PosicionInicio[PosicionInicioAleatoria].position);
        //Instancia en la ubicacion seleccionada la habitacion del indice especifico
        //En la posicion inicial
        //Con una rotacion de cero
        Instantiate(cuartos[0], transform.position, Quaternion.identity);
        Instantiate(personaje, transform.position, Quaternion.identity);
        direccion = Random.Range(1, 6   );
    }

    private void Movimiento()
    {
        //Movimiento a la derecha
        if (direccion == 1 || direccion == 2)
        {
            //Mientras la posicion en X sea menor que el limite maximo en X
            //Se puede seguir moviendo hacia la derecha
            //De lo contrario irá hacia el siguiente piso
            if (transform.position.x < maxX)
            {
                Vector2 nuevaPos = new Vector2(transform.position.x + CantidadMovimiento, transform.position.y);
                Debug.Log(nuevaPos);
                transform.position = nuevaPos;

                //Se selecciona cualquier cuarto, pues ninguno bloquea los laterales
                int aleatorio = Random.Range(0, cuartos.Length);
                Instantiate(cuartos[aleatorio], transform.position, Quaternion.identity);
                Debug.Log("Nuevo Cuarto: "+cuartos[aleatorio].name+" en posicion:"+nuevaPos);
                //Si se mueve hacia la derecha, se evita a toda costa que retroceda
                //Una vez iniciado el movimiento se vuelve a escoger una dirección
                //pero si es a la izquierda solo escoge si ir hacia abajo o a la derecha
                direccion = Random.Range(1, 6);
                if (direccion == 3)
                {
                    direccion = 2;
                }
                else if (direccion == 4)
                {
                    direccion = 5;
                }
            }
            else
            {
                direccion = 5;
            }
        }
        //Movimiento Izquierda
        else if (direccion == 3 || direccion == 4)
        {
            //Mientras la posicion en X sea mayor que el limite minimo en X
            //Se puede seguir moviendo hacia la izquierda
            //De lo contrario irá hacia el siguiente piso
            if (transform.position.x > minX)
            {
                Vector2 nuevaPos = new Vector2(transform.position.x - CantidadMovimiento, transform.position.y);
                transform.position = nuevaPos;
                Debug.Log(nuevaPos);
                //Se selecciona cualquier cuarto, pues ninguno bloquea los laterales
                int aleatorio = Random.Range(0, cuartos.Length);
                Instantiate(cuartos[aleatorio], transform.position, Quaternion.identity);
                Debug.Log("Nuevo Cuarto: " + cuartos[aleatorio].name + " en posicion:" + nuevaPos);

                //Si se inicia hacia la izquierda se evita que retroceda, por lo que las unicas posibilidades
                //son ir hacia abajo o seguir hacia la izquierda
                direccion = Random.Range(3, 6);
            }
            else
            {
                direccion = 5;
            }
        }
        //Movimiento Arriba
        else if (direccion == 5)
        {
            //Mientras l posicion en y sea menor que la maxima coordenada en Y, podrá seguir subiendo
            //De lo contrario, se pone la salida
            if (transform.position.y < maxY)
            {
                ContadorArriba++;

                //Se crea un circulo en la misma capa donde se encuentran las habitaciones
                //Este circulo obtiene el tipo de habitacion y la destruye para sobreescribir encima
                //Un cuarto con apertura arriba
                Collider2D detector = Physics2D.OverlapCircle(transform.position, 1, cuarto);
                if(detector.GetComponent<TipoCuarto>().tipo!=0 && detector.GetComponent<TipoCuarto>().tipo != 1)
                {
                    detector.GetComponent<TipoCuarto>().DestruccionCuarto();
                    int alea0 = Random.Range(0, 2);
                    Instantiate(cuartos[alea0], transform.position, Quaternion.identity);

                }

                Vector2 nuevaPos = new Vector2(transform.position.x, transform.position.y + CantidadMovimiento);
                transform.position = nuevaPos;
                //Se crea una habitacion que tenga una apertura abajo, si esta resulta ser la habitacion con
                //Apertura arriba se cambia a una que tenga las 4 paredes abiertas
                int aleatorio = Random.Range(0, 3);
                if (aleatorio == 1)
                {
                    aleatorio = 0;
                }
                Debug.Log("El indice de la habitacion es: " + aleatorio+" Cuarto de tipo: "+cuartos[aleatorio].name);
                Instantiate(cuartos[aleatorio], transform.position, Quaternion.identity);
                //El contador no permite que hayan dos salas hacia arriba consecutivamente
                if (ContadorArriba >= 1)
                {
                    //Si esta arriba pero al borde decidira ir a la derecha o a la izquierda
                    //Dependiendo de a que borde se encuentre
                    if (transform.position.x < maxX)
                    {
                        direccion = 1;
                    }
                    else if(transform.position.x > minX)
                    {
                        direccion = 4;
                    }
                    ContadorArriba = 0;
                }
                else
                {
                    //Si no esta en los bordes, escoge cualquier direccion
                    //menos hacia arriba
                    direccion = Random.Range(1, 5);
                }
                Debug.Log(direccion);
            }
            else
            {
                //Si esta en el ultimo piso y decide ir hacia arriba, se detiene
                //la generación de mundo
                Instantiate(triggerFinal, transform.position, Quaternion.identity);
                DetenerGeneracion = true;
                Debug.Log("Final de Generación");
            }
        }
    }

    void Update()
    {
        //Ya que el metodo Update se actualiza cada frame
        //Se dice que mientras el Tiempo entre cuartos sea menor
        //o igual a cero, o que llegué a la cima del nivel sucederá un movimiento
        if (TiempoEntreCuartos <= 0 && DetenerGeneracion==false)
        {
            Movimiento();
            TiempoEntreCuartos = InicioTiempoEntreCuartos;
        }
        //De lo contrario, se le resta un DeltaTime
        else
        {
            TiempoEntreCuartos -= Time.deltaTime;
        }
    }
}
