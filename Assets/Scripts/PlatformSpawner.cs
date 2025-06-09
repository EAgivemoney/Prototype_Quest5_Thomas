using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    [Header("Platform Settings")]
    [SerializeField] GameObject[] platformPrefabs;
    [SerializeField] float spawnDistance = 20f;
    [SerializeField] float yVariation = 2f;
    [SerializeField] float minY = -5f;
    [SerializeField] float maxY = 5f;
    [SerializeField] int maxPlatforms = 10;

    [Header("References")]
    [SerializeField] Transform player;
    [SerializeField] Vector3 initialSpawnPoint = new Vector3(177.34f, -2.87f, 0f);

    private Vector3 nextSpawnPoint;
    private float lastPlatformY;
    private Queue<GameObject> spawnedPlatforms = new Queue<GameObject>();

    void Start()
    {
        nextSpawnPoint = initialSpawnPoint;
        lastPlatformY = initialSpawnPoint.y;
    }

    void Update()
    {
        if (player.position.x > nextSpawnPoint.x - spawnDistance)
        {
            SpawnPlatform();
        }
    }

    void SpawnPlatform()
    {
        GameObject prefab = platformPrefabs[Random.Range(0, platformPrefabs.Length)];

        float yOffset = Random.Range(-yVariation, yVariation);
        float newY = Mathf.Clamp(lastPlatformY + yOffset, minY, maxY);

        Vector3 spawnPos = new Vector3(nextSpawnPoint.x, newY, nextSpawnPoint.z);
        GameObject platform = Instantiate(prefab, spawnPos, Quaternion.identity);

        // Ensure the platform is in the correct layer
        platform.layer = LayerMask.NameToLayer("Ground");
        foreach (Transform child in platform.GetComponentsInChildren<Transform>())
        {
            child.gameObject.layer = LayerMask.NameToLayer("Ground");
        }

        // Move nextSpawnPoint using the EndPoint, if present
        Transform endPoint = platform.transform.Find("EndPoint");
        if (endPoint != null)
        {
            Vector3 endOffset = endPoint.position - platform.transform.position;
            nextSpawnPoint += new Vector3(endOffset.x, 0, endOffset.z);
        }
        else
        {
            nextSpawnPoint += new Vector3(10f, 0f, 0f);
        }

        lastPlatformY = newY;
        spawnedPlatforms.Enqueue(platform);

        if (spawnedPlatforms.Count > maxPlatforms)
        {
            Destroy(spawnedPlatforms.Dequeue());
        }
    }
}