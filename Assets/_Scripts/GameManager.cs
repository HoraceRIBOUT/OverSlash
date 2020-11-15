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
    public TMPro.TMP_Text scoreText;
    public UnityEngine.UI.Slider scoreSlider;
    public float maxScore = 50;
    public float score;



    //[Sirenix.OdinInspector.Button]
    //public void SpawnBlob()
    //{
    //    foreach(Blob bl in FindObjectsOfType<Blob>())
    //    {
    //        bl.Spawn();
    //    }
    //}

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }

    public void AddScore(float add)
    {
        score += add;
        scoreText.text = "Satisfaction =\n" + score;
        scoreSlider.value = score / maxScore;
    }


    [Header("Sound Management")]
    public List<AudioSource> hitSound;
    public int indexHitSound = 0;

    public void PlayHit()
    {
        hitSound[indexHitSound].Play();

        indexHitSound++;
        if(indexHitSound == hitSound.Count)
            indexHitSound = 0;
    }
}
