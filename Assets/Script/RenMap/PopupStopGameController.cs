using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Assets.Helper;
using System;
using UnityEngine.SceneManagement;

public class PopupStopGameController : MonoBehaviour
{
	public GameObject popupPanelBackground;
	public GameObject popupPanelStop;
	public GameObject popupPaneQuit;
	public Button StopButton;
	public Button ExitButton;
	public Button PlayButton;
	public Button Quit1Button;
	public Button Quit2Button;
	public Button CancelButton;

	private void Start()
	{
		popupPanelBackground.SetActive(false);
		popupPanelStop.SetActive(false);
		popupPaneQuit.SetActive(false);

		StopButton.onClick.AddListener(OnStopButtonClicked);
		ExitButton.onClick.AddListener(OnExitButtonClicked);
		CancelButton.onClick.AddListener(OnCancelButtonClicked);
		PlayButton.onClick.AddListener(OnPlayButtonClicked);
		Quit1Button.onClick.AddListener(OnQuit1ButtonClicked);
		Quit2Button.onClick.AddListener(OnQuit2ButtonClicked);
	}

	private void OnExitButtonClicked()
	{
		popupPanelBackground.SetActive(false);
		popupPanelStop.SetActive(false);
		popupPaneQuit.SetActive(false);

		popupPanelBackground.SetActive(true);
		popupPaneQuit.SetActive(true);
		Time.timeScale = 0f;
	}

	private void OnStopButtonClicked()
	{
		popupPanelBackground.SetActive(false);
		popupPanelStop.SetActive(false);
		popupPaneQuit.SetActive(false);

		popupPanelBackground.SetActive(true);
		popupPanelStop.SetActive(true);
		Time.timeScale = 0f;
	}

	private void OnQuit1ButtonClicked()
	{
		RenderMap renderMap = FindObjectOfType<RenderMap>();
		renderMap.SaveMap();
		PlayerPrefs.SetString("NextScene", "SelectMap");
		SceneManager.LoadScene("LoadScene");
	}

	private void OnQuit2ButtonClicked()
	{
		RenderMap renderMap = FindObjectOfType<RenderMap>();
		renderMap.SaveMap();
		PlayerPrefs.SetString("NextScene", "SelectMap");
		SceneManager.LoadScene("LoadScene");
	}

	private void OnPlayButtonClicked()
	{
		popupPanelBackground.SetActive(false);
		popupPanelStop.SetActive(false);
		popupPaneQuit.SetActive(false);
		Time.timeScale = 1f;
	}

	private void OnCancelButtonClicked()
	{
		popupPanelBackground.SetActive(false);
		popupPanelStop.SetActive(false);
		popupPaneQuit.SetActive(false);
		Time.timeScale = 1f;  // Tiếp tục game
	}
}
