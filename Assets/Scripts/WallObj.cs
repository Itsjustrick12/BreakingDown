using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallObj : BreakableObj
{
    [SerializeField ]private float damageGate = 3f;
    public override void HitObj(float damage)
    {

        if (damage >= damageGate)
        {
            //Progressively apply damage
            objHealth -= damage;

            if (flashEffect != null)
            {
                flashEffect.HitFlash();
            }
            if (objHealth <= 0)
            {
                FindObjectOfType<GameManager>().SwitchCameraConfiner();
                BreakObj();
            }
        }

    }

    public override void BreakObj()
    {
        //Add score and break object into pieces

        //Add score to player's current score
        AudioManager.instance.PlayUniqueSound(objName);
        if (GameManager.instance.doCountWalls)
        {
            GameManager.instance.AddScore(points);
        }

        GameObject broke = Instantiate(brokenObj);
        broke.transform.position = transform.position;

        if (breakType == BreakType.Burst)
        {
            broke.GetComponent<BrokenObj>().Burst();

        }
        else
        {
            broke.GetComponent<BrokenObj>().Shatter();
        }

        Destroy(this.gameObject);
    }
    //Do not take knockback from hits
    public override void HitObj(float damage, Vector2 hitLocation, float knockForce, bool hitByPlayer)
    {
        HitObj(damage);
    }
    }
