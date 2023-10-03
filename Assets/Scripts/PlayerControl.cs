using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Fliping X-scale is the fliping of the whole obejct (including sprite) sometimes might affect some physics
public class PlayerControl : MonoBehaviour
{
    float horizontalMove;
    public float speed = 2f;
    Rigidbody2D myBody;
    SpriteRenderer mySprite;

    bool grounded = true;
    public Sprite spr_EndFall;
    public Sprite spr_EndJump;

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
            myAnim.SetBool("JUMP", true); //jumping animation starts
            myAnim.SetBool("IDLE", false);
            myAnim.SetBool("FALL", false);

        }

        if ((horizontalMove > 0.1f || horizontalMove < -0.1f) && grounded) //check if grounded and moving
        {
             myAnim.SetBool("IDLE", true);
             myAnim.SetBool("JUMP", false);
             myAnim.SetBool("FALL", false);
        }
        else
        {
            myAnim.SetBool("IDLE", false); //IDLE animation ends (stationary)
            myAnim.SetBool("JUMP", false);
            myAnim.SetBool("FALL", false);
        }

        if (horizontalMove < 0f)
        {
            mySprite.flipX = true;
        }
        else
        {
            mySprite.flipX = false;
        }

        
    }

     void FixedUpdate() //Runs the same frequency as the physcis engine, ususally running slower than Update
     {
        float moveSpeed = horizontalMove * speed;

        if (jump)
        {
            myBody.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            jump = false;
        }

        if (myBody.velocity.y > 0)
        {
            myBody.gravityScale = gravityScale; //check if going up
        }

        else if (myBody.velocity.y < 0)
        {
            myBody.gravityScale = gravityFall;
        }

        else //reaching the highest point
        {
            myAnim.SetBool("FALL", true); //falling animation starts
            myAnim.SetBool("JUMP", false);
            myAnim.SetBool("IDLE", false);
        }

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, castDist);

        if (hit.collider != null && hit.transform.name == "branch")
        {
            grounded = true;
            myAnim.SetBool("IDLE", false); //falling animation ends (stationary)
            myAnim.SetBool("FALL", false);
            myAnim.SetBool("JUMP", false);

        }

        else
        {
            grounded = false;
        }

        myBody.velocity = new Vector3(moveSpeed, myBody.velocity.y, 0f);

        }

    void AnimationDetection(string message)
    {
        if (message.Equals("End of JUMP"))
        {
            mySprite.sprite = spr_EndJump;
        }

        else if (message.Equals("End of FALL"))
        {
            mySprite.sprite = spr_EndFall;
        }
    }

    //Death collision trigger
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.name == "DeathCollisionBox")
        {
            SceneManager.LoadScene("Level1");
        }
    }

}