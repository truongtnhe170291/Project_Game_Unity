// MapManager.cs
using Assets.Helper;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Welcome : MonoBehaviour
{
    [SerializeField]
    public Button newGameButton;
    [SerializeField]
    public Button continueButton;
    [SerializeField]
    public Button settingButton;

    private void Start()
    {
        newGameButton.onClick.AddListener(() => OnNewGameButtonClicked());
		continueButton.onClick.AddListener(() => OnContinueButtonClicked());
    }

    private void OnNewGameButtonClicked()
    {
        DoorData.StatusDoors = new List<int>() { 0, -1, -1, -1, -1, -1, -1, -1, -1 };
        PlayerPrefs.SetString("NextScene", "SelectMap");
        SceneManager.LoadScene("LoadScene");
    }

	private void OnContinueButtonClicked()
	{
        DoorData.StatusDoors = DoorData.GetListDoorInJsonFile();
		PlayerPrefs.SetString("NextScene", "SelectMap");
		SceneManager.LoadScene("LoadScene");
	}
}