using UnityEngine;

public class JamScript : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            // make the coin noise
            SoundManager.Steve.MakeBreadSound();

            GameManager.Gary.WinText();
            GameManager.Gary.AddScore(100);
            GameManager.Gary.StopTimer();
            GameManager.Gary.LevelWin();

            Destroy(this.gameObject);
        }
    }
}