using UnityEngine;
using System.IO;

public class EnemyStats : MonoBehaviour
{
    public int maxHealth = 30;
    public int currentHealth;
    public int attack = 10;
    public int defense = 0;
    public int moveSpeed = 4;

    public PlayerStats playerStats;

    private string filePath;

    void Start()
    {
        playerStats = GameObject.FindWithTag("Player").GetComponent<PlayerStats>();

        currentHealth = maxHealth;
        ConfigPathForEnemyData();
        LoadEnemyData();
    }

    public void ConfigPathForEnemyData()
    {
        string directoryPath = Path.Combine(Application.dataPath, "Script", "ChiSo");

        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        filePath = Path.Combine(directoryPath, "EnemyData.json");
    }

    public void LoadEnemyData()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            EnemyData data = JsonUtility.FromJson<EnemyData>(json);

            maxHealth = data.maxHealth;
            currentHealth = maxHealth;
            attack = data.attack;
            defense = data.defense;
            moveSpeed = data.moveSpeed;
        }
        else
        {
            Debug.LogWarning("Error Load Enemy Data.");
        }
    }

    public void ReduceStat(string stat, int value)
    {
        switch (stat)
        {
            case "health":
                value = playerStats.attack;
                int actualDamage = Mathf.Max(value - defense, 0); // Giáp giảm sát thương, không âm máu
                currentHealth = Mathf.Max(0, currentHealth - actualDamage);
                if (currentHealth <= 0)
                {
                    Destroy(gameObject);
                }
                break;
        }
    }
}
