using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DonutMovingStickScript : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float obstacleSpeed;
    private Vector3 initialPos;
    private Vector3 childPos;
    private Vector3 nextTarget;

    void Start()
    {
        childPos = target.position;
        initialPos = transform.position;
        nextTarget = childPos;
        Destroy(target.gameObject);
    }

    void Update()
    {
        //Moves donut sticks between 2 points - child and initial position
        //Can edit child position on editor
        if ((transform.position - nextTarget).magnitude < 0.1f && nextTarget == childPos)
            nextTarget = initialPos;
        else if ((transform.position - nextTarget).magnitude < 0.1f && nextTarget == initialPos)
            nextTarget = childPos;

        transform.position += (nextTarget - transform.position).normalized * obstacleSpeed * Time.deltaTime;
    }
}
