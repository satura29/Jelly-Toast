using UnityEngine;

public class PeanutButterPowerup : MonoBehaviour
{
    public AudioClip collectSound;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            CharacterController2DScript toaster = other.GetComponent<CharacterController2DScript>();

            if (toaster != null)
            {
                toaster.AddLife();
                         
                Destroy(gameObject);
            }
        }
    }
}