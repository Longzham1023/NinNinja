using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float speed = 5;
    [SerializeField] private float jumpForce = 350;
    [SerializeField] private Kunai kunaiPrefab;
    [SerializeField] private Transform throwPoint;
    [SerializeField] private GameObject meleePoint;

    private bool isGround = true;
    private bool isJumping = false;
    private bool isAttack = false;
    private bool isDeath = false;

    private int coin = 0;

    
    private float horizontal;
    //private float vertical;

    // Start is called before the first frame update
    private Vector3 savePoint;


    // Update is called once per frame
    void Update()
    {
        Debug.Log("Update");
        Debug.LogError("Update");
        if(isDead)
        {
            return;
        }
        isGround = CheckGrounded();

        //horizontal = Input.GetAxisRaw("Horizontal");
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
            if (Input.GetKey(KeyCode.Space) && isGround)
            {
                Jump();
            }
            //Run
            if (Mathf.Abs(horizontal) > 0.1f)
            {
                ChangeAnim("run");
            }

            //Attack
            if (Input.GetKey(KeyCode.F))
            {
                Attack();
            }
            //Throw
            if (Input.GetKey(KeyCode.V))
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
            //ChangeAnim("run");
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);

            transform.rotation = Quaternion.Euler(new Vector3(0, horizontal > 0 ? 0 : 180, 0));
            //transform.localScale= new Vector3(horizontal, 1, 1);
        }
        else if(isGround) 
        {
            ChangeAnim("idle");
            rb.velocity = Vector2.zero;
        }
    }
    /*****Reset position*////
    public override void OnInit()
    {
        base.OnInit();
        isAttack = false;

        transform.position = savePoint;
        ChangeAnim("idle");
        DeActiveAttack();

        SavePoint();
        UIManager.Instance.SetCoin(coin);
    }
    protected override void OnDeath()
    {
        base.OnDeath();
        if (coin >= 10)
        {
            coin -= 10;
            UIManager.Instance.SetCoin(coin);
        }
        else
        {
            coin = 0;
            UIManager.Instance.SetCoin(coin);
        }
    }
    public override void OnDespawn()
    {
        base.OnDespawn();
        OnInit();
    }
    /***********Lazer check ground*/
    private bool CheckGrounded()
    {
        Debug.DrawLine(transform.position, transform.position + Vector3.down * 1.1f, Color.red);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.1f, groundLayer);
        return hit.collider != null;
    }

    /********Action and animation*/
    public void Attack()
    {
       ChangeAnim("attack");
       isAttack = true;
       Invoke(nameof(ResetAttack), 0.5f);
       ActiveAttack();
       Invoke(nameof(DeActiveAttack), 0.5f);
    }

    public void Throw()
    {
        ChangeAnim("throw");
        isAttack = true;
        Invoke(nameof(ResetAttack), 0.5f);

        Instantiate(kunaiPrefab, throwPoint.position, throwPoint.rotation);
    }

    private void ResetAttack()
    {
        isAttack = false;
        ChangeAnim("ilde");
    }

    //public void Jump()
    //{
    //    if (Input.GetKey(KeyCode.Space) && isGround)
    //    {
    //        isJumping = true;
    //        ChangeAnim("jump");
    //        rb.AddForce(jumpForce * Vector2.up);
    //    }
    //}
    public void Jump()
    {
        if (isGround)
        {
            isJumping = true;
            ChangeAnim("jump");
            rb.AddForce(jumpForce * Vector2.up);
        }
    }

    /*******Save Point*******/
    internal void SavePoint()
    {
        savePoint = transform.position;
    }
    private void ActiveAttack()
    {
        meleePoint.SetActive(true);
    }
    private void DeActiveAttack()
    {
        meleePoint.SetActive(false);
    }
    /*************Collider*/////////////////
    public void SetMove(float horizontal)
    {
        this.horizontal = horizontal;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Coin")
        {
            coin++;
            UIManager.Instance.SetCoin(coin);
            Destroy(collision.gameObject);
        }
        if(collision.tag == "DeadZone")
        {
            isDeath = true;
            ChangeAnim("die");
            Invoke(nameof(OnInit), 1f);
        }
    }
}
