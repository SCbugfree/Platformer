using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingUpDown : MonoBehaviour
{
    public Rigidbody2D rb;
    private Vector2 initialPos;
    private float speed = 3f;
    public AudioSource dragonflyAudio;
    public AudioClip flyingSnd;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        initialPos = rb.position;
    }

    void FixedUpdate()
    {
        //transform.position = new Vector3(0,Mathf.Sin(Time.time),0);
        float newPosY = Mathf.Sin(Time.time * speed);
        Vector2 newPos = new Vector2(0, newPosY) + initialPos;
        rb.MovePosition(newPos);
    }
}
