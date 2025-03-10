using UnityEngine;
public class ChestDrops : MonoBehaviour
{
    public LootItem[] loots; // Danh sách vật phẩm có thể rơi
    private Animator animator;
    private bool isOpened = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!isOpened && other.CompareTag("Player"))
        {
            OpenChest();
        }
    }
    void OpenChest()
    {
        isOpened = true;
        animator.SetTrigger("ChestOpen");
    }

    //Thêm vào sự kiện animation
    public void DropLoot()
    {
        for (int i = 0; i < 3; i++) 
        {
            GameObject item = GetRandomLoot();
            if (item != null)
            {
                Vector3 dropPosition = transform.position + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);
                Instantiate(item, dropPosition, Quaternion.identity);
            }
        }
    }

    GameObject GetRandomLoot()
    {
        float randomValue = Random.Range(0f, 1f);
        float cumulativeProbability = 0f;

        foreach (LootItem loot in loots)
        {
            cumulativeProbability += loot.dropChance;
            if (randomValue <= cumulativeProbability)
            {
                return loot.itemPrefab;
            }
        }
        return null;
    }
}
