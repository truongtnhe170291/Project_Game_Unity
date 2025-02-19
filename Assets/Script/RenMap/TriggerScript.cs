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
            DoorData.StatusDoors[DoorData.DoorId - 1] = 1;
            SceneManager.LoadScene("LoadScene");
        }
    }
}
