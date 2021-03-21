using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HalfDonutScript : MonoBehaviour
{
    [SerializeField] private float rotationSpeed;
    void Update()
    {
        //Rotates the donut
        transform.Rotate(Vector3.right, rotationSpeed * Time.deltaTime);
    }
}
