// SoundManager.cs
using UnityEngine;

// Singleton simple para reproducir sonidos desde cualquier script
public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Audio Sources")]
    public AudioSource sfxSource;     // para efectos cortos (PlayOneShot)
    public AudioSource musicSource;   // si quieres música de fondo

    [Header("Audio Clips")]
    public AudioClip bounceClip;
    public AudioClip hitPlayerClip;
    public AudioClip gameOverClip;
    public AudioClip startClip;
    public AudioClip powerupClip;

    void Awake()
    {
        // Singleton básico
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }

        DontDestroyOnLoad(gameObject);
    }

    // reproducir cualquier clip rápido
    public void PlaySFX(AudioClip clip)
    {
        if (clip == null || sfxSource == null) return;
        sfxSource.PlayOneShot(clip);
    }

    public void PlayBounce() => PlaySFX(bounceClip);
    public void PlayHitPlayer() => PlaySFX(hitPlayerClip);
    public void PlayGameOver() => PlaySFX(gameOverClip);
    public void PlayStart() => PlaySFX(startClip);
    public void PlayPowerup() => PlaySFX(powerupClip);
}
