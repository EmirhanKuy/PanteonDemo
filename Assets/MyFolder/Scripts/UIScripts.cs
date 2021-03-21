using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScripts : MonoBehaviour
{
    PlayerController playerInstance;
    GameObject[] opponentInstance;
    [SerializeField] private Text[] textArray;
    [SerializeField] private Text winText;

    //Arrays we will use for remaining distance of object and who it is
    private float[] distance;
    private string[] nameIndex;
    bool gameOver;

    //temporary variables for sorting the array
    float temp;
    string temp2;
    int minIndex;

    void Start()
    {
        //Get instaces of every character to find out their position and direction
        playerInstance = FindObjectOfType<PlayerController>();
        opponentInstance = new GameObject[2];
        opponentInstance[0] = GameObject.Find("Girl");
        opponentInstance[1] = GameObject.Find("Girl (1)");

        distance = new float[3];
        nameIndex = new string[3];
        gameOver = false;
    }

    void LateUpdate()
    {
        if (playerInstance.stage == 1)
        {
            //Find everyones distance to the finish line to see who's first
            if (playerInstance.transform.forward == Vector3.forward)
                distance[0] = 40f + GameObject.Find("Destination").transform.position.z - playerInstance.transform.position.z;
            else
                distance[0] = GameObject.Find("DestinationII").transform.position.x - playerInstance.transform.position.x;

            for (int i = 1; i < distance.Length; i++)
            {
                if (opponentInstance[i - 1].transform.forward == Vector3.forward)
                    distance[i] = 40f + GameObject.Find("Destination").transform.position.z - opponentInstance[i - 1].transform.position.z;
                else
                    distance[i] = GameObject.Find("DestinationII").transform.position.x - opponentInstance[i - 1].transform.position.x;
            }

            //Name index required for knowing whose distance we have
            nameIndex[0] = "Player";
            nameIndex[1] = "Opponent";
            nameIndex[2] = "Opponent 2";

            //Sort the array with corresponding names
            for (int i = 0; i < distance.Length - 1; i++)
            {
                minIndex = i;
                for (int j = i + 1; j < distance.Length; j++)
                    if (distance[j] < distance[minIndex])
                        minIndex = j;

                temp = distance[i];
                temp2 = nameIndex[i];

                distance[i] = distance[minIndex];
                nameIndex[i] = nameIndex[minIndex];
                distance[minIndex] = temp;
                nameIndex[minIndex] = temp2;
            }

            //Assign correct values in ascending order from top to down to UI texts
            for (int i = 0; i < distance.Length; i++)
                textArray[i].text = nameIndex[i];

            //Check for finish line and print winner
            if (distance[0] < 0f && gameOver == false)
            {
                gameOver = true;
                winText.text = nameIndex[0] + " wins";
                winText.enabled = true;
                StartCoroutine(winTextTimer());

                for (int i = 0; i < distance.Length; i++)
                    textArray[i].text = "";
            }
        }
    }

    IEnumerator winTextTimer()
    {
        yield return new WaitForSeconds(2f);
        winText.text = "";
    }
}