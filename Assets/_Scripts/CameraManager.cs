using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class CameraManager : MonoBehaviour
{
    public Transform pivotPoint;

    [Range(-1, 1)]
    public float xPersoPosition = 0;

    public float xRotRight = 0;
    public float zRotRight = 0;
    public float xMaxRight = 0;
    public float xMiddle = 0;
    public float xMaxLeft = 0;
    public float xRotLeft = 0;
    public float zRotLeft = 0;
    private float previousRotationValueX = 0;
    private float previousRotationValueY = 65;
    private float previousRotationValueZ = 0;
    public Vector3 yRotation = new Vector3(64, 65, 67);
    public Vector2 yPosMinMax = new Vector2(-6,6);

    public AnimationCurve yMiddleCurve = new AnimationCurve(new Keyframe(-1,0), new Keyframe(0, 0), new Keyframe(1, 0));

    [Range(0, 1)]
    public float cameraSpeed = 0.9f;

    [Range(0, 1)]
    public float rotationSpeed = 0.9f;

    private Vector3 destination;

    private Transform hero;

    // Start is called before the first frame update
    void Start()
    {
        hero = GameManager.instance.main_character.transform;
    }

    // Update is called once per frame
    void Update()
    {
        CameraRotationManagement();




    }

    public void CameraRotationManagement()
    {
        float positionHero = hero.position.x;
        positionHero -= xMiddle;
        float rotationValueX = 0;
        float rotationValueZ = 0;

        if (positionHero > 0)
        {
            //xPersoPosition = -positionHero / xMaxRight;
            rotationValueX = xRotRight * (-positionHero / xMaxRight);
            rotationValueZ = zRotRight * (-positionHero / xMaxRight);
        }
        else if (positionHero < 0)
        {
            //xPersoPosition = positionHero / xMaxLeft;
            rotationValueX = xRotLeft * (-positionHero / xMaxLeft);
            rotationValueZ = zRotLeft * (-positionHero / xMaxLeft);
        }
        //else
        //    xPersoPosition = positionHero;

        rotationValueX = rotationValueX * rotationSpeed + previousRotationValueX * (1 - rotationSpeed);
        rotationValueZ = rotationValueZ * rotationSpeed + previousRotationValueZ * (1 - rotationSpeed);
        pivotPoint.rotation = Quaternion.Euler(Vector3.up * rotationValueX + Vector3.forward * rotationValueZ);

        previousRotationValueX = rotationValueX;
        previousRotationValueZ = rotationValueZ;



        float positionHeroY = hero.position.z;
        positionHeroY -= yMiddleCurve.Evaluate(positionHero);
        float rotationValueY = 0;
        if (positionHeroY > 0)
        {
            rotationValueY = Mathf.Lerp(yRotation.y, yRotation.x, positionHeroY / yPosMinMax.y);
            xPersoPosition = positionHeroY / yPosMinMax.y;
    }
        else if (positionHeroY < 0)
        {
            rotationValueY = Mathf.Lerp(yRotation.y, yRotation.z, positionHeroY / yPosMinMax.x);
            xPersoPosition = positionHeroY / yPosMinMax.x;
        }

//        Debug.Log(" hero.position.z = " + hero.position.z + " Position hero = " + positionHeroY + " rotationValueY = " + rotationValueY + " yMiddleCurve.Evaluate(positionHeroY) =" + yMiddleCurve.Evaluate(positionHero));

        rotationValueY = rotationValueY * rotationSpeed + previousRotationValueY * (1 - rotationSpeed);
        this.transform.localRotation = Quaternion.Euler(Vector3.right * rotationValueY);


        previousRotationValueY = rotationValueY;
    }

}
