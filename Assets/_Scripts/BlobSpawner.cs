using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class BlobSpawner : MonoBehaviour
{

    public void Start()
    {
        if(Application.isPlaying)
            StartCoroutine(BlobSpawning());
    }

    private void Update()
    {
        Debug.DrawLine(this.transform.position + Vector3.back    * randomYAmpl + Vector3.right * randomXAmpl,
                       this.transform.position + Vector3.back    * randomYAmpl + Vector3.left  * randomXAmpl, Color.red);
        Debug.DrawLine(this.transform.position + Vector3.back    * randomYAmpl + Vector3.right * randomXAmpl,
                       this.transform.position + Vector3.forward * randomYAmpl + Vector3.right * randomXAmpl, Color.red);
        Debug.DrawLine(this.transform.position + Vector3.forward * randomYAmpl + Vector3.left  * randomXAmpl,
                       this.transform.position + Vector3.forward * randomYAmpl + Vector3.right * randomXAmpl, Color.red);
        Debug.DrawLine(this.transform.position + Vector3.forward * randomYAmpl + Vector3.left  * randomXAmpl,
                       this.transform.position + Vector3.back    * randomYAmpl + Vector3.left  * randomXAmpl, Color.red);
    }

    [Header("Spawner info")]
    public GameObject blobPrefab;
    public float randomRangeAround = 6f;
    public Transform blobParent;

    public float waitTimerMin = 6f;
    public float randomTimer = 4f;
    public float randomXAmpl = 4f;
    public float randomYAmpl = 4f;

    public int blobCounter = 0;

    public int blobLimit = 8;

    public IEnumerator BlobSpawning()
    {
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
            float randomWait = waitTimerMin + Random.Range(0, randomTimer);
            yield return new WaitForSeconds(randomWait);
        }
    }

    public void SpawnBlob()
    {
        Vector3 randomPosition = this.transform.position;
        randomPosition.x += Random.Range(-randomXAmpl, randomXAmpl);
        randomPosition.z += Random.Range(-randomYAmpl, randomYAmpl);

        Blob newBlob = Instantiate(blobPrefab, randomPosition, Quaternion.identity, blobParent).GetComponent<Blob>();

        blobCounter++;

        newBlob.mySpawner = this;
    }


    public List<Transform> idlingPoint;
    public List<int> indexTaken;

    public void RemoveThisPoint(Transform pointToRemove)
    {
        indexTaken.Remove(idlingPoint.IndexOf(pointToRemove));
    }
    public Transform GetNewIdlingPosition()
    {
        int randomIndex = Random.Range(0, idlingPoint.Count - indexTaken.Count);

        while (indexTaken.Contains(randomIndex))
        {
            randomIndex++;
            if(randomIndex == idlingPoint.Count - 1)
            {
                randomIndex = 0;
            }
        }
        indexTaken.Add(randomIndex);

        return idlingPoint[randomIndex];
    }


}
