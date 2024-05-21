using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public enum BreakType
{
    Shatter,
    Burst
}

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(WhiteHitEffect))]

public class BreakableObj : MonoBehaviour, IBreakable, IInteract
{
    public string objName = "Break";
    public int points = 1;
    public float objHealth = 3;
    public Rigidbody2D rb;
    public GameObject brokenObj;

    //Used to determine how to send the pieces flying
    protected BreakType breakType = BreakType.Burst;

    protected WhiteHitEffect flashEffect;

    private void Awake()
    {
        flashEffect = GetComponent<WhiteHitEffect>();
        rb = GetComponent<Rigidbody2D>();
    }

    //Deafault damage script
    public virtual void HitObj(float damage)
    {
        //Progressively apply damage
        objHealth -= damage;

        if (flashEffect != null)
        {
            flashEffect.HitFlash();
        }
        if (objHealth <= 0)
        {
            BreakObj();
        }

    }

    public virtual void HitObj(float damage, Vector2 hitLocation, float knockForce, bool hitByPlayer)
    {
        //Determine where to apply forces
        Vector2 dir = ( (Vector2)this.transform.position - hitLocation).normalized;
        dir.Normalize();
        rb.AddForce(dir * knockForce, ForceMode2D.Impulse);

        if (hitByPlayer)
        {
            breakType = BreakType.Shatter;
        }
        HitObj(damage);
    }

    public virtual void BreakObj()
    {
        //Add score and break object into pieces

        //Add score to player's current score

        GameManager.instance.AddScore(points);

        GameObject broke = Instantiate(brokenObj);
        broke.transform.position = transform.position;

        AudioManager.instance.PlayUniqueSound(objName);

        if (breakType ==BreakType.Burst)
        {
            broke.GetComponent<BrokenObj>().Burst();

        }
        else
        {
            broke.GetComponent<BrokenObj>().Shatter();
        }

        Destroy(this.gameObject);
    }

    public virtual void Interact()
    {
    }
}
