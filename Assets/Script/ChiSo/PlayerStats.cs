using UnityEngine;
using System;
using TMPro;

// Player Stats
public class PlayerStats : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    public int attack = 10;
    public int defense = 5; 
    public int moveSpeed = 5;
    public int experience = 0;
    public int level = 1; // Cấp độ hiện tại
    public int expToNextLevel = 100; // EXP cần để lên level 2


    public TextBar healthBar;
    public TextBar levelText; // Thêm Text UI để hiển thị cấp độ
    public CharacterMove playerMove;


    void Start()
    {
        currentHealth = maxHealth;
        healthBar.UpdateBar(currentHealth, maxHealth);
        levelText.UpdateExpBar(experience, expToNextLevel, level);
        // levelText.text = "Level: " + level;
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
                currentHealth = Math.Min(currentHealth + value, maxHealth);
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

    public void UpdateHealthBar(){
        healthBar.UpdateBar(currentHealth, maxHealth);
    }

    public void GainExperience(int amount)
    {
        experience += amount; // Cộng exp

        // Kiểm tra nếu đủ exp để lên cấp
        while (experience >= expToNextLevel)
        {
            LevelUp();
        }
    }

    public void LevelUp()
    {
        experience -= expToNextLevel; // Giữ lại phần exp dư
        level++; // Tăng cấp
        expToNextLevel = CalculateExpForNextLevel(); // Cập nhật exp cần thiết cho cấp mới

        // Debug.Log("Chúc mừng! Bạn đã lên cấp " + level);
        UpdateExpUI();
    }

    public int CalculateExpForNextLevel()
    {
        return (int)(expToNextLevel * 1.5f); // EXP tăng 1.5x mỗi level
    }

    public void UpdateExpUI()
    {
        if (levelText != null)
        {
            levelText.UpdateExpBar(experience, expToNextLevel, level);
        }
    }


}