using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float cameraSmoothness;
    private float cameraDistance;
    private float cameraHeight;

    private PlayerController playerControllerInstance;

    void Start()
    {
        cameraDistance = GameObject.Find("Boy").transform.position.z - transform.position.z;
        cameraHeight = transform.position.y;

        playerControllerInstance = FindObjectOfType<PlayerController>();
    }

    void LateUpdate()
    {
        //Follow player by the scene distance
        //Rotate towards players forward direction
        transform.position = playerControllerInstance.transform.position - playerControllerInstance.transform.forward * cameraDistance + Vector3.up * cameraHeight;
        transform.forward = Vector3.Lerp(transform.forward, playerControllerInstance.transform.forward, cameraSmoothness);
    }
}
