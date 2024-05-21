using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PolygonCollider2D))]
public class BrokenPiece : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer sprite;

    [SerializeField] float waitToFade = 1f;
    [SerializeField] private float fadeTime = 3f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        StartCoroutine("FadeOut");
    }

    public IEnumerator FadeOut()
    {
        float timer = waitToFade;

        while (timer >= 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }

        timer = fadeTime;
        while (timer >= 0)
        {
            timer -= Time.deltaTime;

            //Progressively fade the color of the sprite
            float normalizedTime = 1 - (timer / fadeTime);
            Color newColor = new Color(255, 255, 255, Mathf.Lerp(1f, 0f, normalizedTime));
            sprite.color = newColor;

            yield return null;

        }

        //Destroy the object
        Destroy(this.gameObject);
    }

    public void AddForce(float force, Vector2 location)
    {
        //Determine where to apply forces
        Vector2 dir = ((Vector2)this.transform.position - location).normalized;
        dir.Normalize();
        rb.AddForce(dir * (force*1.5f), ForceMode2D.Impulse);
    }

    public void SetFadeTime(float time)
    {
        fadeTime = time;
    }
}
