using UnityEngine;

public class ToastProjectile : MonoBehaviour
{
    public float speed = 6f;
    public float lifetime = 3f;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.Translate(Vector2.down * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            BossScript boss = collision.gameObject.GetComponent<BossScript>();
            if (boss != null)
            {
                Destroy(gameObject);
                boss.ToasterDamage();
                //GameManager.Gary.AddScore(1);
            } else
            {
                EnemyScript enemy = collision.gameObject.GetComponent<EnemyScript>();
                // send it the death command
                enemy.PlayerKilledEnemy();
                GameManager.Gary.AddScore(1);

                Destroy(collision.gameObject);
                Destroy(gameObject);
            }
        }

        if (collision.CompareTag("Threat"))
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }

        if (collision.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }
}