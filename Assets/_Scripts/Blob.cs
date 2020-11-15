using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blob : MonoBehaviour
{
    public bool alive = true;
    public Transform draggedBy = null;
    public Animator _animator;
    public MeshRenderer _meshRenderer;
    private Material defaultMat;
    public CapsuleCollider _collider;
    public GameObject damageUI;
    public GameObject deadUI;

    public Vector3 nextTargetPosition;
    public Transform nextTarget;

    public Animator layerSpeechBubble;
    public GameObject attributeNeedPrefab;
    public BlobSpawner mySpawner;

    [Header("GD data")]

    public float life = 5f;
    public float weight = 1f;
    public float rangeAttack = 3f;
    public float speed = 1f;

    [Range(0, 1.99f)]
    public float dragDistance = 2f;
    [Range(0, 1)]
    public float dragEffect = 0.7f;

    public UtilsEnum.attribute blobAttribute = UtilsEnum.attribute.None;
    public Vector3 smallSize = new Vector3(1, 1, 1);
    public float smallWeight = 1.4f;
    public float smallrangeAttack = 2f;
    public Vector3 mediumSize = new Vector3(1, 1, 1);
    public float mediumWeight = 1.9f;
    public float mediumrangeAttack = 3f;
    public Vector3 largeSize = new Vector3(1, 1, 1);
    public float largeWeight = 2.5f;
    public float largerangeAttack = 6f;

    [Header("For atribute")]
    public Material matWater;
    public GameObject nobleHat;
    public GameObject horn;
    public ParticleSystem spicy;

    public void Start()
    {
        nextTargetPosition = this.transform.position;
        defaultMat = _meshRenderer.material;
        Spawn();
    }

    #region Cosmethic

    [Sirenix.OdinInspector.Button]
    public void Spawn()
    {
        ClearAttribute();

        life = 5f;//for now 

        //Randomize 3 attribute
        int randomSize = Random.Range(0, 3);
        int decalSize = 1 << randomSize;
        UtilsEnum.attribute size = (UtilsEnum.attribute)decalSize;
        ChangeSize(size);
        blobAttribute = size;

        //add two attribute : 
        int firstRand = Random.Range(3, (int)System.Enum.GetNames(typeof(UtilsEnum.attribute)).Length - 1);
        int secondRand = Random.Range(3, (int)System.Enum.GetNames(typeof(UtilsEnum.attribute)).Length - 1);
        if (secondRand == firstRand)
        {
            secondRand = firstRand == 3 ? firstRand + 1 : firstRand - 1;
        }


        int decalFirstAttrib = 1 << firstRand;
        int decalSecondAttrib = 1 << secondRand;

        //        Debug.Log(size + "(" + randomSize + ")" + ((UtilsEnum.attribute)decalFirstAttrib) + "("+ firstRand + ")" + " and " + (UtilsEnum.attribute)decalSecondAttrib + "(" + secondRand + ")");

        blobAttribute |= (UtilsEnum.attribute)decalFirstAttrib;
        ActivateAtribute((UtilsEnum.attribute)decalFirstAttrib);
        blobAttribute |= (UtilsEnum.attribute)decalSecondAttrib;
        ActivateAtribute((UtilsEnum.attribute)decalSecondAttrib);



        GameObject gO = GameManager.instance.ui_manager.SpawnThis(attributeNeedPrefab, this.transform.position + Vector3.up * 3);
        layerSpeechBubble = gO.GetComponentInChildren<Animator>();
        gO.GetComponentInChildren<AttributeUI>().Initialization(this.transform, blobAttribute);
    }

    public void ChangeSize(UtilsEnum.attribute size)
    {
        switch (size)
        {
            case UtilsEnum.attribute.small:
                _animator.transform.localScale = smallSize;
                _collider.radius = smallSize.x / 2f;
                weight = smallWeight;
                rangeAttack = smallrangeAttack;
                break;
            case UtilsEnum.attribute.medium:
                _animator.transform.localScale = mediumSize;
                _collider.radius = mediumSize.x / 2f;
                weight = mediumWeight;
                rangeAttack = mediumrangeAttack;
                break;
            case UtilsEnum.attribute.big:
                _animator.transform.localScale = largeSize;
                _collider.radius = largeSize.x / 2f;
                weight = largeWeight;
                rangeAttack = largerangeAttack;
                break;
            default:
                break;
        }
    }

    public void ActivateAtribute(UtilsEnum.attribute attribute)
    {
        switch (attribute)
        {
            case UtilsEnum.attribute.spicy:
                spicy.Play();
                break;
            case UtilsEnum.attribute.watery:
                _meshRenderer.material = matWater;
                break;
            case UtilsEnum.attribute.horn:
                horn.SetActive(true);
                break;
            case UtilsEnum.attribute.noble:
                nobleHat.SetActive(true);
                break;
            default:
                break;
        }
    }

    public void ClearAttribute()
    {

        spicy.Stop();
        _meshRenderer.material = defaultMat;
        horn.SetActive(false);
        nobleHat.SetActive(false);
    }

    #endregion

    #region Movement

    public void Update()
    {
        if (alive)
        {
            //Follow a clear path
            FollowPath();
        }
        else if (draggedBy != null)
        {
            //Follow the player 
            DraggedByPlayer();

        }

        AttributeBubbleManagement();

    }

   public void AttributeBubbleManagement()
    {
        float val = layerSpeechBubble.GetLayerWeight(1);

        if(draggedBy != null)
        {
            if (val == 0)
                return;
            layerSpeechBubble.SetLayerWeight(1, Mathf.Max(val - Time.deltaTime * 3f, 0));
        }
        else
        {
            if (val == 1)
                return;
            layerSpeechBubble.SetLayerWeight(1, Mathf.Min(val +Time.deltaTime * 3f,1));
        }
    }

    public float timerPause = 0;
    public Vector2 randomWaitMinMax = new Vector2(2f, 6.5f);
    private float randomWait = 0;

    public void FollowPath()
    {

        if (nextTargetPosition == this.transform.position)
        {
            Debug.Log("Or else...");
            //wait for timer
            timerPause += Time.deltaTime;
            if(timerPause > randomWait)
            {
                //At the end of the wait, they move to the "next point"
                //so choose the next non target point

                mySpawner.RemoveThisPoint(nextTarget);
                nextTarget = mySpawner.GetNewIdlingPosition();
                nextTargetPosition = nextTarget.position;
            }
        }
        else
        {
            //move to the point
            Vector3 direction = nextTargetPosition - this.transform.position;
            //move at a constant speed (direction.normalize * speed ) 
            if(direction.magnitude > speed * Time.deltaTime)
            {
                //Move to it
                Debug.Log("Orhere");

                this.transform.Translate(direction.normalized * speed * Time.deltaTime);
            }
            else
            {
                Debug.Log("Here");
                this.transform.position = nextTargetPosition;
                randomWait = Random.Range(randomWaitMinMax.x, randomWaitMinMax.y);
                timerPause = 0;
                //From one point to another one
                //finish, start the wait !
            }

        }

    }

    public void DraggedByPlayer()
    {
        Vector3 direction = draggedBy.position - this.transform.position;
        Debug.DrawRay(this.transform.position, direction, Color.red);
        Debug.DrawRay(this.transform.position, direction.normalized * dragDistance, Color.green);
        if(direction.sqrMagnitude > dragDistance)
        {
            nextTargetPosition = (draggedBy.position) - direction * dragDistance * 0.5f;  //vient un peu plus proche mais pas collé en gros
        }
        this.transform.position = this.transform.position * dragEffect + nextTargetPosition * (1 - dragEffect);
    }
    #endregion

    public void DraggedStart(Transform transformToFollow)
    {
        draggedBy = transformToFollow;
    }

    public void DraggedEnd()
    {
        draggedBy = null;
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

    public void Destroy()
    {
        Destroy(layerSpeechBubble.gameObject);
        Destroy(this.gameObject);
    }
}
