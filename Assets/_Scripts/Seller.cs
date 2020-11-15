using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seller : MonoBehaviour
{
    public Customer customerNext;
    public Customer customerNextNext;

    public void Start()
    {
        StartCoroutine(CustomerSpawner());
    }

    [Header("Spawner info")]
    public GameObject customerPrefab;
    public Transform spawnPoint; //offscreen
    public Transform parentCustomer; 
    public List<Customer> allCurrentCustomer;
    public List<Transform> waitingPoints;
    public Transform quitPoint;

    public float waitTimerMin = 6f;
    public float randomAmpl = 4f;

    public IEnumerator CustomerSpawner()
    {
        yield return new WaitForSeconds(1f);
        SpawnCustomer();
        yield return new WaitForSeconds(2f);
        while (true)
        {
            if(allCurrentCustomer.Count != waitingPoints.Count)
            {
                //add some jump to not make it every X 
                SpawnCustomer();
            }
            //else, don't spawn any more
            float randomWait = waitTimerMin + Random.Range(0, randomAmpl);
            Debug.Log("Random wait = " + randomWait + " with " + allCurrentCustomer.Count + "customer");
            yield return new WaitForSeconds(randomWait);
        }
    }

    public  void SpawnCustomer()
    {
        GameObject gO = Instantiate(customerPrefab, spawnPoint.position, Quaternion.identity, parentCustomer);

        Customer newCustomer = gO.GetComponent<Customer>();

        if (allCurrentCustomer.Count == 0)
        {
            customerNext = newCustomer;
            customerNext.radiusStart *= 2;
            customerNext.radiusOk *= 2;
        }
        if (allCurrentCustomer.Count == 1)
        {
            customerNextNext = newCustomer;
            customerNextNext.radiusStart *= 1.5f;
        }


        newCustomer.ChangeWaitingPoint(waitingPoints[allCurrentCustomer.Count].position);

        allCurrentCustomer.Add(newCustomer);
    }

    public void ReSetWaitingPoint()
    {
        for (int i = 0; i < allCurrentCustomer.Count; i++)
        {
            allCurrentCustomer[i].ChangeWaitingPoint(waitingPoints[i].position);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Blob blob = other.GetComponent<Blob>();
        if(blob != null)
        {
            //Customer :
            customerNext.GiveBlob(blob.blobAttribute);
            customerNext.ChangeWaitingPoint(quitPoint.position);
            allCurrentCustomer.Remove(customerNext);


            customerNext = customerNextNext;
            customerNext.radiusStart *= 2;
            customerNext.radiusOk *= 2;
            if (allCurrentCustomer.Count == 1)
            {
                SpawnCustomer();
            }
            customerNextNext = allCurrentCustomer[1];//Not the first one (0) but the following one
            customerNextNext.radiusStart *= 1.5f;
            ReSetWaitingPoint();

            //MainCharacter :
            GameManager.instance.main_character.DropBlob();
            //Blob : destroyyyyyy !!!
            blob.Destroy();
        }
    }


}
