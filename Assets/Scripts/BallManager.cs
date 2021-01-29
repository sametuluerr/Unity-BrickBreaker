using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallManager : MonoBehaviour
{
    public AudioSource brokenBrick;
    public Rigidbody2D rb;

    void Update()
    {
        Movement();
    }

    // Topun Hareketini ayarlamak için yazılan fonksiyon
    void Movement()
    {
        if (GameManager.startGame == true && GameManager.gameStarted == false)
        {
            rb.AddForce(transform.up * 300f);
            rb.AddForce(transform.right * -150f);

            GameManager.gameStarted = true;
        }

        // Topun x yönünde sıkışmaması için
        if (rb.velocity.x < 1 && rb.velocity.x >= 0)
            rb.AddForce(transform.right * -50);
        if (rb.velocity.x > -1 && rb.velocity.x < 0)
            rb.AddForce(transform.right * 50);

        // Topun y yönünde sıkışmaması için
        if (rb.velocity.y < 1 && rb.velocity.y >= 0)
            rb.AddForce(transform.up * -50);
        if (rb.velocity.y > -1 && rb.velocity.y < 0)
            rb.AddForce(transform.up * 50);
    }

    // Çarpışma kontrolünü sağlayan fonksiyon
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "LoseBox")
            GameManager.isDead = true;

        if (collision.gameObject.tag == "OrangeBlock" || collision.gameObject.tag == "BlueBlock" || collision.gameObject.tag == "GreenBlock")
            brokenBrick.Play();
    }
}