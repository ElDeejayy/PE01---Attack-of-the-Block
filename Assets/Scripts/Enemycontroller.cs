// EnemyController.cs
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed = 4f;          // velocidad base
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.freezeRotation = true;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

        // dirección aleatoria inicial
        Vector2 dir = Random.insideUnitCircle.normalized;
        rb.linearVelocity = dir * speed;

        // asegurar Z = 0
        Vector3 p = transform.position; p.z = 0; transform.position = p;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // si choca con muro (tag "Wall") -> reflejar la velocidad (rebote manual)
        if (collision.gameObject.CompareTag("Wall"))
        {
            // usamos la normal del primer contacto
            if (collision.contacts.Length > 0)
            {
                Vector2 normal = collision.contacts[0].normal;
                Vector2 newDir = Vector2.Reflect(rb.linearVelocity.normalized, normal);
                rb.linearVelocity = newDir * speed;

                // sonido de rebote
                if (SoundManager.Instance != null) SoundManager.Instance.PlayBounce();
            }
        }

        // si choca con el player, reproducimos sonido de golpe (GameManager se encarga de restar vida)
        if (collision.gameObject.CompareTag("Player"))
        {
            if (SoundManager.Instance != null) SoundManager.Instance.PlayHitPlayer();
        }
    }

    // método público para cambiar la velocidad temporalmente (usado por powerups)
    public void ApplySpeedMultiplier(float multiplier, float duration)
    {
        StartCoroutine(ApplySpeedCoroutine(multiplier, duration));
    }

    private System.Collections.IEnumerator ApplySpeedCoroutine(float multiplier, float duration)
    {
        float oldSpeed = speed;
        speed *= multiplier;
        rb.linearVelocity = rb.linearVelocity.normalized * speed; // ajustar la velocidad actual

        yield return new WaitForSecondsRealtime(duration);

        speed = oldSpeed;
        rb.linearVelocity = rb.linearVelocity.normalized * speed;
    }
}
