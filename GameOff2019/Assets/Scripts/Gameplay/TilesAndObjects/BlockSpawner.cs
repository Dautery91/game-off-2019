using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BlockSpawner : MonoBehaviour
{
    [SerializeField] GameObject Block;
    [SerializeField] Tilemap Tilemap;
    public float SpawnTimeDuration;
    GameObject tempBlock;

    Timer spawnTimer;

    private void Awake()
    {
        spawnTimer = gameObject.AddComponent<Timer>();
        spawnTimer.Duration = SpawnTimeDuration;

        Tilemap[] tilemaps = FindObjectsOfType<Tilemap>();

        foreach (Tilemap tm in tilemaps)
        {
            if (tm.tag == "Ground")
            {
                Tilemap = tm;
            }
        }

    }

    private void Start()
    {
        spawnTimer.Run();
    }

    private void Update()
    {
        if (spawnTimer.Finished)
        {
            spawnTimer.Stop();

            tempBlock = Instantiate(Block, this.transform.position, Quaternion.identity);
        
            spawnTimer.Run();
        }
    }
}
