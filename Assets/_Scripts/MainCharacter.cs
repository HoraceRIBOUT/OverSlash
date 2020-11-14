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
    public Blob currentDraggedBlob;

    [Header("GD data")]
    public float speed = 3f;
    public float rangeAttack = 3f;
    public float rangeMovement = 3f;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = FindObjectOfType<Camera>();
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
                Debug.Log("Did Hit");

                destination = hit.point;
                destination.y = -1;

                Blob blobHit = hit.collider.GetComponent<Blob>();
                if (blobHit != null)
                {
                    currentBlobTarget = blobHit;
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

            visualAnimator.SetBool("Walk", true);
        }
        else
        {
            destination = this.transform.position;

            visualAnimator.SetBool("Walk", false);
        }

        if (currentBlobTarget != null)
        {
            float distance = (this.transform.position - currentBlobTarget.transform.position).sqrMagnitude;
            if (distance < rangeAttack)
            {

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
            }
        }
    }

    
    public void DealDamageToBlob()
    {
        visualAnimator.SetTrigger("Attack");
        currentBlobTarget.TakeDamage(1);
    }

}
