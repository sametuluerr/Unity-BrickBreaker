using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleManager : MonoBehaviour
{
    private float paddleSpeed = 9f;
    private float input;

    void Update()
    {
        input = Input.GetAxisRaw("Horizontal");
    }

    private void FixedUpdate()
    {
        // Pedalın hızını ayarla
        GetComponent<Rigidbody2D>().velocity = Vector2.right * input * paddleSpeed;
    }
}