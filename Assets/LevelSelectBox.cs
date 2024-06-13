using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelectBox : MonoBehaviour
{
    [SerializeField] int levelNum = 0;
    [SerializeField] Image backgroundImg = null;
    [SerializeField] Sprite unlockedSpr = null;
    [SerializeField] Sprite lockedSpr = null;
    [SerializeField] TextMeshProUGUI numText = null;
    [SerializeField] TextMeshProUGUI timeText = null;
    [SerializeField] Button btn = null;


    private void Start()
    {
        UpdateBox();
    }

    public void UpdateBox()
    {
        numText.text = levelNum.ToString();

        //Check that the previous level has been cleared
        float time = PlayerPrefs.GetFloat("Level" + (levelNum-1) + "Time", -1f);
        float levelTime = PlayerPrefs.GetFloat("Level" + levelNum + "Time", -1f);
        if (levelTime != -1f)
        {
            timeText.text = TruncateDecimal(levelTime).ToString();
        }
        else
        {
            timeText.text = "";
        }


        if (levelNum == 0)
        {
            numText.text = "T";
            btn.interactable = true;
            backgroundImg.sprite = unlockedSpr;
        }
        else
        {
            if (time >= 0f)
            {
                btn.interactable = true;
                backgroundImg.sprite = unlockedSpr;
            }
            else if (time == -1 && levelNum == 1)
            {
                //Makes sure player can always access level 1
                btn.interactable = true;
                backgroundImg.sprite = unlockedSpr;
            }
            else
            {
                btn.interactable = false;
            }
        }

    }

    float TruncateDecimal(float value)
    {
        float multiplier = Mathf.Pow(10, 1);
        return Mathf.Floor(value * multiplier) / multiplier;
    }

    public void GoToLevel()
    {
        //Add 2 to level number (Scene 0 is the title, 1 is Level Select, 2 is the tutorial)
        SceneManager.LoadScene(levelNum+2);
    }
}
