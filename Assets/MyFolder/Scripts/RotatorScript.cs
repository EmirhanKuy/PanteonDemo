using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatorScript : MonoBehaviour
{
    [SerializeField] private float rotationSpeed;

    void Update()
    {
        //Rotates the base of the rotator stick
        transform.Rotate(Vector3.up * rotationSpeed);
    }
}
