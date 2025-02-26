using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            EnemyDrops enemy = collision.GetComponent<EnemyDrops>(); // Lấy script Enemy
            if (enemy != null)
            {
                enemy.Die();
            }
            Destroy(gameObject);
        }
        else if (collision.CompareTag("wall"))
        {
            Destroy(gameObject);
        }
    }
}
