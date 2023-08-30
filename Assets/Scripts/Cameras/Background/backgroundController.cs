using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class backgroundController : MonoBehaviour
{

    [SerializeField] private Vector2 moveSpeed;
    private Vector2 offset;
    private Material material;

    private void Awake()
    {
        material = GetComponent<SpriteRenderer>().material;
    }

    private void Update()
    {
        offset = moveSpeed * Time.deltaTime;
        material.mainTextureOffset += offset;
    }
}
