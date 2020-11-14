using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacter : MonoBehaviour
{
    public Camera mainCamera;
    public Animator visualAnimator;
    public LayerMask layerMaskForMovement;
    
    private Vector3 destination = new Vector3(0,-1,0);

    public Blob currentBlobTarget;
    public enum actionToDoWithTarget
    {
        none,
        attack,
        drag,
        drop,
    }
    public actionToDoWithTarget actionToDo = actionToDoWithTarget.none;
    public Blob currentDraggedBlob;

    [Header("GD data")]
    public float speed = 3f;
    private float initialSpeed = 3f;
    public float rangeMultiplier = 1f;
    public float rangeMovement = 3f;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = FindObjectOfType<Camera>();
        destination = this.transform.position;
        initialSpeed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 10;
            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mousePos);

            //DEBUG//target.position = worldPosition;

            RaycastHit hit;
            // Does the ray intersect any objects excluding the player layer
            if (Physics.Raycast(mainCamera.transform.position, worldPosition - mainCamera.transform.position, out hit, 50f, layerMaskForMovement))
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
//                Debug.Log("Did Hit");

                destination = hit.point;
                destination.y = -1;

                Blob blobHit = hit.collider.GetComponent<Blob>();
                if (blobHit != null)
                {
                    currentBlobTarget = blobHit;
                    actionToDo = actionToDoWithTarget.attack;
                }

            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 10;
            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mousePos);

            //DEBUG//target.position = worldPosition;

            RaycastHit hit;
            // Does the ray intersect any objects excluding the player layer
            if (Physics.Raycast(mainCamera.transform.position, worldPosition - mainCamera.transform.position, out hit, 50f, layerMaskForMovement))
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
                //Debug.Log("Did Hit");

                destination = hit.point;
                destination.y = -1;

                Blob blobHit = hit.collider.GetComponent<Blob>();
                if (blobHit != null)
                {
                    currentBlobTarget = blobHit;
                    actionToDo = actionToDoWithTarget.drag;
                }
                else if(currentDraggedBlob != null)
                {
                    currentBlobTarget = null;
                    //Go to drop it
                    actionToDo = actionToDoWithTarget.drop;
                }

            }
        }


        MovementManagement();
    }
    
    public void MovementManagement()
    {
        if((this.transform.position - destination).sqrMagnitude > speed * Time.deltaTime + rangeMovement)
        {
            Vector3 direction = (destination - this.transform.position).normalized;
            this.transform.Translate(direction * speed * Time.deltaTime);

            //Rotation :
            Quaternion quat = Quaternion.FromToRotation(Vector3.back, direction);
            visualAnimator.transform.rotation = quat;
            //Cool trick : make it with a lerp ?

            visualAnimator.SetBool("Walk", true);
        }
        else
        {
            destination = this.transform.position;

            visualAnimator.SetBool("Walk", false);
            if(actionToDo == actionToDoWithTarget.drop)
            {
                DropBlob();
                actionToDo = actionToDoWithTarget.none;
            }
        }

        if (currentBlobTarget != null)
        {
            float distance = (this.transform.position - currentBlobTarget.transform.position).sqrMagnitude;
            if (distance < currentBlobTarget.rangeAttack * rangeMultiplier)
            {
                ActionMaker();
            }
        }
    }

    public void ActionMaker()
    {
        switch (actionToDo)
        {
            case actionToDoWithTarget.none:
                break;
            case actionToDoWithTarget.attack:
                if (currentBlobTarget.alive)
                {
                    DealDamageToBlob();
                    currentBlobTarget = null;
                }
                else
                {
                    visualAnimator.SetTrigger("Attack");
                    currentBlobTarget = null;
                }
                break;
            case actionToDoWithTarget.drag:
                DropBlob();
                DragBlob();
                break;
            default:
                break;
        }
        actionToDo = actionToDoWithTarget.none;
    }


    public void DealDamageToBlob()
    {


        visualAnimator.SetTrigger("Attack");
        currentBlobTarget.TakeDamage(1);
    }

    public void DragBlob()
    {
        Debug.Log("Hi ?");
        visualAnimator.SetBool("Grab", true);
        currentBlobTarget.DraggedStart(this.transform);
        speed /= currentBlobTarget.weight;
        currentDraggedBlob = currentBlobTarget;
        currentBlobTarget = null;
    }

    public void DropBlob()
    {
        if (currentDraggedBlob == null)
            return;

        visualAnimator.SetBool("Grab", false);
        speed *= currentDraggedBlob.weight;
        if (initialSpeed != speed)
        {
            Debug.LogError("Initial speed != speed after calculus " + initialSpeed + " vs " + speed);
            speed = initialSpeed;
        }

        currentDraggedBlob.DraggedEnd();
        currentDraggedBlob = null;
    }

}
