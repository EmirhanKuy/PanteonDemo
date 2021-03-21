using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpponentScript : MonoBehaviour
{
    private float movementSpeed;
    private float turnSmoothness;
    private float movementSpeedCache;

    private Vector3 facingDirection;
    private Vector3 initialPosition;

    PlayerController playerControllerInstance;

    //Variables for AI movement
    private bool moveRight = false;
    private bool moveLeft = false;
    private float rightWeight;
    private float leftWeight;
    private float timer;

    void Start()
    {
        //The direction i want my character to move on
        facingDirection = Vector3.forward;
        initialPosition = transform.position;

        //Get the variables from player so changing it from the editor doesnt require to changes to both
        playerControllerInstance = FindObjectOfType<PlayerController>();
        movementSpeed = playerControllerInstance.movementSpeed;
        turnSmoothness = playerControllerInstance.turnSmoothness;

        movementSpeedCache = movementSpeed;
    }

    void Update()
    {
        //Turn & move forward
        transform.forward = Vector3.Lerp(transform.forward, facingDirection, turnSmoothness);
        transform.position += transform.forward * movementSpeed * Time.deltaTime;

        //AI movement
        //Rays i will use for AI movement
        //Front
        Debug.DrawRay(transform.position, -transform.up, Color.white);
        Debug.DrawRay(transform.position + transform.right * 0.28f + transform.up * 0.4f, transform.forward * 5f, Color.white);
        Debug.DrawRay(transform.position - transform.right * 0.3f + transform.up * 0.4f, transform.forward * 5f, Color.white);
        //Sides
        Debug.DrawRay(transform.position + transform.up * 0.2f, transform.right * 10f, Color.white);
        Debug.DrawRay(transform.position + transform.up * 0.2f, -transform.right * 10f, Color.white);


        //Ray positions according to character colliders and general obstacle height
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -transform.up, out hit, 1f))
        {
            if (hit.transform.CompareTag("RotatingPlatform"))
                if (transform.position.z < hit.transform.position.z)
                    transform.position += -transform.right.normalized * movementSpeed * Time.deltaTime;
                else if (transform.position.z > hit.transform.position.z)
                    transform.position += transform.right.normalized * movementSpeed * Time.deltaTime;

        }
        //Is there an obstacle on the path
        //Check both       (((((   || in one if statement doesnt work?   )))))
        if (Physics.Raycast(transform.position + transform.right * 0.28f + transform.up * 0.4f, transform.forward, out hit, 5f))
        {
            if (hit.transform.CompareTag("Obstacle"))
                moveLeft = true;
        }
        if (Physics.Raycast(transform.position - transform.right * 0.3f + transform.up * 0.4f, transform.forward, out hit, 5f))
        {
            if (hit.transform.CompareTag("Obstacle"))
                moveRight = true;
        }

        //Calculating the free distance on both sides
        //Prevent movement if there isnt enough space
        if (Physics.Raycast(transform.position + transform.up * 0.2f, transform.right, out hit, 10f))
        {
            rightWeight = (Vector3.Scale(hit.transform.position, transform.right) - Vector3.Scale(transform.position, transform.right)).magnitude;

            if (hit.transform.CompareTag("Sides") && rightWeight < 1f)
                moveRight = false;
        }

        if (Physics.Raycast(transform.position + transform.up * 0.2f, -transform.right, out hit, 10f))
        {
            leftWeight = (Vector3.Scale(hit.transform.position, -transform.right) - Vector3.Scale(transform.position, -transform.right)).magnitude;
            if (hit.transform.CompareTag("Sides") && leftWeight < 1f)
                moveLeft = false;
        }

        //Debug.Log(rightWeight);
        //Debug.Log(leftWeight);

        if (moveLeft || moveRight)
        {
            //Moving left if visibly closer to the left side of the obstacle
            //Moving right if vice-versa
            //If cant tell move random
            if (moveLeft && !moveRight)
            {
                moveLeft = false;
                transform.position += -transform.right.normalized * movementSpeed * Time.deltaTime;
                //transform.position = Vector3.Lerp(transform.position, transform.position - transform.right * 0.3f * 2, turnSmoothness * 0.5f);
            }
            else if (!moveLeft && moveRight)
            {
                moveRight = false;
                transform.position += transform.right.normalized * movementSpeed * Time.deltaTime;
            }
            else if (moveLeft && moveRight && Mathf.Abs(leftWeight - rightWeight) < 1f && timer < Time.time)
            {
                timer = Time.time + 1f;
            }
            else if (moveLeft && moveRight && Mathf.Abs(leftWeight - rightWeight) < 1f && timer > Time.time)
            {
                moveRight = false;
                transform.position += transform.right.normalized * movementSpeed * Time.deltaTime;
            }
            else if (moveLeft && moveRight)
            {
                if (leftWeight > rightWeight)
                {
                    moveLeft = false;
                    transform.position += -transform.right.normalized * movementSpeed * Time.deltaTime;
                }
                else
                {
                    moveRight = false;
                    transform.position += transform.right.normalized * movementSpeed * Time.deltaTime;
                }
            }
        }
    }

    //Moving direction changes when hit by the collider
    private void OnTriggerEnter(Collider collider)
    {
        //Turn Right
        if (collider.transform.CompareTag("TurnRight"))
            facingDirection = transform.right;
        //Destroy after falling
        else if (collider.transform.CompareTag("Teleport"))
            Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Go back
        if (collision.transform.CompareTag("Obstacle"))
        {
            transform.position = initialPosition;
            facingDirection = Vector3.forward;
        }
        //Stagger
        if (collision.transform.CompareTag("Sides") || collision.transform.CompareTag("Player"))
        {
            Component[] renderers;
            renderers = gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();

            movementSpeed = 0f;

            foreach (SkinnedMeshRenderer renderer in renderers)
                renderer.enabled = false;

            StartCoroutine(staggerTimer());
        }
    }

    //Stagger Timer
    IEnumerator staggerTimer()
    {
        yield return new WaitForSeconds(0.3f);

        Component[] renderers;
        renderers = gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();

        movementSpeed = movementSpeedCache;

        foreach (SkinnedMeshRenderer renderer in renderers)
            renderer.enabled = true;
    }
}
