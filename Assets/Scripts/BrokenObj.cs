using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum Weight
{
    Light,
    Medium,
    Heavy
}
public class BrokenObj : MonoBehaviour
{
    [SerializeField] private BrokenPiece[] pieces;

    public Weight objWeight = Weight.Medium;
    public int burstForce = 2;
    private float pieceMass = 1;
    [SerializeField] float fadeTime = 3;

    [SerializeField] bool BurstOverride = false;


    private void Awake()
    {
        switch (objWeight)
        {
            case Weight.Heavy:
                pieceMass = 5;
                break;
            case Weight.Medium:
                pieceMass = 1;
                break;
            case Weight.Light:
                pieceMass = 0.5f;
                break;
        }

        for (int i = 0; i < pieces.Length; i++)
        {
            pieces[i].SetFadeTime(fadeTime);
        }

        StartCoroutine(WaitToDestroy());
        
    }

    public void Shatter()
    {
        if (BurstOverride)
        {
            Burst();
            return;
        }

        for (int i = 0; i < pieces.Length; i++)
        {
            // Determine where to apply forces
            Vector2 dir = ((Vector2)pieces[i].transform.position - (Vector2)FindObjectOfType<PlayerMovement>().transform.position).normalized;
            dir.Normalize();

            Rigidbody2D pieceRB = pieces[i].GetComponent<Rigidbody2D>();
            pieceRB.mass = pieceMass;

            //pieceRB.AddForceAtPosition(Vector2.one * burstForce, this.transform.position);
            pieceRB.AddForce(dir * FindObjectOfType<PlayerAttack>().GetKnockBack(), ForceMode2D.Impulse);
        }
    }

    public void Burst()
    {

        for (int i = 0; i < pieces.Length; i++)
        {
            
            Rigidbody2D pieceRB = pieces[i].GetComponent<Rigidbody2D>();
            pieceRB.mass = pieceMass;

            float variableForce = burstForce/4 * Random.Range(0, 1);

            pieceRB.AddForceAtPosition(Vector2.one * (burstForce + variableForce), this.transform.position);
        }
    }

    private IEnumerator WaitToDestroy()
    {
        yield return new WaitForSeconds(fadeTime + 1);
        Destroy(this.gameObject);
    }
}
