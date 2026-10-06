using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class JumpOverGoomba : MonoBehaviour
{
    public Transform enemyLocation;
    // private bool onGroundState;

    // [System.NonSerialized]
    // public int score = 0; // we don't want this to show up in the inspector

    // public Vector3 boxSize;
    // public float maxDistance;
    // public LayerMask layerMask;
    GameManager gameManager;
    private bool alive = true;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        // mario jumps
        // if (Input.GetKeyDown("space") && onGroundCheck())
        // {
        //     onGroundState = false;
        //     countScoreState = true;
        // }
        // when jumping, and Goomba is near Mario and we haven't registered our score
        if (Mathf.Abs(transform.position.x - enemyLocation.position.x) < 0.5f && alive)
            {
                // countScoreState = false;
                // score++;
                // scoreText.text = "Score: " + score.ToString();
                // Debug.Log(score);
                gameManager.IncreaseScore(1); // uses the game manager gameobject to edit score so each portion stays in their poriton
            }
    }


}
