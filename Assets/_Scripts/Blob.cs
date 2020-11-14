using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blob : MonoBehaviour
{
    public bool alive = true;
    public Animator _animator;
    public SphereCollider _collider;
    public GameObject damageUI;
    public GameObject deadUI;

    [Header("GD data")]

    public float life = 5f;
    public float weight = 1f;

    public UtilsEnum.attribute blobAttribute = UtilsEnum.attribute.None;
    public Vector3 smallSize = new Vector3(1,1,1);
    public Vector3 mediumSize = new Vector3(1,1,1);
    public Vector3 largeSize = new Vector3(1,1,1);

    public void Start()
    {
        Spawn();
    }

    
    public void Spawn()
    {
        life = 5f;//for now 

        //Randomize 3 attribute
        UtilsEnum.attribute size = (UtilsEnum.attribute)Random.Range(1, 4);
        ChangeSize(size);
        blobAttribute = size;

        //add two attribute : 
        int firstRand = Random.Range(4, System.Enum.GetNames(typeof(UtilsEnum.attribute)).Length);
        int secondRand = Random.Range(4, System.Enum.GetNames(typeof(UtilsEnum.attribute)).Length);
        if(secondRand == firstRand)
        {
            secondRand = firstRand == 0 ? firstRand + 1 : firstRand - 1;
        }

        Debug.Log(((UtilsEnum.attribute)firstRand) + " and " + (UtilsEnum.attribute)secondRand);

        blobAttribute |= (UtilsEnum.attribute)firstRand;
        blobAttribute |= (UtilsEnum.attribute)secondRand;
    }

    public void ChangeSize(UtilsEnum.attribute size)
    {
        switch (size)
        {
            case UtilsEnum.attribute.big:
                _animator.transform.localScale = smallSize;
                _collider.radius = smallSize.x/2f;
                break;
            case UtilsEnum.attribute.medium:
                _animator.transform.localScale = mediumSize;
                _collider.radius = mediumSize.x / 2f;
                break;
            case UtilsEnum.attribute.small:
                _animator.transform.localScale = largeSize;
                _collider.radius = largeSize.x / 2f;
                break;
            default:
                break;
        }
    }


    public void TakeDamage(int damage)
    {
        life -= damage;
        //Spawn damage output
        GameManager.instance.ui_manager.SpawnThis(damageUI, this.transform.position + Vector3.up * 2f);

        _animator.SetTrigger("Hit");

        if (life <= 0)
            Die();
    }

    public void Die()
    {
        alive = false;
        _animator.SetBool("Dead", true);
        GameManager.instance.ui_manager.SpawnThis(deadUI, this.transform.position + Vector3.up * 2f);
    }


}
