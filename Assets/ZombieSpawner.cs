using System.Collections;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    public GameObject zombiePrefab;
    public float spawnRate = 10f;
    public float spawnIncreaseInterval = 30f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("SpawnZombie");
    }

    IEnumerator SpawnZombie()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnRate);
            Instantiate(zombiePrefab, transform.position, Quaternion.identity);
        }
    }
}
