using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalObstacleScript : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float obstacleSpeed;
    [SerializeField] private float rotationSpeed;
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
        //Makes child position next target
        //Makes initial position next target after reaching child
        //Moves between targets
        if ((transform.position - nextTarget).magnitude < 0.1f && nextTarget == childPos)
            nextTarget = initialPos;
        else if((transform.position - nextTarget).magnitude < 0.1f && nextTarget == initialPos)
            nextTarget = childPos;

        //Rotation and the movement of the stick
        //transform.position = Vector3.Lerp(transform.position, nextTarget, obstacleSpeedSmoothness);
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        transform.position += (nextTarget - transform.position).normalized * obstacleSpeed * Time.deltaTime;
    }

    //To see & debug the move path
    void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        if (nextTarget != Vector3.zero)
            Gizmos.DrawLine(transform.position, nextTarget);
        else
            Gizmos.DrawLine(transform.position, target.position);
    }
}
