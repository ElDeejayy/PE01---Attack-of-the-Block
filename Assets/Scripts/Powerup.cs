// Powerup.cs
using UnityEngine;

public enum PowerupType { SpeedUpPlayer, Invincibility, SlowEnemies }

public class Powerup : MonoBehaviour
{
    public PowerupType type = PowerupType.SpeedUpPlayer;
    public float value = 1.5f;   // multiplicador (por ejemplo 1.5x speed)
    public float duration = 5f;  // duración del efecto en segundos

    void Start()
    {
        // asegurarnos que el collider está como Trigger
        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        // sonido powerup
        if (SoundManager.Instance != null) SoundManager.Instance.PlayPowerup();

        // aplicar efecto según tipo
        PlayerController player = other.GetComponent<PlayerController>();

        if (type == PowerupType.SpeedUpPlayer)
        {
            if (player != null)
                StartCoroutine(ApplyPlayerSpeed(player, value, duration));
        }
        else if (type == PowerupType.Invincibility)
        {
            if (player != null)
                player.StartInvincibility(duration);
        }
        else if (type == PowerupType.SlowEnemies)
        {
            // buscar todos los enemigos y aplicar multiplicador (ej: value = 0.5 para ralentizar)
            EnemyController[] enemies = FindObjectsOfType<EnemyController>();
            foreach (EnemyController e in enemies)
                e.ApplySpeedMultiplier(value, duration);
        }

        // destruir powerup recogido
        Destroy(gameObject);
    }

    private System.Collections.IEnumerator ApplyPlayerSpeed(PlayerController player, float multiplier, float duration)
    {
        float oldSpeed = player.speed;
        player.speed *= multiplier;
        yield return new WaitForSecondsRealtime(duration);
        player.speed = oldSpeed;
    }
}
