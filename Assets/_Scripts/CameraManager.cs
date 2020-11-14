using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class CameraManager : MonoBehaviour
{
    public Transform pivotPoint;

    public Vector3 farRight;
    public Vector3 center;
    public Vector3 farLeft;
    public float xMaxRight = 0;
    public float xMiddle = 0;
    public float xMaxLeft = 0;

    [Range(0, 1)]
    public float cameraSpeed = 0.9f;

    private Vector3 destination;

    private Transform hero;

    // Start is called before the first frame update
    void Start()
    {
        center = pivotPoint.rotation.eulerAngles;
        hero =        GameManager.instance.main_character.transform;
    }

    // Update is called once per frame
    void Update()
    {
        float positionHero = hero.position.x;
        positionHero -= xMiddle;
        if (positionHero > 0)
            destination = Vector3.Lerp((farRight), (center), positionHero/xMaxRight);
        else if(positionHero < 0)
            destination = Vector3.Lerp((farRight), (center), positionHero / xMaxLeft);



        this.transform.rotation = Quaternion.Euler(this.transform.rotation.eulerAngles * cameraSpeed + destination * (1 - cameraSpeed));


    }
}
