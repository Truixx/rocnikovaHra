using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Rigidbody2D projectileRB;
    public float speed;

    public float projectileLife;
    public float projectileCount;

    public CharacterController2D CharacterController2D;
    public bool facingRight;

    public Damage enemyHealth;

    void Start()
    {
        projectileCount = projectileLife;
        CharacterController2D = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController2D>();
        facingRight = CharacterController2D.m_FacingRight;
        if (!facingRight)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        projectileCount -= Time.deltaTime;
        if (projectileCount <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        if (facingRight)
        {
            projectileRB.velocity = new Vector2(speed, projectileRB.velocity.y);
        }
        else
        {
            projectileRB.velocity = new Vector2(-speed, projectileRB.velocity.y);
        }
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag =="Weak point")
        {
            Destroy(collision.gameObject);
        }

        Destroy(gameObject);
    }
    
}
