using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttributeUI : MonoBehaviour
{
    public Transform targetTrans;
    [Range(0,1)]
    public float delay = 0.9f;

    private int indexIcon = 0;
    public List<UnityEngine.UI.Image> iconUI = new List<UnityEngine.UI.Image>();

    //Auto sorting attribute to list icons image 
    //Also following his "master" 
    //
    [System.Serializable]
    public struct Pair
    {
        public UtilsEnum.attribute attrib;
        public Sprite icon;
    }
    public List<Pair> attribIcon = new List<Pair>();

    public void Update()
    {

        Vector3 positionRectified = targetTrans.transform.position;
        positionRectified.y = this.transform.position.y;
        this.transform.position = transform.position * delay + positionRectified * (1 - delay);
    }

    public void Initialization(Transform target, UtilsEnum.attribute targetAttribute)
    {
        targetTrans = target;
        IconSet(targetAttribute);
    }

    public void IconSet(UtilsEnum.attribute iconToDisplay)
    {
        indexIcon = 0;
        foreach (Pair iconPair in attribIcon)
        {
            if (iconToDisplay.HasFlag(iconPair.attrib))
            {
//                Debug.Log("Hello");
                iconUI[indexIcon].sprite = null;
//                Debug.Log("Still there");
                iconUI[indexIcon].sprite = iconPair.icon;
                iconUI[indexIcon].color = Color.blue;
                indexIcon++;
                if (indexIcon == 3)
                    return;
            }
        }
    }

    [Sirenix.OdinInspector.Button]
    public void CreatePairForEachAttribute()
    {
        attribIcon.Clear();
        for (int i = 0; i < System.Enum.GetNames(typeof(UtilsEnum.attribute)).Length; i++)
        {
            int powerI = 1 << i;
            UtilsEnum.attribute newAttrib = (UtilsEnum.attribute)powerI;
            Pair pair;
            pair.attrib = newAttrib;
            pair.icon = null;
            attribIcon.Add(pair);
        }
    }

}
