using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage = 10;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy")) // Kiểm tra va chạm với kẻ địch
        {
            EnemyDrops enemy = collision.GetComponent<EnemyDrops>(); // Lấy script Enemy
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
        else if (collision.CompareTag("wall")) // Nếu chạm vào tường, nền thì cũng biến mất
        {
            Destroy(gameObject);
        }
    }
}