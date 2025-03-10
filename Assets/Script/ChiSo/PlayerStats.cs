using UnityEngine;
using System.IO;

public class PlayerStats : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public int attack = 10;
    public int defense = 5;
    public int moveSpeed = 5;
    public int experience = 0;
    public int level = 1;
    public int expToNextLevel = 100;

    public TextBar healthBar;
    public TextBar levelText;
    public Player playerMove;
    public EnemyStats enemyStats;

    private string filePath;

    void Start()
    {
        healthBar = GameObject.Find("HealthBar").GetComponent<TextBar>();

        playerMove = GameObject.FindWithTag("Player").GetComponent<Player>();
        enemyStats = GameObject.FindWithTag("EnemyCanShoot").GetComponent<EnemyStats>();
        // Tìm ExpBar bằng tên
        levelText = GameObject.Find("ExpBar").GetComponent<TextBar>();

        if (healthBar == null || levelText == null)
        {
            Debug.LogWarning("Không tìm thấy HealthBar hoặc ExpBar.");
        }
        ConfigPathForPlayerData();
        LoadPlayerData();
        UpdateUI();
    }

    public void ConfigPathForPlayerData()
    {
        string directoryPath = Path.Combine(Application.dataPath, "Script", "ChiSo");

        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        filePath = Path.Combine(directoryPath, "PlayerData.json");
    }

    //public void SavePlayerData()
    //{
    //    PlayerData data = new PlayerData
    //    {
    //        maxHealth = maxHealth,
    //        attack = attack,
    //        defense = defense,
    //        moveSpeed = moveSpeed,
    //    };

    //    string json = JsonUtility.ToJson(data, true);
    //    File.WriteAllText(filePath, json);
    //}

    public void LoadPlayerData()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            PlayerData data = JsonUtility.FromJson<PlayerData>(json);

            maxHealth = data.maxHealth;
            currentHealth = maxHealth;
            attack = data.attack;
            defense = data.defense;
            moveSpeed = data.moveSpeed;
        }
        else
        {
            Debug.LogWarning("Error Load Player Data.");
        }
    }

    public void IncreaseStat(string stat, int value)
    {
        switch (stat)
        {
            case "maxHealth":
                maxHealth += value;
                currentHealth += value;
                UpdateHealthBar();
                break;
            case "health":
                currentHealth = Mathf.Min(currentHealth + value, maxHealth);
                UpdateHealthBar();
                break;
            case "attack":
                attack += value;
                break;
            case "defense":
                defense += value;
                break;
            case "speed":
                moveSpeed += value;
                playerMove.moveSpeed += value;
                break;
            case "exp":
                GainExperience(value);
                break;
        }
    }

    public void ReduceStat(string stat, int value)
    {
        switch (stat)
        {
            case "maxHealth":
                maxHealth -= value;
                currentHealth = Mathf.Max(0, currentHealth - value);
                UpdateHealthBar();
                CheckDeath();
                break;
            case "health":
                value = enemyStats.attack;
                int actualDamage = Mathf.Max(value - defense, 0); // Giáp giảm sát thương, không âm máu
                currentHealth = Mathf.Max(0, currentHealth - actualDamage);
                UpdateHealthBar();
                CheckDeath();
                break;
            case "attack":
                attack -= value;
                break;
            case "defense":
                defense -= value;
                break;
            case "speed":
                moveSpeed -= value;
                playerMove.moveSpeed -= value;
                break;
        }
    }

    public void CheckDeath()
    {
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void UpdateHealthBar()
    {
        healthBar.UpdateHealthBar(currentHealth, maxHealth);
    }

    public void GainExperience(int amount)
    {
        experience += amount;

        while (experience >= expToNextLevel)
        {
            LevelUp();
        }
    }

    public void LevelUp()
    {
        experience -= expToNextLevel;
        level++;
        expToNextLevel = CalculateExpForNextLevel();
        UpdateExpUI();
    }

    public int CalculateExpForNextLevel()
    {
        return (int)(expToNextLevel * 1.5f);
    }

    public void UpdateExpUI()
    {
        if (levelText != null)
        {
            levelText.UpdateExpBar(experience, expToNextLevel, level);
        }
    }

    void UpdateUI()
    {
        healthBar.UpdateHealthBar(currentHealth, maxHealth);
        levelText.UpdateExpBar(experience, expToNextLevel, level);
    }
}
