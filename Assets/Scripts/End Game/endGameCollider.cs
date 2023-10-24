using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class endGameCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Terminar el juego cerrando la aplicación
            Application.Quit();
        }
    }
}
