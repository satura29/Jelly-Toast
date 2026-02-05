using UnityEngine;

public class HazardScript : MonoBehaviour
{
    public GameObject deathSoundPrefab, gameOverPrefab;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            Destroy(collision.gameObject); // kill the player

            GameObject soundDeath = Instantiate(deathSoundPrefab, transform.position, Quaternion.identity);
            Destroy(soundDeath, 3f);
            GameObject gameDeath = Instantiate(gameOverPrefab, transform.position, Quaternion.identity);
            Destroy(gameDeath, 3f);
        }
    }
}
