using UnityEngine;
using System.Collections;

public class Bouncebox : MonoBehaviour
{
    public GameObject item;
    private Rigidbody2D rb;
    private Vector2 startPosition;
    private bool bouncing = false;
    private bool coin = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startPosition = rb.position;

        // question box stays completely fixed
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        foreach (ContactPoint2D contact in collision.contacts)
        {
            // when mario hits bottom of bbox
            if (contact.normal.y > 0.5f && !bouncing)
            {
                if (item != null && coin){
                    Instantiate(item, transform. position, Quaternion.identity);
                    coin = false;
                }
                StartCoroutine(Bounce());
                break;
            }
        }
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