using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingPlatformScript : MonoBehaviour
{
    [SerializeField] private float rotateTimerMin = 0f;
    [SerializeField] private float rotateTimerMax = 5f;
    [SerializeField] private float rotationSpeed;
    private int rotationDirectionLeft = 0;
    private int rotationDirectionRight = 2;
    private float rotationDirection;
    private bool rotationChange = true;
    private Rigidbody rigidBody;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        //Picks a direction and time between 1-5 seconds.
        //Rotates towards that direction for that amount of seconds after that rotation direction is chosen again randomly
        if (rotationChange == true)
        {
            rotationChange = false;
            StartCoroutine(rotationChangeTimer());
            
            rotationDirection = Mathf.Pow(-1, Random.Range(rotationDirectionLeft, rotationDirectionRight));
        }

        rigidBody.angularVelocity = transform.forward * rotationDirection * rotationSpeed * Time.deltaTime;
    }

    IEnumerator rotationChangeTimer()
    {
        yield return new WaitForSeconds(Random.Range(rotateTimerMin, rotateTimerMax));
        rotationChange = true;
    }
}
