using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatorStickScript : MonoBehaviour
{
    [SerializeField] private float horsePower;
    void OnCollisionEnter(Collision other)
    {
        //Adds force to the players towards the rotating vector
        if (other.collider.transform.CompareTag("Player"))
            other.collider.GetComponent<Rigidbody>().AddForce(horsePower * transform.right);
    }
}
