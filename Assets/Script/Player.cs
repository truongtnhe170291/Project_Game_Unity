using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = 5f;

    public SpriteRenderer characterSR;
    Animator animator;

    //public float dashBoost = 2f;
    //private float dashTime;
    //public float DashTime;
    //private bool once;

    Vector3 moveInput;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }


    // Update is called once per frame
    void Update()
    {
        /// Part 2
        // Movement
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
        transform.position += moveSpeed * Time.deltaTime * moveInput;
        //

        animator.SetFloat("Speed", moveInput.sqrMagnitude);

        //if (Input.GetKeyDown(KeyCode.Space) && dashTime <= 0)
        //{
        //    animator.SetBool("Roll", true);
        //    moveSpeed += dashBoost;
        //    dashTime = DashTime;
        //    once = true;
        //}

        //if (dashTime <= 0 && once)
        //{
        //    animator.SetBool("Roll", false);
        //    moveSpeed -= dashBoost;
        //    once = false;
        //}
        //else
        //{
        //    dashTime -= Time.deltaTime;
        //}

        // Rotate Face
        if (moveInput.x != 0)
            if (moveInput.x < 0)
                characterSR.transform.localScale = new Vector3(-1, 1, 0);
            else
                characterSR.transform.localScale = new Vector3(1, 1, 0);
    }

}