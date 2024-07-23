using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKFootSolver : MonoBehaviour
{
    [SerializeField] private LayerMask terrainLayer;
    [SerializeField] private IKFootSolver otherFoot;
    [SerializeField] private float groundHeight = 5f;
    [SerializeField] private float stepDistance, stepHeight, stepLenght, footSpacing, speed;
    [SerializeField] private Transform body;
    [SerializeField] private Vector3 footOffset;
    private Vector3 oldPosition, newPosition, currentPosition;
    private Vector3 oldNormal, currentNormal, newNormal;
    private Vector3 initialPostion;
    private float lerp;
    bool canLerpBack = false;
    Ray ray;

    private void Start()
    {
        initialPostion = transform.localPosition;
        footSpacing = transform.localPosition.x;
        oldPosition = newPosition = currentPosition = transform.position;
        oldNormal = currentNormal = newNormal = transform.up;
        lerp = 1;
    }

    private void Update()
    {
        transform.up = currentNormal;
        ray = new Ray(body.position + (body.right * footSpacing), Vector3.down);
        UpdateNewPosition();
        UpdateLocalPosition();

        if (lerp < 1)
        {
            Vector3 tempPos = Vector3.Lerp(oldPosition, newPosition, lerp);
            tempPos.y += Mathf.Sin(lerp * Mathf.PI) * stepHeight;
            currentPosition = tempPos;
            currentNormal = Vector3.Lerp(oldNormal, newNormal, lerp);
            lerp += Time.deltaTime * speed;
        }
        else
        {
            oldPosition = newPosition;
            oldNormal = newNormal;
        }
    }

    private void UpdateNewPosition()
    {
        if (Physics.Raycast(ray, out RaycastHit hit, 10))
        {
            if (Vector3.Distance(newPosition, hit.point) > stepDistance && !otherFoot.isMoving() && lerp >= 1)
            {
                lerp = 0;
                int direction = body.InverseTransformPoint(hit.point).z > body.InverseTransformPoint(newPosition).z ? 1 : -1;
                newPosition = hit.point + (body.forward * stepLenght * direction) + footOffset;
                newNormal = hit.normal;
            }
        }
    }

    private void UpdateLocalPosition()
    {
        if (Physics.Raycast(ray, out RaycastHit hit, groundHeight))
        {
            transform.position = currentPosition;
 
        }
        else if (canLerpBack)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, initialPostion, Time.deltaTime);
         
        }

        if (transform.localPosition != initialPostion)
            canLerpBack = true;
        else
            canLerpBack = false;

    }

    public bool isMoving()
    {
        return lerp < 1;
    }
}