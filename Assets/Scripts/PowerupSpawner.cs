// PowerupSpawner.cs
using UnityEngine;
using System.Collections;

public class PowerupSpawner : MonoBehaviour
{
    public GameObject[] powerupPrefabs; // arrastra prefabs al Inspector
    public float spawnInterval = 15f;
    public float margin = 0.5f; // margen para no spawnear fuera de cámara

    void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    private IEnumerator SpawnLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            if (powerupPrefabs.Length == 0) continue;

            // posición aleatoria dentro de la cámara
            Vector2 bottomLeft = Camera.main.ScreenToWorldPoint(Vector2.zero);
            Vector2 topRight = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));

            float x = Random.Range(bottomLeft.x + margin, topRight.x - margin);
            float y = Random.Range(bottomLeft.y + margin, topRight.y - margin);

            GameObject pref = powerupPrefabs[Random.Range(0, powerupPrefabs.Length)];
            Instantiate(pref, new Vector3(x, y, 0f), Quaternion.identity);
        }
    }
}
