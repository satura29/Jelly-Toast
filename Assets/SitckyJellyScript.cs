using UnityEngine;

public class SitckyJellyScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            CharacterController2DScript toaster = other.GetComponent<CharacterController2DScript>();
            if (toaster != null)
            {
                toaster.hasJamPowerUp = true;
                Destroy(this.gameObject);
            }
        }
    }
}
