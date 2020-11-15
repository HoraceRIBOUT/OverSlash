using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlobSpawner : MonoBehaviour
{

    public void Start()
    {
        StartCoroutine(BlobSpawning());
    }

    [Header("Spawner info")]
    public GameObject blobPrefab;
    public float randomRangeAround = 6f;
    public Transform blobParent;

    public float waitTimerMin = 6f;
    public float randomAmpl = 4f;

    public int blobCounter = 0;

    public int blobLimit = 8;

    public IEnumerator BlobSpawning()
    {
        SpawnBlob();
        SpawnBlob();
        SpawnBlob();
        SpawnBlob();
        yield return new WaitForSeconds(2f);
        while (true)
        {
            if (blobCounter < blobLimit)
            {
                //add some jump to not make it every X 
                SpawnBlob();
            }
            //else, don't spawn any more
            float randomWait = waitTimerMin + Random.Range(0, randomAmpl);
            yield return new WaitForSeconds(randomWait);
        }
    }

    public void SpawnBlob()
    {
        Vector3 randomPosition = this.transform.position;
        randomPosition.x += Random.Range(-randomAmpl, randomAmpl);
        randomPosition.z += Random.Range(-randomAmpl, randomAmpl);

        Instantiate(blobPrefab, randomPosition, Quaternion.identity, blobParent);

        blobCounter++;
    }

}
