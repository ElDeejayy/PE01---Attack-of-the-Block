// PlayerController.cs
using UnityEngine;
using UnityEngine.UI; // usamos UI Text legacy para no depender de TMP

public class PlayerController : MonoBehaviour
{
    public float speed = 50f;               // para seguir rápido al ratón
    private Rigidbody2D rb;

    // UI: asigna en Inspector (GameObject Text dentro del Canvas)
    public GameObject gameOverText;         // GameObject del texto (desactivado al inicio)
    public GameObject redScreen;            // Panel rojo (desactivado al inicio)

    private bool isDead = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        Cursor.visible = false;

        // asegurarnos en Z = 0
        Vector3 pos = transform.position;
        pos.z = 0f;
        transform.position = pos;

        // preparar UI
        if (gameOverText != null) gameOverText.SetActive(false);
        if (redScreen != null) redScreen.SetActive(false);

        // seguridad física
        rb.gravityScale = 0f;
        rb.freezeRotation = true;
    }

    void FixedUpdate()
    {
        if (isDead) return;

        // posición del ratón -> mundo
        Vector3 mouseWorld3 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mouseWorld = new Vector2(mouseWorld3.x, mouseWorld3.y);

        // limitar un poco para no intentar ir un mundo fuera (evita quedarse trabado)
        Camera cam = Camera.main;
        Vector2 bottomLeft = cam.ScreenToWorldPoint(new Vector2(0, 0));
        Vector2 topRight = cam.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));

        float halfW = GetComponent<SpriteRenderer>().bounds.extents.x;
        float halfH = GetComponent<SpriteRenderer>().bounds.extents.y;

        float clampedX = Mathf.Clamp(mouseWorld.x, bottomLeft.x - halfW * 2f, topRight.x + halfW * 2f);
        float clampedY = Mathf.Clamp(mouseWorld.y, bottomLeft.y - halfH * 2f, topRight.y + halfH * 2f);
        Vector2 target = new Vector2(clampedX, clampedY);

        // mover al jugador rápidamente (sin teletransporte brusco)
        rb.MovePosition(Vector2.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead) return;

        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("GAME OVER");
            isDead = true;

            if (gameOverText != null) gameOverText.SetActive(true);
            if (redScreen != null) redScreen.SetActive(true);

            Time.timeScale = 0f; // pausa el juego
        }
    }
}
