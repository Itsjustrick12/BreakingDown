using UnityEngine;

public enum FacingDirection
{
    Up,
    Down,
    Right,
    Left
}

public class PlayerMovement : MonoBehaviour
{


    [SerializeField] float moveSpeed = 10f;
    private float currSpeed;
    private Vector2 moveDirection;

    private bool canMove = true;
    private FacingDirection direction = FacingDirection.Right;

    private Animator anim;

    private Rigidbody2D rb;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        currSpeed = moveSpeed;
        rb = GetComponent<Rigidbody2D>();
        changeDirection(FacingDirection.Down);
    }
    public void FixedUpdate()
    {
        if (canMove)
        {
            MovePlayer();
        }
    }

    private void getInputs()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        if (moveX < 0 )
        {
            changeDirection(FacingDirection.Left);
        }
        else if (moveX > 0)
        {
            changeDirection(FacingDirection.Right);
        }
        else if (moveY > 0)
        {
            changeDirection(FacingDirection.Up);
        }
        else if (moveY < 0)
        {
            changeDirection(FacingDirection.Down);
        }

        moveDirection = new Vector2(moveX, moveY);

        if (moveX != 0 || moveY != 0)
        {

            anim.SetFloat("Speed", 1);
        }
        else
        {
            anim.SetFloat("Speed", 0);
        }
    }

    private void MovePlayer()
    {
        getInputs();

        rb.velocity = new Vector2(moveDirection.x * currSpeed, moveDirection.y * currSpeed);
    }

    private void changeDirection(FacingDirection dir)
    {
        //If trying to change to a direction, make sure its different
        if (direction == dir)
        {
            return;
        }

        direction = dir;
        UpdateAnimator(dir);
    }

    public FacingDirection GetDirection()
    {
        return direction;
    }

    public void ChangeSpeed(float mulitplier)
    {
        if (mulitplier >= 1)
        {
            moveSpeed *= mulitplier;
        }
        else
        {
            currSpeed = moveSpeed * mulitplier; 
        }
    }

    public void ResetSpeed()
    {
        currSpeed = moveSpeed;
    }

    private void UpdateAnimator(FacingDirection dir)
    {

        switch (dir)
        {
            case FacingDirection.Up:
                //Change hit direction
                anim.SetBool("FacingRight", false);
                anim.SetBool("FacingLeft", false);
                anim.SetBool("FacingUp", true);
                anim.SetBool("FacingDown", false);
                anim.SetTrigger("ChangeDir");
                break;
            case FacingDirection.Down:
                //Change hit direction
                anim.SetBool("FacingRight", false);
                anim.SetBool("FacingLeft", false);
                anim.SetBool("FacingUp", false);
                anim.SetBool("FacingDown", true);
                anim.SetTrigger("ChangeDir");
                break;
            case FacingDirection.Left:
                //Change hit direction
                anim.SetBool("FacingRight", false);
                anim.SetBool("FacingLeft", true);
                anim.SetBool("FacingUp", false);
                anim.SetBool("FacingDown", false);
                anim.SetTrigger("ChangeDir");
                break;
            case FacingDirection.Right:
                //Change hit direction
                anim.SetBool("FacingRight", true);
                anim.SetBool("FacingLeft", false);
                anim.SetBool("FacingUp", false);
                anim.SetBool("FacingDown", false);
                anim.SetTrigger("ChangeDir");
                break;
        }
    }

    public void DisableMovement()
    {
        canMove = false;
        rb.velocity = Vector2.zero;
        
    }

    public void EnableMovement()
    {
        canMove = true;
    }

    public void TriggerBreakdown()
    {
        anim.SetTrigger("Breakdown");
        AudioManager.instance.PlaySound("Cry");
    }

}


