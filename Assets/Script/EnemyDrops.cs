using UnityEngine;

[System.Serializable]
public class LootItem
{
    public GameObject itemPrefab; // Prefab của vật phẩm
    public float dropChance; // Xác suất rơi (tổng tất cả phải <= 1)
}

public class EnemyDrops : MonoBehaviour
{
    public int health = 30;
    public LootItem[] loots; // Danh sách vật phẩm có thể rơi

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        DropLoot();
        Destroy(gameObject);
    }

    void DropLoot()
    {
        float randomValue = Random.Range(0f, 1f);
        float cumulativeProbability = 0f;

        foreach (LootItem loot in loots)
        {
            cumulativeProbability += loot.dropChance;

            if (randomValue <= cumulativeProbability)
            {
                Instantiate(loot.itemPrefab, transform.position, Quaternion.identity);
                break;
            }
        }
    }

}
