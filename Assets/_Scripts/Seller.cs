using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seller : MonoBehaviour
{
    public Customer customer;

    public void Start()
    {
        customer = FindObjectOfType<Customer>();
    }

    //Maybe more a spawn of customer

    private void OnTriggerEnter(Collider other)
    {
        Blob blob = other.GetComponent<Blob>();
        if(blob != null)
        {
            //Customer :
            customer.GiveBlob(blob.blobAttribute);

            //MainCharacter :
            GameManager.instance.main_character.DropBlob();
            //Blob : destroyyyyyy !!!
            Destroy(blob.gameObject);
        }
    }
}
