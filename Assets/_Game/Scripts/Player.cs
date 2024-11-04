using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float speed = 5;
    [SerializeField] private Animator anim;
    [SerializeField] private float jumpForce = 350;


    private bool isGround = true;
    private bool isJumping = false;
    private bool isAttack = false;

    private string currentAnimName;
    private float horizontal;
    //private float vertical;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        isGround = CheckGrounded();

        horizontal = Input.GetAxisRaw("Horizontal");
        //vertical = Input.GetAxisRaw("Vertical");
        if(isAttack)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        if (isGround)
        {
            if (isJumping)
            {
                return;
            }
            //Jump
            Jump();
            //Run
            if (Mathf.Abs(horizontal) > 0.1f)
            {
                ChangeAnim("run");
            }

            //Attack
            if (Input.GetKey(KeyCode.F) && isGround)
            {
                Attack();
            }
            //Throw
            if (Input.GetKey(KeyCode.C) && isGround)
            {
                Throw();
            }
         }
        //Fall
        if (!isGround && rb.velocity.y < 0)
        {
            isJumping = false;
            ChangeAnim("fall");
        }

        //Moving
        if (Mathf.Abs(horizontal) > 0.1f)
        {
            ChangeAnim("run");
            rb.velocity = new Vector2(horizontal * Time.fixedDeltaTime * speed, rb.velocity.y);

            transform.rotation = Quaternion.Euler(new Vector3(0, horizontal > 0 ? 0 : 180, 0));
            //transform.localScale= new Vector3(horizontal, 1, 1);
        }
        else if(isGround) 
        {
            ChangeAnim("idle");
            rb.velocity = Vector2.zero;
        }
    }
    /***********Lazer check ground*/
    private bool CheckGrounded()
    {
        Debug.DrawLine(transform.position, transform.position + Vector3.down * 1.1f, Color.red);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.1f, groundLayer);
        return hit.collider != null;
    }

    /********Action and animation*/
    private void Attack()
    {
       ChangeAnim("attack");
       isAttack = true;
       Invoke(nameof(ResetAttack), 0.5f);
    }

    private void Throw()
    {
        ChangeAnim("throw");
        isAttack = true;
        Invoke(nameof(ResetAttack), 0.5f);
    }

    private void ResetAttack()
    {
        isAttack = false;
        ChangeAnim("ilde");
    }
    private void Run()
    {

    }

    private void Jump()
    {
        if (Input.GetKey(KeyCode.Space) && isGround)
        {
            isJumping = true;
            ChangeAnim("jump");
            rb.AddForce(jumpForce * Vector2.up);
        }
    }

    /*********Change anim***************/
    private void ChangeAnim(string animName)
    {
        if (currentAnimName != animName)
        {
            currentAnimName = animName;
            anim.SetTrigger(currentAnimName);      
        }
    }
}
