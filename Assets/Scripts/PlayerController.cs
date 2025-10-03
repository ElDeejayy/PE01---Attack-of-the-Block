// PlayerController.cs
using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [Header("Movimiento")]
    public float speed = 50f; // cuanto mayor, más rápido sigue el ratón

    [Header("Estado")]
    [HideInInspector] public bool isInvincible = false; // accesible por GameManager

    private Rigidbody2D rb;
    private SpriteRenderer sr;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        Cursor.visible = false;

        // aseguramos Z = 0
        Vector3 p = transform.position; p.z = 0; transform.position = p;

        // seguridad física
        rb.gravityScale = 0f;
        rb.freezeRotation = true;
    }

    void FixedUpdate()
    {
        // si estamos muertos (game over) o pausado, no hacemos nada
        if (Time.timeScale == 0f) return;

        // posición del ratón en mundo
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = 0;

        // mover inmediatamente hacia el ratón (rápido)
        rb.MovePosition(Vector3.MoveTowards(rb.position, mouseWorld, speed * Time.fixedDeltaTime));
    }

    // cuando chocamos con un enemigo (collision normal, no trigger)
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Si ya somos invencibles, no pasamos daño
            if (isInvincible) return;

            // avisar al GameManager para que reste vida y gestione invencibilidad
            if (GameManager.Instance != null)
                GameManager.Instance.PlayerTookHit(this);
        }
    }

    // Método público que GameManager llama para dar invencibilidad al jugador
    public void StartInvincibility(float duration)
    {
        StartCoroutine(InvincibilityCoroutine(duration));
    }

    // parpadeo y flag de invencibilidad
    private IEnumerator InvincibilityCoroutine(float duration)
    {
        isInvincible = true;

        float elapsed = 0f;
        float flashPeriod = 0.15f;

        while (elapsed < duration)
        {
            // alternar visibilidad para efecto de parpadeo
            sr.enabled = false;
            yield return new WaitForSecondsRealtime(flashPeriod);
            sr.enabled = true;
            yield return new WaitForSecondsRealtime(flashPeriod);

            elapsed += flashPeriod * 2f;
        }

        sr.enabled = true;
        isInvincible = false;
    }
}
