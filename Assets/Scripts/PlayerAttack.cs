using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using Cinemachine;


[RequireComponent(typeof(CinemachineImpulseSource))]
public class PlayerAttack : MonoBehaviour
{
    private PlayerInteract playerI;
    private PlayerMovement playerMove;

    [SerializeField] private Transform attackPos;
    [SerializeField] private float attackRadius = 2;
    
    //STATS THAT CAN BE UPGRADED

    //Used to prevent the player from spamming the button
    [SerializeField] private float AttackCooldown = 0.5f;
    [SerializeField] private float attackSpeed = 0.25f;
    [SerializeField] private float attackDamage = 1;

    private CinemachineImpulseSource source;

    [SerializeField] private float spinSpeed = 1;

    private bool canAttack = true;

    private RotateWeapon weapon;

    [SerializeField] private float knockBackForce = 5f;

    [SerializeField] LayerMask whatCanBreak;

    private void Awake()
    {
        playerI = GetComponent<PlayerInteract>();
        playerMove = GetComponent<PlayerMovement>();
        weapon = GetComponentInChildren<RotateWeapon>();
        source = GetComponent<CinemachineImpulseSource>();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Vector3 drawPos = attackPos.position;
        Gizmos.DrawWireSphere(drawPos, attackRadius);
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space) && canAttack)
        {
            canAttack = false;
            Attack();
            weapon.SetSwingSpeed(attackSpeed);
            weapon.SwingWeapon();
            StartCoroutine("StartCooldown");
        }
    }

    public void Attack()
    {
        bool hasShaken = false;

        if (!playerI.isHoldingItem)
        {
            Collider2D[] objs = Physics2D.OverlapCircleAll(attackPos.position, attackRadius);

            //If theres objects apply damage to all
            if (objs.Length > 0)
            {

                for (int i = 0; i < objs.Length; i++)
                {
                    //Try to find an object that can break
                    if (objs[i].gameObject.GetComponent<IBreakable>() != null)
                    {

                        //Apply shake if at least one of the objects hit is breakable
                        if (!hasShaken && objs[i].gameObject.GetComponent<WallObj>() == null)
                        {
                            hasShaken = true;
                            ApplyHitShake();
                            AudioManager.instance.PlayUniqueSound("Hit");
                        }

                        //Apply damage to objs
                        IBreakable breakable = objs[i].gameObject.GetComponent<IBreakable>();
                        breakable.HitObj(attackDamage, attackPos.position, knockBackForce, true);
                    }
                    else if (objs[i].gameObject.GetComponent<BrokenPiece>() != null)
                    {
                        objs[i].GetComponent<BrokenPiece>().AddForce(knockBackForce, transform.position);
                    }
                }
            }
        }
    }

    private IEnumerator StartCooldown()
    {
        yield return new WaitForSeconds(AttackCooldown);
        canAttack = true;

    }

    public float GetKnockBack()
    {
        return knockBackForce;
    }

    public void ApplyHitShake()
    {
        Debug.Log("Shake Generated");
        source.GenerateImpulse();
    }

    public void IncreaseAtk(float dmg)
    {
        attackDamage += dmg;
    }

    public void IncreaseStats(float dmg, float cooldown, float knockback)
    {
        attackDamage += dmg;

        if (cooldown < AttackCooldown && cooldown > 0)
        {
            AttackCooldown = cooldown;
        }
        knockBackForce += knockback;
    }

    public void DisableAtk()
    {
        canAttack = false;
        weapon.DisableWeapon();
    }

    public void EnableAtk()
    {
        canAttack = true;
        weapon.EnableWeapon();
    }
}
