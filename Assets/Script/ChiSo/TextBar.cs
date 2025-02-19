using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class TextBar : MonoBehaviour
{
    public Image fillBar;
    public TextMeshProUGUI valueText;

    public void UpdateBar (int currentValue, int maxValue){
        fillBar.fillAmount = (float)currentValue / (float)maxValue;
        valueText.text = currentValue.ToString() + " / " + maxValue.ToString();
    }

    public void UpdateExpBar (int currentValue, int maxValue, int level){
        fillBar.fillAmount = (float)currentValue / (float)maxValue;
        valueText.text = "Level: " + level;
    }
}
