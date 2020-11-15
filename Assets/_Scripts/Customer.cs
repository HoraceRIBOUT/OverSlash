using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customer : MonoBehaviour
{
    public float radiusStart = 12f;
    public float radiusOk = 5f;
    
    public Animator layerSpeechBubble;
    public GameObject attributeNeedPrefab;
    public GameObject scoreUI;

    private Transform hero;

    public UtilsEnum.attribute need = UtilsEnum.attribute.None;

    public void Start()
    {
        hero = GameManager.instance.main_character.transform;
        Randomize();
    }
    [Sirenix.OdinInspector.Button]
    public void Randomize()
    {
        //add two attribute : 
        int firstRand = Random.Range(2, (int)System.Enum.GetNames(typeof(UtilsEnum.attribute)).Length - 1);
        int secndRand = Random.Range(2, (int)System.Enum.GetNames(typeof(UtilsEnum.attribute)).Length - 1);
        //Debug.Log("(" + firstRand + ")" + " and " + "(" + secndRand + ")");

        if (secndRand == firstRand)
        {
            secndRand = (firstRand == 2) ? firstRand + 1 : firstRand - 1;
        }

        if (firstRand == 2) firstRand = Random.Range(0, 3);
        if (secndRand == 2) secndRand = Random.Range(0, 3);
        
        int decalFirstAttrib = 1 << firstRand;
        int decalSecndAttrib = 1 << secndRand;

        //Debug.Log(((UtilsEnum.attribute)decalFirstAttrib) + "("+ firstRand + ")" + " and " + (UtilsEnum.attribute)decalSecndAttrib + "(" + secndRand + ")");

        need  = (UtilsEnum.attribute)decalFirstAttrib;
        need |= (UtilsEnum.attribute)decalSecndAttrib;


        GameObject gO = GameManager.instance.ui_manager.SpawnThis(attributeNeedPrefab, this.transform.position + Vector3.up * 3);
        layerSpeechBubble = gO.GetComponentInChildren<Animator>();
    }

    public void Update()
    {


        float valueZeroOne = 1;
        Vector3 distanceVec = this.transform.position - hero.position;
        if (distanceVec.sqrMagnitude < radiusStart * radiusStart)
        {
            float distance = distanceVec.magnitude;
            
            valueZeroOne = (distance - radiusOk) / (radiusStart - radiusOk);
            valueZeroOne = Mathf.Clamp01(valueZeroOne);


//            Debug.Log("distance = " + distance + " radiusOk = " + radiusOk + "radiusStart = " + radiusStart);
        }

        layerSpeechBubble.SetLayerWeight(1, valueZeroOne);
    }

    public void GiveBlob(UtilsEnum.attribute blob)
    {
        GameManager.instance.ui_manager.SpawnThis(scoreUI, this.transform.position + Vector3.up);
        TMPro.TMP_Text text = scoreUI.GetComponentInChildren<TMPro.TMP_Text>();

        //Compare food
        blob &= need;
        int scoreCounter = UtilsEnum.howManyFlag(blob);
        //Add score :
        Debug.Log("scoreCounter = "+scoreCounter);
        text.text = "+" + scoreCounter;
        //Goes away

        //Make the next customer comes

    }

}
