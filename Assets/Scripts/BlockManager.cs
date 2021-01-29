using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockManager : MonoBehaviour
{
    public int blockHealth;

    // Top ve tuğlaların çarpışması sonrasında çalışan fonksiyon
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ball")
            blockHealth--;

        if (blockHealth == 0)
        {
            Destroy(this.gameObject);
            GameManager.brokenBrickCount += 1;

            if (this.gameObject.tag == "OrangeBlock")
                GameManager.gameScore += 10;
            if (this.gameObject.tag == "BlueBlock")
                GameManager.gameScore += 20;
            if (this.gameObject.tag == "GreenBlock")
                GameManager.gameScore += 30;
        }
    }
}