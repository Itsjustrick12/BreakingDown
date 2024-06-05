using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ThrowableObject : BreakableObj
{
    public Weight objWeight = Weight.Medium;
    [SerializeField] LayerMask collideWith;

    public bool isThrown = false;

    [SerializeField] float waitToDrag = 1f;
    private float prevDrag;
    private float prevMass;

    [SerializeField] int throwDamage = 3;
    [SerializeField] float throwForce = 10;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        flashEffect = GetComponent<WhiteHitEffect>();
        prevDrag = rb.drag;
        prevMass = rb.mass;
    }

    public void Throw(FacingDirection dir)
    {
        isThrown = true;
        rb.drag = 0;
        //rb.excludeLayers = LayerMask.NameToLayer("Shards");
        //GetComponent<BoxCollider2D>().excludeLayers = LayerMask.NameToLayer("Shards");

        switch (objWeight)
        {
            case Weight.Medium:
                rb.mass = 1;
                break;
            case Weight.Light:
                rb.mass = 0.5f;
                break;
            case Weight.Heavy:
                rb.mass = 2;
                break;
        }

        Vector2 throwDirection = Vector2.zero;

        switch (dir)
        {
            case FacingDirection.Up:
                throwDirection = Vector2.up;
                break;
            case FacingDirection.Down:
                //Change hit direction
                throwDirection = Vector2.down;
                break;
            case FacingDirection.Left:
                //Change hit direction
                throwDirection = Vector2.left;
                break;
            case FacingDirection.Right:
                //Change hit direction
                throwDirection = Vector2.right;
                break;
        }

        rb.AddForce(throwDirection * throwForce, ForceMode2D.Impulse);
        StartCoroutine("WaitForDrag");
    }

    public void Throw(FacingDirection dir, float force)
    {
        isThrown = true;
        rb.drag = 0;

        switch (objWeight)
        {
            case Weight.Medium:
                rb.mass = 1;
                break;
            case Weight.Light:
                rb.mass = 0.5f;
                break;
            case Weight.Heavy:
                rb.mass = 2;
                break;
        }

        Vector2 throwDirection = Vector2.zero;

        switch (dir)
        {
            case FacingDirection.Up:
                throwDirection = Vector2.up;
                break;
            case FacingDirection.Down:
                //Change hit direction
                throwDirection = Vector2.down;
                break;
            case FacingDirection.Left:
                //Change hit direction
                throwDirection = Vector2.left;
                break;
            case FacingDirection.Right:
                //Change hit direction
                throwDirection = Vector2.right;
                break;
        }

        rb.AddForce(throwDirection * force, ForceMode2D.Impulse);
        StartCoroutine("WaitForDrag");
    }

    public override void Interact()
    {

        if (!isThrown)
        {
            PlayerInteract player = FindObjectOfType<PlayerInteract>();

            if (player != null)
            {
                if (!player.isHoldingItem)
                {
                    player.PickupThrowable(this);
                }
            }
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        // Check if the collided object is in the "shards" layer
        if (collision.gameObject.layer == LayerMask.NameToLayer("Shards") && isThrown)
        {
            // Ignore collision with the collided object
            Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>(), true);
        }
        else if (isThrown && !collision.gameObject.CompareTag("Player"))
        {

            IBreakable breakObj = collision.gameObject.GetComponent<IBreakable>();

            //Reset the mask to allow obj to collide with player again
            rb.excludeLayers = 0;

            if (breakObj != null)
            {
                AudioManager.instance.PlayUniqueSound("Thud");
                breakObj.HitObj(throwDamage);
            }

            //Apply a subtle screen shake
            FindObjectOfType<PlayerAttack>().ApplyHitShake();

            HitObj(throwDamage);
            isThrown = false;
        }
    }

    private IEnumerator WaitForDrag()
    {
        float timer = waitToDrag;
        while (timer >= 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }
        isThrown = false;
        rb.drag = prevDrag;
        rb.mass = prevMass;
        rb.excludeLayers = 0;
    }
}
