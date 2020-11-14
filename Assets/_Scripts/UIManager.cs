using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public Transform worldInCanvas;



    public GameObject SpawnThis(GameObject uiGameObject, Vector3 positionInWorld)
    {
        GameObject gO = Instantiate(uiGameObject, positionInWorld, Quaternion.identity, this.transform);

        return gO;
    }
}
