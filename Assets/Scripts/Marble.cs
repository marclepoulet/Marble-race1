using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Marble : MonoBehaviour
{
    private Rigidbody2D rb;

    //public Image image;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Freeze()
    {
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    public void Unfreeze()
    {
        rb.constraints = RigidbodyConstraints2D.None;
    }

    public void FreezeThenUnfreeze(float delay)
    {
        StartCoroutine(FreezeUnfreezeCoroutine(delay));
    }

    private IEnumerator FreezeUnfreezeCoroutine(float delay)
    {
        Freeze();
        yield return new WaitForSeconds(delay);
        Unfreeze();
    }
}
