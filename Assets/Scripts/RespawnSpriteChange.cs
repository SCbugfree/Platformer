using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RespawnSpriteChange : MonoBehaviour
{

    SpriteRenderer budRenderer;
    Rigidbody2D myBody;
    public Sprite newSpr;

    void Start()
    {
        myBody = GetComponent<Rigidbody2D>();
        budRenderer = GetComponent<SpriteRenderer>();
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.name == "Player") //if player reaches respawn point
        {
            budRenderer.sprite = newSpr;
        }
    }
}
