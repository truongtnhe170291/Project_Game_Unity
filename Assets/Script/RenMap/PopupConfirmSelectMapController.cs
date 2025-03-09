using UnityEngine;
using UnityEngine.UI; // Dùng nếu không dùng TextMeshPro
using TMPro;
using Assets.Helper;
using UnityEngine.SceneManagement;         // Dùng nếu sử dụng TextMeshPro

public class PopupConfirmSelectMapController : MonoBehaviour
{
    public GameObject popupPanel;  
    public Button playNewButton;       
    public Button continueButton;       
    public Button noButton;        

    private void Start()
    {
        popupPanel.SetActive(false);

		playNewButton.onClick.AddListener(OnPlayNewButtonClicked);
		continueButton.onClick.AddListener(OnContinueButtonClicked);
        noButton.onClick.AddListener(OnNoButtonClicked);
    }

    public void ShowPopup()
    {
        popupPanel.SetActive(true);
        Time.timeScale = 0f;  // Tạm dừng game
    }

    private void OnPlayNewButtonClicked()
    {
        Debug.Log("Player agree new game!");

        popupPanel.SetActive(false);
        Time.timeScale = 1f;  // Tiếp tục game
        int levelNumber = PlayerPrefs.GetInt("levelNumber");
        PlayerPrefs.SetString(PlayerPrefsHelper.NextScene, "CharacterSelectionScene");
        SceneManager.LoadScene("LoadScene");
    }

	private void OnContinueButtonClicked()
	{
		Debug.Log("Player agree new game!");
        PlayerPrefs.SetString(PlayerPrefsHelper.IsContinue, "True");
		popupPanel.SetActive(false);
		Time.timeScale = 1f;  // Tiếp tục game
		
        PlayerPrefs.SetString(PlayerPrefsHelper.NextScene, "CharacterSelectionScene");
        SceneManager.LoadScene("LoadScene");
        //GameManager.Instance.StartLevel(levelNumber);
	}

	private void OnNoButtonClicked()
    {
        Debug.Log("Người chơi đã từ chối!");

        popupPanel.SetActive(false);
        Time.timeScale = 1f;  // Tiếp tục game
    }
}
