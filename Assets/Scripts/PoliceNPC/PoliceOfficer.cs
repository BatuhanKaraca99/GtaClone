using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliceOfficer : MonoBehaviour
{
    [Header("Character Info")]
    public float movingSpeed;
    public float runningSpeed;
    private float CurrentMovingSpeed;
    public float turningSpeed = 100f;
    public float stopSpeed = 1f;

    [Header("Destination Var")]
    public Vector3 destination;
    public bool destinationReached;

    [Header("Police AI")]
    public GameObject playerBody;
    public LayerMask playerLayer;
    public float visionRadius;
    public float shootingRadius;
    public bool playerInvisionRadius;
    public bool playerInshootingRadius;

    [Header("Police Shooting Var")]
    public WantedLevel wantedLevelScript;

    private void Start()
    {
        playerBody = GameObject.Find("Player");
        wantedLevelScript = GameObject.FindObjectOfType<WantedLevel>();
        CurrentMovingSpeed = movingSpeed;
    }

    private void Update()
    {
        playerInvisionRadius = Physics.CheckSphere(transform.position, visionRadius, playerLayer);
        playerInshootingRadius = Physics.CheckSphere(transform.position, shootingRadius, playerLayer);
    
        if(playerInvisionRadius && !playerInshootingRadius && wantedLevelScript.level1 == false
        || wantedLevelScript.level2 == false || wantedLevelScript.level3 == false
        || wantedLevelScript.level4 == false || wantedLevelScript.level5 == false)
        {
            Walk();
        }
        if(playerInvisionRadius && !playerInshootingRadius && wantedLevelScript.level1 == true
        || wantedLevelScript.level2 == true || wantedLevelScript.level3 == true
        || wantedLevelScript.level4 == true || wantedLevelScript.level5 == true)
        {
            ChasePlayer();
        }
    }
    
    public void Walk()
    {
        if (transform.position != destination)
        {
            Vector3 destinationDirection = destination - transform.position;
            destinationDirection.y = 0;
            float destinationDistance = destinationDirection.magnitude;

            if (destinationDistance >= stopSpeed)
            {
                //turning
                destinationReached = false;
                Quaternion targetRotation = Quaternion.LookRotation(destinationDirection);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turningSpeed * Time.deltaTime);

                //Move AI
                transform.Translate(Vector3.forward * movingSpeed * Time.deltaTime);
            }
            else
            {
                destinationReached = true;
            }
        }
    }

    public void LocateDestination(Vector3 destination)
    {
        this.destination = destination;
        destinationReached = false;
    }

    public void ChasePlayer()
    {
        transform.position += transform.forward * CurrentMovingSpeed * Time.deltaTime;
        transform.LookAt(playerBody.transform);

        CurrentMovingSpeed = runningSpeed;
    }
}
