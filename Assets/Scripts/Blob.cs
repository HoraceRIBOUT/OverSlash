using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blob : MonoBehaviour
{
    public bool alive = true;
    public Animator _animator;
     
    public float life = 5f;
    public float weight = 1f;

    public UtilsEnum.attribute blobAttribute = UtilsEnum.attribute.None;

    public void Start()
    {
        Spawn();
    }

    
    public void Spawn()
    {
        life = 5f;//for now 

        //Randomize 3 attribute
        blobAttribute = (UtilsEnum.attribute)Random.Range(1,4);
    }


    public void TakeDamage(int damage)
    {
        life -= damage;
        //Spawn damage output

        _animator.SetTrigger("Hit");

        if (life <= 0)
            Die();
    }

    public void Die()
    {
        alive = false;
        _animator.SetBool("Dead", true);
    }


}
