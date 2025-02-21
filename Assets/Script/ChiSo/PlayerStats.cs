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
    public CharacterMove playerMove;

    private string filePath;

    void Start()
    {
        ConfigPathForPlayerData();
        LoadPlayerData();
        UpdateUI();
    }

    // void OnApplicationQuit()
    // {
    //     SavePlayerData(); // Lưu chỉ số khi thoát game
    // }

    public void ConfigPathForPlayerData()
    {
        string directoryPath = Path.Combine(Application.dataPath, "Script", "ChiSo");

        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        filePath = Path.Combine(directoryPath, "PlayerData.json");
    }

    public void SavePlayerData()
    {
        PlayerData data = new PlayerData
        {
            maxHealth = maxHealth,
            attack = attack,
            defense = defense,
            moveSpeed = moveSpeed,
        };

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(filePath, json);
    }

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
            case "heal":
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
                currentHealth -= value;
                UpdateHealthBar();
                break;
            case "health":
                currentHealth = Mathf.Min(currentHealth - value, maxHealth);
                UpdateHealthBar();
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
