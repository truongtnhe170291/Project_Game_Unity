// GameManager.cs
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int currentLevel { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            currentLevel = PlayerPrefs.GetInt("CurrentLevel", 1); // Load từ save
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Gọi từ scene chọn màn chơi (button click)
    public void StartLevel(int level)
    {
        Debug.Log($"Load level: {level}");
        currentLevel = level;
        PlayerPrefs.SetInt("CurrentLevel", level);
        PlayerPrefs.SetString("NextScene", "DemoRenMap");
        SceneManager.LoadScene("LoadScene");
    }

    // Gọi khi hoàn thành màn chơi
    public void CompleteLevel()
    {
        currentLevel++;
        PlayerPrefs.SetInt("CurrentLevel", currentLevel);
        SceneManager.LoadScene("RenderMap"); // Load lại scene với level mới
    }
}