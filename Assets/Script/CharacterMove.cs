using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMove : MonoBehaviour
{
    private float horizontal;
    // Start is called before the first frame update
    [SerializeField] private float SpeedMove = 5f;
    [SerializeField] private float JumpPower = 15f;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private Transform groundCheck;
    //[SerializeField] private Animator animator;
    private bool isFacingRight = true;

    void Start()
    {
        //animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        if (Input.GetKey(KeyCode.UpArrow) && IsInGround())
        {
            //animator.SetBool("isJump", true);
            rb.velocity = new Vector2(rb.velocity.x, JumpPower);
        }
        //else
        //{
        //    animator.SetBool("isJump", false);
        //}
        //if (horizontal != 0f)
        //{
        //    animator.SetBool("isRunning", true);
        //}
        //else
        //{
        //    animator.SetBool("isRunning", false);
        //}
        Flip();
    }
    private void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontal * SpeedMove, rb.velocity.y);
    }
    private bool IsInGround()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.6f, layerMask);
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(groundCheck.position, 0.6f);
    //}
    private void Flip()
    {
        if(isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
}
