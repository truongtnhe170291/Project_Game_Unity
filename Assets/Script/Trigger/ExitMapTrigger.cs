using Assets.Helper;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerScript : MonoBehaviour
{
    public string nextSceneName = "SelectMap"; 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerPrefs.SetString("NextScene", nextSceneName);
            int currentLevel = PlayerPrefs.GetInt("CurrentLevel");
            DoorData.StatusDoors[currentLevel - 1] = 3;
            if(currentLevel < DoorData.StatusDoors.Count)
                DoorData.StatusDoors[currentLevel] = 0;

            //DoorData.StatusDoors[DoorData.DoorId - 1] = 1;
            SceneManager.LoadScene("LoadScene");
        }
    }
}
