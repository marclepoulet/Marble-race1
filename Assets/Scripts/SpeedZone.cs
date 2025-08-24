using UnityEngine;

public class SpeedZone : MonoBehaviour
{
    [Header("Réglages de la zone")]
    public float boostForce = 10f;       // force appliquée
    public Vector2 boostDirection = Vector2.right; // direction du boost

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Marble")) // vérifie que c'est une bille
        {
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.AddForce(boostDirection.normalized * boostForce, ForceMode2D.Force);
            }
        }
    }
}
