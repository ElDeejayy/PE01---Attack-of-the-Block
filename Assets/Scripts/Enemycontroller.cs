// EnemyController.cs
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed = 4f;    // velocidad del enemigo
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        rb.gravityScale = 0f;
        rb.freezeRotation = true;

        // direcci�n aleatoria inicial
        Vector2 dir = Random.insideUnitCircle.normalized;
        rb.linearVelocity = dir * speed;

        // asegurar Z=0
        Vector3 p = transform.position;
        p.z = 0f;
        transform.position = p;
    }
}
