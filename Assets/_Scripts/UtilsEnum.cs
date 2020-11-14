using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilsEnum : MonoBehaviour
{
    [System.Flags]
    public enum attribute
    {
        None = 0,
        big       = 1 << 0,
        medium    = 1 << 1,
        small     = 1 << 2,
        spicy     = 1 << 3,
        watery    = 1 << 4,
        horn      = 1 << 5,
        noble     = 1 << 6,
    }

    public static int howManyFlag(attribute attributeToDivide)
    {
        int counter = 0;
        string[] justForLength = System.Enum.GetNames(typeof(attribute));
        for (int i = 0; i < justForLength.Length; i++)
        {
            attribute lonelyFlag = (attribute)(1 << i);
            if (attributeToDivide.HasFlag(lonelyFlag))
            {
                counter++;
            }
        }
        return counter;
    }

}
