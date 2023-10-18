using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Fliping X-scale is the fliping of the whole obejct (including sprite) sometimes might affect some physics
public class PlayerControl : MonoBehaviour
{
    float horizontalMove;
    public float speed = 20f;
    public AudioSource audioSource;
    public AudioClip thornSnd;
    public AudioClip bgm;
    //public AudioClip moveSnd;
    public AudioClip ladybirdSnd;
    Rigidbody2D myBody;
    SpriteRenderer mySprite;

    bool grounded = true;

    public float castDist = 1f;
    public float jumpPower = 100f;
    public float gravityScale = 2f;
    public float gravityFall = 40f;
    bool jump = false;
    bool roll = false;

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

        if (grounded)
        {

            if (Input.GetButtonDown("Jump"))
            {
                jump = true;
            }

            else if ((horizontalMove > 0.1f || horizontalMove < -0.1f) && !jump)//check if grounded and moving
            {
                myAnim.SetBool("IDLE", true);//Play IDLE animation

            }

            if (Input.GetKey(KeyCode.R))//Enter rolling state
            {
                roll = true;
                myAnim.SetBool("ROLL", true);
                speed = 50f;
            }
            else
            {
                roll = false;
                myAnim.SetBool("ROLL", false);
                speed = 20f;
            }


            if (horizontalMove < 0f)
            {
                mySprite.flipX = true;
            }
            else if (horizontalMove == 0)//Stationary
            {
                myAnim.SetBool("IDLE", false);//No animation is played
            }
            else
            {
                mySprite.flipX = false;
            }
        }

    }

     void FixedUpdate() //Runs the same frequency as the physcis engine, ususally running slower than Update
     {
        float moveSpeed = horizontalMove * speed;

        if (jump)
        {
            myBody.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            jump = false;

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
            }
        }

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, castDist);

        if (hit.collider != null && (hit.transform.name == "branch" || hit.transform.name == "dragonfly"))
        {
            grounded = true;

        }
        else
        {
            grounded = false;
        }

        myBody.velocity = new Vector3(moveSpeed, myBody.velocity.y, 0f);

     }
    


    //Death collision trigger
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.name == "DeathCollisionBox")
        {
            string sceneName = SceneManager.GetActiveScene().name;

            // load the same scene
            SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        }

        else if(collider.gameObject.name == "exit")
        {
            SceneManager.LoadScene("Level2");
        }

        else if(collider.gameObject.name == "exit2")
        {
            SceneManager.LoadScene("End");
        }

        else if(collider.gameObject.name == "ladybird")
        {
            if (roll)//Rolling to destroy ladybird
            {
                audioSource.PlayOneShot(ladybirdSnd);
                Destroy(collider.gameObject);
            }
            else
            {
                string sceneName = SceneManager.GetActiveScene().name;

                // load the same scene
                SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
            }
        }

        else if(collider.gameObject.name == "thorn")
        {
            audioSource.PlayOneShot(thornSnd);
            string sceneName = SceneManager.GetActiveScene().name;

            // load the same scene
            SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        }
        else
        {

        }

    }

}