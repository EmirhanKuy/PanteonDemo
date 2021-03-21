using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float movementSpeed;
    public float turnSmoothness;
    public float movementSpeedCache;
    public int stage;

    private Vector3 initialPosition;
    private Vector3 facingDirection;

    void Start()
    {
        stage = 1;
        initialPosition = transform.position;
        //Direction i want my character to move on
        facingDirection = Vector3.forward;
        movementSpeedCache = movementSpeed;
    }

    void Update()
    {
        //Only at the runner stage of the game
        if (stage == 1)
        {
            //Turning Right
            transform.forward = Vector3.Lerp(transform.forward, facingDirection, turnSmoothness);

            //Movement
            if (Input.GetKey(KeyCode.A))
                transform.position += -transform.right * movementSpeed * Time.deltaTime;
            if (Input.GetKey(KeyCode.D))
                transform.position += transform.right * movementSpeed * Time.deltaTime;

            transform.position += transform.forward * movementSpeed * Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        //Turn Right
        if (collider.transform.CompareTag("TurnRight"))
            facingDirection = transform.right;
        //Teleport to Drawing Board and Destroy unnecessary components move game to stage 2 paint stage
        else if (collider.transform.CompareTag("Teleport"))
        {
            stage = 2;
            //movementSpeed = 0f;
            Destroy(GameObject.Find("Girl"));
            Destroy(GameObject.Find("Girl (1)"));
            Destroy(GameObject.Find("Canvas"));
            transform.position = new Vector3(1000f, 1f, 1000f);
            gameObject.SetActive(false);

            //Camera.main.transform.position = new Vector3(1000f, 1f, 1000f);
        }
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
