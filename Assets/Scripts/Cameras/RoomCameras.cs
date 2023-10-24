using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCameras : MonoBehaviour
{
    public GameObject virtualCam;
    public GameObject background;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player") && !other.isTrigger)
        {
            virtualCam.SetActive(true);
            background.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            virtualCam.SetActive(false);
            background.SetActive(false);
        }
    }
}
