using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    private float hp;
    private string currentAnimName;
    [SerializeField] private Animator anim;
    [SerializeField] protected HeathBar heathBar;
    [SerializeField] protected CombatText CBtextPrefabs;

    public bool isDead => hp <= 0;

    private void Start()
    {
        OnInit();
    }

    public virtual void OnInit()
    {
        hp = 100;
        heathBar.OnInit(100, transform);
    }

    public virtual void OnDespawn()
    {

    }

    protected virtual void OnDeath()
    {
        ChangeAnim("die");
        Invoke(nameof(OnDespawn), 2f);
    }

    /*********Change anim***************/
    protected void ChangeAnim(string animName)
    {
        if (currentAnimName != animName)
        {
            currentAnimName = animName;
            anim.SetTrigger(currentAnimName);
        }
    }
    public virtual void OnHit(float damage)
    {
        Debug.Log("Hit");
        if(!isDead)
        {
            hp -=damage;
            if(isDead)
            {
                hp = 0;
                OnDeath();
            }
            heathBar.SetNewHp(hp);
            Instantiate(CBtextPrefabs, transform.position + Vector3.up, Quaternion.identity).OnInit(damage);
        }
    }
}
