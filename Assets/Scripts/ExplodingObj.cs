using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

[RequireComponent(typeof(CinemachineImpulseSource))]
public class ExplodingItem : ThrowableObject
{
    public float explosionRadius = 2f;
    public float explosionDamage = 5f;
    public float explosionForce = 5f;

    public float explosionPause = 2f;

    private CinemachineImpulseSource source;

    public override void BreakObj()
    {
        //Add score and break object into pieces

        //Add score to player's current score
        source = GetComponent<CinemachineImpulseSource>();
        AudioManager.instance.PlaySound(objName);
        StartCoroutine("WaitToExplode");

        
    }

    public void Explode()
    {
        if (FindObjectOfType<PlayerInteract>().heldObj == this)
        {
            FindObjectOfType<PlayerInteract>().DropItem();
        }

        Collider2D[] objs = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

        //Apply camera shake
        AudioManager.instance.PlayUniqueSound("Explosion");
        source.GenerateImpulse();
        //If theres objects apply damage to all
        if (objs.Length > 0)
        {
            for (int i = 0; i < objs.Length; i++)
            {
                //Try to find an object that can break
                if (objs[i].gameObject.GetComponent<IBreakable>() != null && objs[i].gameObject != this.gameObject)
                {
                    if (objs[i].gameObject.GetComponent<ExplodingItem>() != null)
                    {
                        objs[i].gameObject.GetComponent<ExplodingItem>().BreakObj();
                    }
                    else
                    {
                        //Apply damage to objs
                        IBreakable breakable = objs[i].gameObject.GetComponent<IBreakable>();
                        breakable.HitObj(explosionDamage, transform.position, explosionForce, false);
                    }


                }
                else if (objs[i].gameObject.GetComponent<BrokenPiece>() != null && objs[i].gameObject != this.gameObject)
                {
                        objs[i].GetComponent<BrokenPiece>().AddForce(explosionForce, transform.position);
                   
                }
            }
        }

        
    }

    private IEnumerator WaitToExplode()
    {
        flashEffect.BlinkFlash(explosionPause);
        yield return new WaitForSeconds(explosionPause);
        Explode();
        GameObject broke = Instantiate(brokenObj);
        broke.transform.position = transform.position;
        //Always explode this object
        broke.GetComponent<BrokenObj>().Burst();
        FindObjectOfType<GameManager>().AddScore(points);
        Destroy(this.gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 drawPos = transform.position;
        Gizmos.DrawWireSphere(drawPos, explosionRadius);
    }
}
