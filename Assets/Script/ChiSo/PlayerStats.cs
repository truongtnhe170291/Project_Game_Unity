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
        // Định nghĩa đường dẫn file JSON trong thư mục ChiSo
        string directoryPath = Path.Combine(Application.dataPath, "Script", "ChiSo");

        // Kiểm tra nếu thư mục chưa tồn tại thì tạo mới
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        // Định nghĩa đường dẫn file JSON
        filePath = Path.Combine(directoryPath, "playerData.json");

        // Load dữ liệu từ file JSON khi game khởi động
        LoadPlayerData();
        UpdateUI();
    }

    void OnApplicationQuit()
    {
        SavePlayerData(); // Lưu chỉ số khi thoát game
    }

    public void SavePlayerData()
    {
        PlayerData data = new PlayerData
        {
            maxHealth = maxHealth,
            currentHealth = currentHealth,
            attack = attack,
            defense = defense,
            moveSpeed = moveSpeed,
            experience = experience,
            level = level,
            expToNextLevel = expToNextLevel
        };

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(filePath, json);
        Debug.Log("🔥 Dữ liệu đã được lưu vào: " + filePath);
    }

    public void LoadPlayerData()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            PlayerData data = JsonUtility.FromJson<PlayerData>(json);

            maxHealth = data.maxHealth;
            currentHealth = data.currentHealth;
            attack = data.attack;
            defense = data.defense;
            moveSpeed = data.moveSpeed;
            experience = data.experience;
            level = data.level;
            expToNextLevel = data.expToNextLevel;

            Debug.Log("✅ Dữ liệu đã được tải thành công từ: " + filePath);
        }
        else
        {
            Debug.LogWarning("⚠ Không tìm thấy file JSON, sử dụng chỉ số mặc định.");
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
