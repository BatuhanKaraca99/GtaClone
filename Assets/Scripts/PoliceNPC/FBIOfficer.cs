using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FBIOfficer : MonoBehaviour
{
    [Header("Character Info")]
    public float movingSpeed = 0.9f;
    public float runningSpeed = 2.2f;
    private float CurrentMovingSpeed;
    public float turningSpeed = 300f;
    public float stopSpeed = 1f;
    private float characterHealth = 100f;
    public float presentHealth;

    [Header("Destination Var")]
    public Vector3 destination;
    public bool destinationReached;
    public Animator animator;

    [Header("FBI AI")]
    public GameObject playerBody;
    public LayerMask playerLayer;
    public float visionRadius = 30f;
    public float shootingRadius = 5f;
    public bool playerInvisionRadius;
    public bool playerInshootingRadius;

    [Header("FBI Shooting Var")]
    public float giveDamageOf = 3f;
    public float shootingRange = 100f;
    public GameObject shootingRaycastArea;
    public float timebtwShoot = 1f;
    public bool previouslyShoot;

    public WantedLevel wantedLevelScript;
    public Player player;
    public GameObject bloodEffect;

    private void Start()
    {
        playerBody = GameObject.Find("Player");
        presentHealth = characterHealth;
        wantedLevelScript = GameObject.FindObjectOfType<WantedLevel>();
        CurrentMovingSpeed = movingSpeed;
        player = GameObject.FindObjectOfType<Player>();
    }

    private void Update()
    {
        playerInvisionRadius = Physics.CheckSphere(transform.position, visionRadius, playerLayer);
        playerInshootingRadius = Physics.CheckSphere(transform.position, shootingRadius, playerLayer);

        if (!playerInvisionRadius && !playerInshootingRadius && (wantedLevelScript.level1 == false
        || wantedLevelScript.level2 == false || wantedLevelScript.level3 == false
        || wantedLevelScript.level4 == false || wantedLevelScript.level5 == false))
        {
            Walk();
        }
        if (playerInvisionRadius && !playerInshootingRadius && (wantedLevelScript.level5 == true))
        {
            ChasePlayer();
        }
        if (playerInvisionRadius && playerInshootingRadius && (wantedLevelScript.level5 == true))
        {
            ShootPlayer();
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

                animator.SetBool("Walk", true);
                animator.SetBool("Shoot", false);
                animator.SetBool("Run", false);
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

        animator.SetBool("Walk", false);
        animator.SetBool("Shoot", false);
        animator.SetBool("Run", true);

        CurrentMovingSpeed = runningSpeed;
    }

    public void ShootPlayer()
    {
        CurrentMovingSpeed = 0f;

        transform.LookAt(playerBody.transform);

        animator.SetBool("Walk", false);
        animator.SetBool("Shoot", true);
        animator.SetBool("Run", false);

        if (!previouslyShoot)
        {
            RaycastHit hit;
            if (Physics.Raycast(shootingRaycastArea.transform.position, shootingRaycastArea.transform.forward, out hit, shootingRange))
            {
                Debug.Log("Shooting" + hit.transform.name);

                PlayerScript playerBody = hit.transform.GetComponent<PlayerScript>();

                if (playerBody != null)
                {
                    playerBody.playerHitDamage(giveDamageOf);
                    GameObject bloodEffectGo = Instantiate(bloodEffect, hit.point, Quaternion.LookRotation(hit.normal));
                    Destroy(bloodEffectGo, 1f);
                }
            }
        }

        previouslyShoot = true;
        Invoke(nameof(ActiveShooting), timebtwShoot);
    }

    private void ActiveShooting()
    {
        previouslyShoot = false;
    }

    public void characterHitDamage(float takeDamage)
    {
        presentHealth -= takeDamage;

        if (presentHealth <= 0f)
        {
            animator.SetBool("Die", true);
            characterDie();
        }
    }

    private void characterDie()
    {
        gameObject.GetComponent<CapsuleCollider>().enabled = false;
        CurrentMovingSpeed = 0f;
        shootingRange = 0f;
        Object.Destroy(gameObject, 4.0f);
        player.currentkills += 1;
        player.playerMoney += 10;
    }
}
