﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerFInal : MonoBehaviour
{

    public GameManager gameManager;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Nivel Completado");
        gameManager.NivelCompleto();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
