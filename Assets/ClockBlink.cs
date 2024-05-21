using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ClockBlink : MonoBehaviour
{

    [SerializeField] Sprite normalSprite;
    [SerializeField] Sprite hurrySprite;
    private Image spriteRend;
    [SerializeField] private float flashTime = 3f;
    [SerializeField] private int numBlinks = 3;

    private void Awake()
    {
        spriteRend = GetComponent<Image>();
    }


    public void ChangeColor()
    {
        StartCoroutine(Blink(numBlinks,  flashTime / numBlinks));
    }

    public void ChangeColor(float time)
    {
        StartCoroutine(Blink(numBlinks, time / numBlinks));
    }

    public IEnumerator Blink(int amt, float pauseTime)
    {
        AudioManager.instance.PlaySound("Hurry");
        for (int i = 0; i < amt; i++)
        {
            spriteRend.sprite = hurrySprite;

            yield return new WaitForSeconds(pauseTime/2);

            spriteRend.sprite = normalSprite;

            yield return new WaitForSeconds(pauseTime/2);
        }

        spriteRend.sprite = hurrySprite;
    }
}
