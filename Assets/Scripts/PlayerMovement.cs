using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    

    public CharacterController2D controller;


    public Animator animator;

    float nextSlideTime = 0f;

    public float SlideRate = 2f;

    public float runSpeed = 40f;

    float horizontalMove = 0f;

    bool jump = false;

    bool crouch = false;

    bool sliding = false;

   

    // Update is called once per frame
    void Update()
    {

        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));


        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
            animator.SetBool("isJumping", true);
        }
        if(Input.GetButtonDown("Crouch"))
        {
            crouch = true;

        }
        else if(Input.GetButtonUp("Crouch"))
        {
            crouch = false;
        }
        else if (Input.GetButtonUp("Slide"))
        {
            sliding = false;
        }

        if (Time.time >= nextSlideTime)
        {
            if (Input.GetButtonDown("Slide"))
            {
                sliding = true;
                nextSlideTime = Time.time + 1f / SlideRate;

            }
           



        }



    }


    public void onLanding()
    {

        animator.SetBool("isJumping", false);

    }

    public void onCrouching(bool isCrouching)
    {

        animator.SetBool("isCrouching", isCrouching);
    
    }
    public void onSliding(bool isSliding)
    {

        animator.SetBool("isSliding", isSliding);

    }

 


    private void FixedUpdate()
    {

        controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump, sliding);
        jump = false;
    }


   



}
