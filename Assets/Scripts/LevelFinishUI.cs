using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SocialPlatforms.Impl;

public class LevelFinishUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI brokenObjText;
    [SerializeField] TextMeshProUGUI timeText;
    [SerializeField] TextMeshProUGUI scoreText;

    public void updateText(string achieve, string time, string score)
    {
        brokenObjText.text = achieve;
        timeText.text = time;
        scoreText.text = score;
    }
}
