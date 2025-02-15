using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Animator animator;

    public int health = 100;

    public void takeDamage(int damage)
    {
        health -= damage;
        animator.SetTrigger("Hurt");
       

        if (health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        animator.SetBool("isDead", true);
  
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;


    }

}
