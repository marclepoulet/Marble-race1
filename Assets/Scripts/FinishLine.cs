using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering;

public class FinishLine : MonoBehaviour
{

    public Transform finishpos;

    private GameManager gameManager;

    private void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.CompareTag("Marble"))
        {
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();

            float randomRangeX = Random.Range(-5f, 5f);
            float randomRangeY = Random.Range(0f, 2f);
            Vector3 randomOffset = new Vector3(randomRangeX, randomRangeY, 0);

            other.transform.position = finishpos.position + randomOffset;
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
            gameManager.BilleArrivee(other.gameObject);

        }
    }


}
