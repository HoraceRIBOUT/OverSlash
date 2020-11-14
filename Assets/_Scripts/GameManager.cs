using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public void Awake()
    {
        if (instance == null)
            instance = this;
    }


    public UIManager ui_manager;
    public MainCharacter main_character;
    public CameraManager camera_Manager;
}
