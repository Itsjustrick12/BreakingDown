using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MessageBox : MonoBehaviour
{
    public Image[] images;
    public TextMeshProUGUI dialogueText;
    private float timer = 0f;
    private bool hidden = true;

    private void Update()
    {

        if (timer >= 0 && !hidden)
        {
            timer -= Time.deltaTime;
        }
        else if (timer <= 0 && !hidden) {
            HideBox();
        }
    }

    public void HideBox(float time)
    {
        if (timer <= 0.1)
        {

            Debug.Log("Resetting timer");
            timer = time;
        }
        else
        {
            Debug.Log("Adding to timer");
            timer += time;
        }
    }

    public void HideBox()
    {
        hidden = true;
        for (int i = 0; i < images.Length; i++)
        {
            images[i].color = Color.clear;
        }
        dialogueText.color = Color.clear;
    }

    public void ShowBox()
    {
        hidden = false;
        AudioManager.instance.PlaySound("UI Pop Up");

        for (int i = 0; i < images.Length; i++)
        {
            images[i].color = Color.white;
        }
        dialogueText.color = Color.black;
    }
    public void ShowBox(DialogueEffect dialogue)
    {
        UpdateBox(dialogue);
        ShowBox();
        HideBox(dialogue.duration);
    }

    private void UpdateBox(DialogueEffect dialogue)
    {
        images[0].sprite = dialogue.portraitSprite;
        dialogueText.text = dialogue.dialougeText;
    }
}
