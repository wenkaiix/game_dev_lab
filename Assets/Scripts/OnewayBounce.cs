using UnityEngine;
using System.Collections;

public class Bouncebox : MonoBehaviour
{
    public GameObject item;
    private Rigidbody2D rb;
    private Vector2 startPosition;
    private bool bouncing = false;
    private bool coin = true;
    public int maxHits = -1;
    public Sprite emptyBlock;


    // to store initial state
    private int initialMaxHits;
    private bool initialCoin;
    private Sprite initialSprite;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startPosition = rb.position;

        // question box stays completely fixed
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        //initialise start state
        initialMaxHits = maxHits;
        initialCoin = coin;
        spriteRenderer = GetComponent<SpriteRenderer>();
        initialSprite = spriteRenderer.sprite;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        foreach (ContactPoint2D contact in collision.contacts)
        {
            // when mario hits bottom of bbox
            if (contact.normal.y > 0.5f && !bouncing && maxHits != 0)
            {
                if (item != null && coin){
                    Instantiate(item, transform. position, Quaternion.identity);
                    coin = false;
                }
                StartCoroutine(Bounce());
                Hit();
                break;
            }
        }
    }


    private void Hit()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        maxHits--;
        if (maxHits == 0) {
            spriteRenderer.sprite = emptyBlock;
            spriteRenderer.sortingLayerName = "Obstales" ;
            spriteRenderer.sortingOrder = 1 ;
        }
    }

    public void GameRestart()
    {
        // reset state of the gameObject
        bouncing = false;
        coin = initialCoin;
        maxHits = initialMaxHits;
        spriteRenderer.sprite = initialSprite;
        rb.position = startPosition; // in case it was mid-bounce when reset happened
        StopAllCoroutines(); // important, cancels any bounce incase they restart while its bouncing
    }

    // function for bounce for the question box
    IEnumerator Bounce()
    {
        bouncing = true;

        Vector2 topPosition = startPosition + Vector2.up * 0.15f;

        // Move up
        while (Vector2.Distance(rb.position, topPosition) > 0.01f)
        {
            rb.MovePosition(
                Vector2.MoveTowards(rb.position, topPosition, 1.5f * Time.fixedDeltaTime)
            );

            yield return new WaitForFixedUpdate();
        }

        // Move back down
        while (Vector2.Distance(rb.position, startPosition) > 0.01f)
        {
            rb.MovePosition(
                Vector2.MoveTowards(rb.position, startPosition, 1.5f * Time.fixedDeltaTime)
            );

            yield return new WaitForFixedUpdate();
        }

        rb.position = startPosition;
        bouncing = false;
    }
}