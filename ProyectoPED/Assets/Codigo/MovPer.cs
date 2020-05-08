using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovPer : MonoBehaviour
{
    //Controla la velocidad
    public float velocidad;
    //Controla la altura del salto
    public float Salto;
    //Controla la tecla escogida
    private float EntradaMovimiento;
    //Controla la fisica, como la gravedad del personaje
    private Rigidbody2D rb;
    //Para que se voltee cuando mire al otro lado
    private bool Mirando = false;
    //Para que detecte si el personaje esta o no en el suelo
    private bool Enterrado;
    //Detecta que en esa ubicacion hay suelo
    public Transform UbicacionSuelo;
    //Se creara un circulo invisible en los pies del personaje, este será su radio
    public float Radio;
    //La capa de textura que detectara el circulo
    public LayerMask suelo;
    //Cantidad de Saltos y la misma guardada internamente
    public int ValorSaltosExtra;
    private int SaltosExtra;
    //Mirilla
    public GameObject mirilla;
    //ANIMACION
    public Animator animador;
    //Flecha
    public GameObject flecha;
    //Vector que dice la direccion adonde se apunta
    private Vector3 apuntar;
    //Vector que dice la posicion del mouse
    private Vector3 posicionMouse;
    //la camara, sirve para que tenga un punto de donde esta el mouse segun la camara
    //Y para seguir al personaje
    private Camera camara;
    //Un desfase angular para la punteria del proyectil
    public float desfase;
    //Número que verifica la velocidad de las flechas
    public float VelocidadFlechas;
    //Objeto que seria el ´proyectil con un RigidBody
    private GameObject proyectil;
    //Un vector que se mueve solo en la coordenada z
    private Vector3 alejamientocamara = new Vector3(0.0f, 0.0f, -10.0f);

    private GameObject Mira;

    private void Awake()
    {
        camara = Camera.main;
    }

    void Start()
    {
        Mira = Instantiate(mirilla);
        SaltosExtra = ValorSaltosExtra;
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        //La camara se coloca en la posicion del jugador, pero se suma un vector que la hace  hacia atras
        //Si estuviera en la misma coordenada z, no se veria el personaje
        camara.transform.localPosition = transform.localPosition + alejamientocamara;

        //El angulo que se actualiza cada frame, usando una funcion arcotangente con la posicion del mouse
        //Se multiplica por una constante que lo convierte de Radianes a Grados
        float angulo = Mathf.Atan2(posicionMouse.y, posicionMouse.x) * Mathf.Rad2Deg;
        //El objeto mirilla sigue a la posicion del jugador mas el vector apuntar
        mirilla.transform.localPosition = transform.localPosition + apuntar;

        //Si enterrado es igual a verdadero, la cantidad de saltos en el aire que puede dar se refreca
        if (Enterrado == true)
        {
            SaltosExtra = ValorSaltosExtra;
        }

        //Si pulsa espacio y el contador de saltos es mayor de cero
        //Entonces salta y descuenta la cantidad de posibles saltos
        if (Input.GetKeyDown(KeyCode.Space) && SaltosExtra > 0)
        {
            rb.velocity = Vector2.up * Salto;
            SaltosExtra--;
        }
        //Si la cantidad es cero, entonces solo hace el salto
        else if (Input.GetKeyDown(KeyCode.Space) && SaltosExtra == 0 && Enterrado == true)
        {
            rb.velocity = Vector2.up * Salto;
        }
    }

    private void FixedUpdate()
    {
        //Se crea un circulo que detectara el suelo(basado en una capa) siguiendo la ubicacion de un objeto en los pies del personaje 
        Enterrado = Physics2D.OverlapCircle(UbicacionSuelo.position, Radio, suelo);
        //Se hace que se mueva de forma horizontal
        EntradaMovimiento = Input.GetAxis("Horizontal");
        //A ese movimiento horizontal se le multiplica la velocidad dada
        rb.velocity = new Vector2(EntradaMovimiento * velocidad, rb.velocity.y);
        //Dependiendo del eje donde se mueve se reproducira una animacion
        animador.SetFloat("Horizontal", Input.GetAxis("Horizontal"));

        //Se llama al movimiento de Mirilla
        MovimientoMirilla();

        //Dependiendo de a donde camine se volteara
        if (Mirando == false && EntradaMovimiento > 0)
        {
            Voltear();
        }
        else if (Mirando == true && EntradaMovimiento < 0)
        {
            Voltear();
        }
    }

    private void Voltear()
    {
        //Si esta mirando a la derecha esto dira que ahora mire a la izquierda
        Mirando = !Mirando;

        //Se pone 0 en el angulo de rotacion en x, 180 en angulo de rotacion y, y 0
        //en angulo de eje z
        transform.Rotate(0f, 180f, 0f);
    }

    private void MovimientoMirilla()
    {
        //Ubica la posicion del Mouse, al ser un Vector 3D tiene que ponerse 0 en el eje Z
        posicionMouse = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0.0f);
        //El vector apuntar esta basado en la camara y la ubicacion del mouse en esta
        //Menos la posicion del jugador
        apuntar = camara.ScreenToWorldPoint(posicionMouse) - transform.localPosition;
        //La direccion de disparo se basa en apuntar y sus coordenadas en x y y
        Vector2 direccionDisparo = new Vector2(apuntar.x, apuntar.y);
        //se normaliza para que su magnitud sea 1
        direccionDisparo.Normalize();
        //Se llama al metodo de disparo y se envia el vector necesario
        Disparar1(direccionDisparo);
        //Si apuntar supera la magnitud de una unidad entonces se normalizara
        if (apuntar.magnitude > 1.0f)
        {
            apuntar.Normalize();
            //La mirilla se mantiene en la punta del vector apuntar
            Mira.transform.localPosition = apuntar+transform.localPosition;
        }
    }

    private void Disparar1(Vector2 DireccionDisparo)
    {
        if (Input.GetButtonDown("Fire1"))
        {
            proyectil = Instantiate(flecha, transform.position, Quaternion.identity);
            proyectil.GetComponent<Rigidbody2D>().velocity = DireccionDisparo * VelocidadFlechas;
            proyectil.transform.Rotate(0.0f, 0.0f, Mathf.Atan2(DireccionDisparo.y, DireccionDisparo.x) * Mathf.Rad2Deg);
            Destroy(proyectil, 3.0f);
        }
    }
}
