using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneraciónCuartosExtra : MonoBehaviour
{
    public LayerMask Cuarto;
    public GeneracionNivel nivel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Collider2D detector = Physics2D.OverlapCircle(transform.position, 1, Cuarto);
        if (detector == null && nivel.DetenerGeneracion==true)
        {
            int aleatorio = Random.Range(0, nivel.cuartos.Length);
            Instantiate(nivel.cuartos[aleatorio], transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
