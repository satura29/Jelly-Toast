using UnityEngine;

public class CoinScript : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            // make the coin noise
            SoundManager.Steve.MakeCoinSound();
            // add to the score
            GameManager.Gary.AddScore(1);
            // remove the sprite
            Destroy(this.gameObject);
        }

        if (collision.gameObject.tag == "Toast")
        {
            // make the coin noise
            SoundManager.Steve.MakeCoinSound();
            // add to the score
            GameManager.Gary.AddScore(1);
            // remove the sprite
            Destroy(this.gameObject);
        }
    }
}