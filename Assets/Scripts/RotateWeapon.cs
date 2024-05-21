using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateWeapon : MonoBehaviour
{
    [SerializeField] private Transform playerLocation;
    private PlayerMovement playerMove;

    FacingDirection currDir = FacingDirection.Right;

    //For the swing effect
    [SerializeField] GameObject swingEffect;
    [SerializeField] float swingDist = 0.5f;

    [SerializeField] float distance = 0.5f;

    [SerializeField] float swingRadius = 1f;
    [SerializeField] public float swingTime = 0.5f;

    private bool isEnabled = true;

    bool isSwinging = false;
    bool hasSwungInDir = false;

    [SerializeField] private Animator swingAnim;

    private void Awake()
    {
        playerMove = GetComponentInParent<PlayerMovement>();
        swingAnim = swingEffect.GetComponent<Animator>();
        currDir = playerMove.GetDirection();
        UpdateDirection();
    }

    private void Update()
    {
        if (isEnabled)
        {
            UpdateDirection();

        }

    }

    public void SwingWeapon()
    {
        isSwinging = true;
        swingAnim.SetTrigger("Swing");
        StartCoroutine("Swing");
    }


    private IEnumerator Swing()
    {
        Quaternion startingRot = transform.rotation;

        Quaternion targetRot;
        
        if (!hasSwungInDir){ 
            targetRot = startingRot* Quaternion.Euler(0, 0, -180);
        }
        else
        {
            targetRot = startingRot * Quaternion.Euler(0, 0, 180);
        }

        float timer = 0f;

        while (timer < swingTime)
        {
            transform.rotation = Quaternion.Lerp(startingRot, targetRot, timer / swingTime);
            timer += Time.deltaTime;
            yield return null;
        }

        transform.rotation = targetRot;
        hasSwungInDir = !hasSwungInDir;
        isSwinging = false;
    }


    public void DisableWeapon()
    {
        swingEffect.GetComponent<SpriteRenderer>().color = Color.clear;
        GetComponent<SpriteRenderer>().color = Color.clear;
        isEnabled = false;
    }

    public void EnableWeapon()
    {
        swingEffect.GetComponent<SpriteRenderer>().color = Color.white;
        GetComponent<SpriteRenderer>().color = Color.white;
        isEnabled = true;
        UpdateDirection();
    }

    private void UpdateDirection()
    {
        //Update the weapon direction if not in the correct place
        if (currDir != playerMove.GetDirection() && !isSwinging)
        {
            hasSwungInDir = false;
            currDir = playerMove.GetDirection();
            switch (currDir)
            {
                case FacingDirection.Up:
                    transform.position = playerLocation.position + new Vector3(0, distance, 0);
                    transform.rotation = Quaternion.Euler(0, 0, -90);
                    swingEffect.transform.position = playerLocation.position + new Vector3(0, swingDist, 0);
                    swingEffect.transform.rotation = Quaternion.Euler(0, 0, 0);
                    break;
                case FacingDirection.Down:
                    transform.position = playerLocation.position + new Vector3(0, -distance, 0);
                    transform.rotation = Quaternion.Euler(0, 0, 90);
                    swingEffect.transform.position = playerLocation.position + new Vector3(0, -swingDist, 0);
                    swingEffect.transform.rotation = Quaternion.Euler(0, 0, 180);
                    break;
                case FacingDirection.Right:
                    transform.position = playerLocation.position + new Vector3(distance, 0, 0);
                    transform.rotation = Quaternion.Euler(0, 0, -180);
                    swingEffect.transform.position = playerLocation.position + new Vector3(swingDist, 0, 0);
                    swingEffect.transform.rotation = Quaternion.Euler(0, 0, 270);
                    break;
                case FacingDirection.Left:
                    transform.position = playerLocation.position + new Vector3(-distance, 0, 0);
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                    swingEffect.transform.position = playerLocation.position + new Vector3(-swingDist, 0, 0);
                    swingEffect.transform.rotation = Quaternion.Euler(0, 0, 90);
                    break;
            }

        }
    }
    public void SetSwingSpeed(float speed)
    {
        swingTime = speed;
    }
}
