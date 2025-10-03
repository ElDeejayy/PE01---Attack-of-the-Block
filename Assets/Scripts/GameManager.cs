// GameManager.cs
using UnityEngine;
using TMPro; // si usas TextMeshPro, si no usa UnityEngine.UI.Text
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Vidas")]
    public int maxLives = 3;
    private int currentLives;

    [Header("UI")]
    public TextMeshProUGUI livesText;    // muestra "Vidas: X"
    public GameObject gameOverPanel;     // panel rojo + texto "GAME OVER"

    [Header("Invencibilidad")]
    public float invincibilityTime = 2f; // tiempo entre golpes para no perder varias vidas seguidas

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        currentLives = maxLives;
        UpdateLivesUI();

        if (gameOverPanel != null) gameOverPanel.SetActive(false);

        // reproducir sonido de inicio si hay SoundManager
        if (SoundManager.Instance != null) SoundManager.Instance.PlayStart();
    }

    void UpdateLivesUI()
    {
        if (livesText != null) livesText.text = "Vidas: " + currentLives;
    }

    // Llamado por el PlayerController cuando recibe un golpe
    public void PlayerTookHit(PlayerController player)
    {
        // si el jugador está invencible, ignoramos
        if (player.isInvincible) return;

        // restar vida
        currentLives--;
        UpdateLivesUI();

        // sonido de golpe
        if (SoundManager.Instance != null) SoundManager.Instance.PlayHitPlayer();

        // si quedan vidas, damos invencibilidad temporal
        if (currentLives > 0)
        {
            // pedimos al player que empiece invencibilidad (parpadeo interno)
            player.StartInvincibility(invincibilityTime);
        }
        else
        {
            // Game Over total
            StartCoroutine(HandleGameOver());
        }
    }

    // Game over final
    private IEnumerator HandleGameOver()
    {
        // sonido game over
        if (SoundManager.Instance != null) SoundManager.Instance.PlayGameOver();

        // mostrar panel
        if (gameOverPanel != null) gameOverPanel.SetActive(true);

        // pausar el juego
        Time.timeScale = 0f;

        yield return null;
    }

    // reiniciar (si implementas un botón)
    public void Restart()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    // salir (si implementas un botón)
    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
