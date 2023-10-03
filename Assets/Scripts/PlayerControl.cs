using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Fliping X-scale is the fliping of the whole obejct (including sprite) sometimes might affect some physics
public class PlayerControl : MonoBehaviour
{
    float horizontalMove;
    public float speed = 2f;
    Rigidbody2D myBody;
    SpriteRenderer mySprite;

    bool grounded = true;

    public float castDist = 1f;
    public float jumpPower = 100f;
    public float gravityScale = 2f;
    public float gravityFall = 40f;
    bool jump = false;

    Animator myAnim;

    void Start()
    {
        myBody = GetComponent<Rigidbody2D>();
        myAnim = GetComponent<Animator>();
        mySprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        horizontalMove = Input.GetAxis("Horizontal");

        if (Input.GetButtonDown("Jump") && grounded)
        {
            jump = true;
            myAnim.SetBool("JUMP",true);
        }
      

        if (horizontalMove > 0.2f || horizontalMove < -0.2f)
        {
            myAnim.SetBool("IDLE", true);
        }

        else
        {
            myAnim.SetBool("IDLE", false);
        }

        if(horizontalMove < 0f)
        {
            mySprite.flipX = true;
        }
        else
        {
            mySprite.flipX = false;
        }

    }

    void FixedUpdate()//Runs the same frequency as the physcis engine, ususally running slower than Update
    {
        float moveSpeed = horizontalMove * speed;

        if (jump)
        {
            myBody.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            jump = false;
            myAnim.SetBool("JUMP", false);
        }

        if (myBody.velocity.y > 0)
        {
            myBody.gravityScale = gravityScale; //check if going up
        }
        else if (myBody.velocity.y < 0)
        {
            myBody.gravityScale = gravityFall;
        }

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, castDist);

        if (hit.collider != null && hit.transform.name == "branch")
        //or hit.transform.tag == "Ground", tag is under Inspector section beside layer order
        {
            grounded = true;
        }
        else
        {
            grounded = false;
        }

         myBody.velocity = new Vector3(moveSpeed, myBody.velocity.y, 0f);

    }
}
