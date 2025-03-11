using UnityEngine;
using System.IO;

public class EnemyStats : MonoBehaviour
{
    public int maxHealth = 30;
    public int currentHealth;
    public int attack = 10;
    public int defense = 0;
    public int moveSpeed = 4;

    public RenderMap renderMap;

    public PlayerStats playerStats;

    private string filePath;

    void Start()
    {
        renderMap = GameObject.Find("Map").GetComponent<RenderMap>();
        playerStats = GameObject.FindWithTag("Player").GetComponent<PlayerStats>();

        currentHealth = maxHealth;
        ConfigPathForEnemyData();
        LoadEnemyData();
        UpdateStatEnemyByLevel();
        
    }

    public void UpdateStatEnemyByLevel()
    {
        int levelEnemy = renderMap.levelEnemy;

        // Hệ số tăng theo level (Level 1: +20%, Level 2: +40%, ...)
        float multiplier = 1.0f + (levelEnemy * 0.2f);

        maxHealth = (int)(maxHealth * multiplier);
        attack = (int)(attack * multiplier);
        defense = (int)(defense * multiplier);

        Debug.Log($"Cập nhật Enemy Level {levelEnemy}: maxHealth = {maxHealth}, attack = {attack}, defense = {defense}");
    }

    public void ConfigPathForEnemyData()
    {
        string directoryPath = Path.Combine(Application.dataPath, "Script", "ChiSo");

        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        if (gameObject.CompareTag("EnemyCanNotShoot"))
        {
            filePath = Path.Combine(directoryPath, "EnemyNoneShotData.json");
        }
        else
        {
            filePath = Path.Combine(directoryPath, "EnemyData.json");
        }
        
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
