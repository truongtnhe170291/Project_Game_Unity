using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    public GameObject gameOverPanel;
    public Button returnButton;

    void Start()
    {
        gameOverPanel.SetActive(false);
        returnButton.onClick.AddListener(ReturnToMap);
    }

    public void ShowGameOverScreen()
    {
        gameOverPanel.SetActive(true);
    }

    public void ReturnToMap()
    {
        SceneManager.LoadScene("SelectMap");
    }
}
