using Assets.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Script.Common
{
	public class BackManager : MonoBehaviour
	{
		//public Button button;

		//private void Start()
		//{
		//	button.onClick.AddListener(() => OnBackButtonClicked(""));
		//}
		public void GoBack()
		{
			// Logic để quay lại trang trước hoặc màn hình trước.
			Debug.Log("Button clicked, going back!");
		}
		public void OnBackButtonClicked(string nextScene)
		{
			Debug.Log("Button clicked to next sen"+ nextScene);
			PlayerPrefs.SetString(PlayerPrefsHelper.NextScene, nextScene);
			SceneManager.LoadScene("LoadScene");
		}
	}
}
