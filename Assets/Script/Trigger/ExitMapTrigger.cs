﻿using Assets.Helper;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerScript : MonoBehaviour
{
    public string nextSceneName = "SelectMap"; 

    private void OnTriggerEnter2D(Collider2D collision)
    {
		var enemies1 = GameObject.FindGameObjectsWithTag("EnemyCanShoot");
		var enemies2 = GameObject.FindGameObjectsWithTag("EnemyCanNotShoot");
		if (enemies1.Length > 0 || enemies2.Length > 0) return;
        if (collision.CompareTag("Player"))
        {
            PlayerPrefs.SetString(PlayerPrefsHelper.NextScene, nextSceneName);
            int currentLevel = PlayerPrefs.GetInt(PlayerPrefsHelper.CurrentLevel);
			if (currentLevel < DoorData.StatusDoors.Count)
				DoorData.StatusDoors[currentLevel - 1] = 3;

			if(currentLevel < DoorData.StatusDoors.Count - 1)
				DoorData.StatusDoors[currentLevel] = 0;

			DoorData.UpdateListDoorInJsonFile(DoorData.StatusDoors);
			//DoorData.StatusDoors[DoorData.DoorId - 1] = 1;

			// kiểm tra xem hoàn thành nhiệm vụ đủ 3s chưa. nếu hoàn thành thì xóa file json lưu data map này
			DeleteJsonFile($"map_{currentLevel}_save");

			SceneManager.LoadScene("LoadScene");
        }
    }

	private void DeleteJsonFile(string fileName)
	{
		string path = Path.Combine(Application.persistentDataPath, $"{fileName}.json");

		// Kiểm tra nếu file tồn tại
		if (File.Exists(path))
		{
			// Xóa file
			File.Delete(path);
			Debug.Log($"File {fileName}.json đã được xóa.");
		}
		else
		{
			Debug.LogError($"File {fileName}.json không tồn tại.");
		}
	}
}
