using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    // Start is called before the first frame update
    public Image HealthImage;
    public TextMeshProUGUI textHealth;
    public void UpdateHealth(int currentBool, int maxBool)
    {
        if (currentBool >= 0)
        {
            HealthImage.fillAmount = (float)currentBool / (float)maxBool;
            textHealth.text = $"{currentBool} / {maxBool}";
        }
    }
}
