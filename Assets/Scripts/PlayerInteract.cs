using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    private PlayerMovement playerMove;
    private PlayerAttack playerAtk;
    [SerializeField] LayerMask whatCanInteract;
    [SerializeField] LayerMask ignorePlayerLayer;

    public Transform objContainer;

    [SerializeField] Transform[] interactLocations;
    private int currInteractPoint = 0;
    private RotateWeapon weapon;

    public bool isHoldingItem = false;
    public ThrowableObject heldObj = null;
    [SerializeField] private Transform heldItemPos;

    private void Awake()
    {
        playerMove = GetComponent<PlayerMovement>();
        playerAtk = GetComponent<PlayerAttack>();
        weapon = GetComponentInChildren<RotateWeapon>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1) && !isHoldingItem)
        {
            //If interact button, try to interact
            Interact();
        }

        if (Input.GetMouseButtonDown(0))
        {
            FacingDirection dir = playerMove.GetDirection();


            switch (dir)
            {
                case FacingDirection.Up:
                    //Change hit direction
                    currInteractPoint = 0;

                    break;
                case FacingDirection.Down:
                    //Change hit direction
                    currInteractPoint = 1;

                    break;
                case FacingDirection.Left:
                    //Change hit direction
                    currInteractPoint = 2;

                    break;
                case FacingDirection.Right:
                    //Change hit direction
                    currInteractPoint = 3;
                    break;
            }
        }

        if (isHoldingItem && heldObj != null)
        {
            heldObj.transform.position = heldItemPos.position;
            if (Input.GetMouseButtonDown(0))
            {
                ThrowItem();
            }
        }
    }

    public void ThrowItem()
    {
        AudioManager.instance.PlaySound("ThrowObj");
        heldObj.transform.parent = objContainer;
        heldObj.Throw(playerMove.GetDirection());
        isHoldingItem = false;
        heldObj.GetComponent<SpriteRenderer>().sortingOrder = 0;
        heldObj = null;
        weapon.EnableWeapon();
        playerMove.ResetSpeed();

    }

    public void DropItem()
    {
        heldObj.transform.parent = objContainer;
        isHoldingItem = false;
        heldObj.GetComponent<SpriteRenderer>().sortingOrder = 0;
        heldObj = null;
        weapon.EnableWeapon();
        playerMove.ResetSpeed();

    }

    public void Interact()
    {
        //Get the player's facing direction
        FacingDirection dir = playerMove.GetDirection();

        Transform checkLocation = null;

        switch (dir)
        {
            case FacingDirection.Up:
                //Change hit direction
                currInteractPoint = 0;
                checkLocation = interactLocations[0];
                break;
            case FacingDirection.Down:
                //Change hit direction
                currInteractPoint = 1;
                checkLocation = interactLocations[1];
                break;
            case FacingDirection.Left:
                //Change hit direction
                currInteractPoint = 2;
                checkLocation = interactLocations[2];
                break;
            case FacingDirection.Right:
                //Change hit direction
                currInteractPoint = 3;
                checkLocation = interactLocations[3];
                break;
        }

        if (checkLocation != null)
        {
            Vector2 checkPoint = new Vector2(checkLocation.position.x, checkLocation.position.y);

            //Check the location for an interactable object and call it's interact function

            Collider2D[] objs = Physics2D.OverlapBoxAll(checkPoint, new Vector2(2, 2), 0, whatCanInteract);

            if (objs.Length > 0)
            {
                for (int i = 0; i < objs.Length; i++)
                {
                    if (objs[i].gameObject.GetComponent<IInteract>() != null && objs[i].gameObject.GetComponent<WallObj>() == null)
                    {
                        objs[i].gameObject.GetComponent<IInteract>().Interact();

                        //Only interact with one object
                        return;
                    }
                }

            }
        }
    }

    public void PickupThrowable(ThrowableObject obj)
    {
        if (heldObj == null)
        {
            AudioManager.instance.PlaySound("PickupObj");
            heldObj = obj;
            heldObj.rb.excludeLayers = ignorePlayerLayer;
            heldObj.rb.velocity = Vector2.zero;
            heldObj.transform.position = heldItemPos.position;
            heldObj.transform.parent = heldItemPos;
            heldObj.GetComponent<SpriteRenderer>().sortingOrder = 1;
            isHoldingItem = true;
            weapon.DisableWeapon();

            switch (heldObj.objWeight)
            {
                case Weight.Light:
                    playerMove.ChangeSpeed(0.75f);
                    break;
                case Weight.Medium:
                    playerMove.ChangeSpeed(0.5f);
                    break;
                case Weight.Heavy:
                    playerMove.ChangeSpeed(0.25f);
                    break;
            }
            
        }
    }
}
