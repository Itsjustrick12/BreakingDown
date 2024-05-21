using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(SpriteRenderer))]
public class WhiteHitEffect : MonoBehaviour
{
    public Material whitemat;
    private Material prevMat;
    SpriteRenderer spriteRend;

    public float flashTime = 0.1f;
    private int numBlinks = 3;

    public void Awake()
    {
        spriteRend = GetComponent<SpriteRenderer>();
        prevMat = spriteRend.material;
    }
    public void HitFlash()
    {
       
        spriteRend.material = whitemat;

        //Give a delay to wait for effect to stop
        Invoke("ChangeBack", flashTime);
    }

    private void ChangeBack()
    {
        spriteRend.material = prevMat;
    }

    public void BlinkFlash(float pauseWait)
    {
        StartCoroutine(Blink(numBlinks, pauseWait/numBlinks));
    }

    public IEnumerator Blink(int amt, float pauseTime)
    {
        for (int i = 0; i < amt; i++)
        {
            HitFlash();
            yield return new WaitForSeconds(pauseTime);
        }
    }
}
