using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paintable : MonoBehaviour
{
    [SerializeField] private GameObject brush;
    [SerializeField] private float brushSize;
    PlayerController playerInstance;

    void Start()
    {
        playerInstance = FindObjectOfType<PlayerController>();

    }

    void Update()
    {
        //Only at the paint stage of the game
        if (playerInstance.stage == 2)
        {
            //Disabled the part where i wanted to calculate paint percent by rays - didnt work :(
            //----------
            //RaycastHit hitInfo;
            //for (int i = 0; i < amountRays / 10; i++)
            //{
            //    for (int j = 0; j < amountRays / 10; j++)
            //    {
            //        ray = new Ray();
            //        ray.origin = rayStart.transform.position - rayStart.transform.forward * i - rayStart.transform.up * j;
            //        //Debug.DrawRay(ray.origin, rayStart.transform.right * 10f, Color.white);

            //        if (Physics.Raycast(ray.origin, rayStart.transform.right, out hitInfo, 50f))
            //        {
            //            if (hitInfo.transform.CompareTag("Wall"))
            //                counter++;
            //        }
            //    }
            //}

            //percent = amountRays - counter / amountRays;
            //Debug.Log(percent);
            //---------

            //Instantiates a red paint material at hit position to wall
            if (Input.GetMouseButton(0))
            {
                var inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(inputRay, out hit))
                {
                    var go = Instantiate(brush, hit.point + Vector3.left * 0.1f, transform.rotation, transform);
                    go.transform.localScale = Vector3.one * brushSize;
                }
            }
        }
    }
}
