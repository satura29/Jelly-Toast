using UnityEngine;

public class FerryPlatform : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        // get the animator component
        animator = GetComponent<Animator>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            animator.SetTrigger("FerryStart");
        }
    }
}