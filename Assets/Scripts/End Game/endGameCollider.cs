using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class endGameCollider : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            // Ends the game going back to the starting screen and also resetting the player's position
            PlayerPrefs.SetFloat("initialPositionX", -1.95f);
            PlayerPrefs.SetFloat("initialPositionY", -10.23f);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }
    }
}
