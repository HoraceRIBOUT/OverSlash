using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilsEnum : MonoBehaviour
{
    [System.Flags]
    public enum attribute
    {
        None = 0,
        big,
        medium,
        small,
        spicy,
        watery,
        horn,
        leafy,
    }



}
