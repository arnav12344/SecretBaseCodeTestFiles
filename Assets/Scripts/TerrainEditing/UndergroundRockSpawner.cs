using UnityEngine;
using System.Collections;

public class UndergroundRockDigger : MonoBehaviour
{
    [Header("Rock Spawn Settings")]
    public GameObject rockPrefab;       // The rock prefab to spawn.
    public int numberOfRocks = 10;        // Total number of rocks to spawn.
    public float spawnDelay = 0.5f;       // Delay (in seconds) between each spawn (after the initial delay).

    [Header("Spawn Area Bounds")]
    public float minX = -50f;
    public float maxX = 50f;
    public float minZ = -50f;
    public float maxZ = 50f;

    [Header("Raycast Settings")]
    public float raycastStartHeight = 100f;
    public LayerMask groundLayer;

    [Header("Underground Spawn Depth")]
    public float minDepth = 1f;
    public float maxDepth = 5f;

    void Start()
    {
        // Wait 3 seconds then begin spawning rocks.
        StartCoroutine(SpawnRocks());
    }

    IEnumerator SpawnRocks()
    {
        yield return new WaitForSeconds(3f);

        for (int i = 0; i < numberOfRocks; i++)
        {
            float randomX = transform.position.x+ Random.Range(minX, maxX);
            float randomZ = transform.position.z + Random.Range(minZ, maxZ);
            Vector3 startPosition = new Vector3( randomX, raycastStartHeight, randomZ);

            Ray ray = new Ray(startPosition, Vector3.down);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, raycastStartHeight * 3f))
            {
                float randomDepth = Random.Range(minDepth, maxDepth);
                Vector3 spawnPosition = hit.point - new Vector3(0, randomDepth, 0);
                Instantiate(rockPrefab, spawnPosition, Quaternion.identity);
            }
            else
            {
                Debug.LogWarning("No ground detected for rock spawn at: " + startPosition);
            }
        }
    }
}
