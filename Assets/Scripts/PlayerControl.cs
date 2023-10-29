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
    public AudioClip ladybirdSnd;
    Rigidbody2D myBody;
    SpriteRenderer mySprite;

    public TrailRenderer trail;

    bool grounded = true;

    public float castDist = 1f;
    public float jumpPower = 100f;
    public float gravityScale = 2f;
    public float gravityFall = 40f;
    bool jump = false;
    bool roll = false;
    public bool slashedThorn = false;

    Animator myAnim;

    public GameObject rp;
    private Transform respawnPoint;

    public GameObject filter;

    public Vector3 velocity;
    float angle; //store original angle

    public float min_angle = -45f;
    public float max_angle = 30f;

    void Start()
    {
        myBody = GetComponent<Rigidbody2D>();
        myAnim = GetComponent<Animator>();
        mySprite = GetComponent<SpriteRenderer>();
        respawnPoint = GameObject.FindGameObjectWithTag("Respawn").transform;
        filter.SetActive(false);
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
    /*
    void LateUpdate()
    {
        float rotationX = transform.rotation.eulerAngles.x;
        float rotationY = transform.rotation.eulerAngles.y;

        Debug.Log("RotationX"+rotationX);
        Debug.Log("RotationY" + rotationY);

        if (rotationX > 45)
        {
            transform.localEulerAngles = new Vector3(45, transform.rotation.eulerAngles.y, 0);
        }
        else if (rotationX < -45)
        {
            transform.localEulerAngles = new Vector3(-45, transform.rotation.eulerAngles.y, 0);
        }
        else if(rotationY > 45)
        {
            transform.localEulerAngles = new Vector3(transform.rotation.eulerAngles.x, 45, 0);
        }
        else if(rotationY < -45)
        {
            transform.localEulerAngles = new Vector3(transform.rotation.eulerAngles.x, -45, 0);
        }
        else
        {

        }
    }

    */
    private void Rotation()
    {
        //conevrt velocity to angle
        float newAngle = Mathf.Atan2(velocity.y, velocity.x);

        //convert to degree and clamp value between ranges
        newAngle = Mathf.Clamp(newAngle * Mathf.Rad2Deg, min_angle, max_angle);

        angle = Mathf.Lerp(angle, newAngle, Time.deltaTime);

        transform.localEulerAngles = new Vector3(0, 0, angle);
    }


    //Death collision trigger
    void OnTriggerEnter2D(Collider2D collider)
    {
        switch (collider.gameObject.name)
        {
            case "DeathCollisionBox":

                filter.SetActive(true);
                Invoke("Respawn",0.1f);

                break;

            case "exit":

                SceneManager.LoadScene("Level2");

                break;

            case "exit2":

                SceneManager.LoadScene("End");

                break;

            case "ladybird":

                if (roll) //Rolling to destroy ladybird
                {
                    audioSource.PlayOneShot(ladybirdSnd);
                    Destroy(collider.gameObject);
                }
                else
                {
                    filter.SetActive(true);
                    // load the same scene
                    Invoke("Respawn", 0.1f);
                }
                break;

            case "thorn":

                audioSource.PlayOneShot(thornSnd);
                slashedThorn = true;

                if (audioSource.isPlaying)
                {
                    filter.SetActive(true);
                    Invoke("Respawn", thornSnd.length);
                }

                break;

            case "respawnPoint1":

                transform.position = rp.transform.position;

                break;

            case "respawnPoint2":

                transform.position = rp.transform.position;

                break;
        }

    }

    void Respawn()
    {

        slashedThorn = false;

        if (transform.position.x >= respawnPoint.position.x)
        {
            transform.position = respawnPoint.transform.position;
            filter.SetActive(false);
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
        }
    }

}