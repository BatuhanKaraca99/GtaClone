using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Handgun : MonoBehaviour
{
    //Rifle Movement Variables

    [Header("Player Movement")]
    public float playerSpeed = 1.1f;
    public float playerSprint = 5f;

    [Header("Player Animator & Gravity")]
    public CharacterController cC;
    public float gravity = -9.81f;
    public Animator animator;

    [Header("Player Script Camera")]
    public Transform playerCamera;

    [Header("Player Jumping & Velocity")]
    public float jumpRange = 1f;
    public float turnCalmTime = 0.1f;
    float turnCalmVelocity;
    Vector3 velocity;
    public Transform surfaceCheck;
    bool onSurface;
    public float surfaceDistance = 0.4f;
    public LayerMask surfaceMask;

    //Rifle Shooting Variables

    [Header("Rifle Things")]
    public Camera cam;
    public float giveDamage = 10f;
    public float shootingRange = 100f;
    public float fireCharge = 10f;
    private float nextTimeToShoot = 0f;
    public Transform hand;
    public Transform playerTransform;
    public Handgun2 handgun2;
    public bool isMoving;

    [Header("Rifle Ammunition and Reloading")]
    private int maximumAmmunition = 25;
    public int mag = 10;
    private int presentAmmunition;
    public float reloadingTime = 4.3f;
    private bool setReloading = false;

    [Header("Rifle Effects")]
    public ParticleSystem muzzleSpark;
    public GameObject metalEffect;
    public GameObject bloodEffect;

    [Header("Sounds & UI")]
    public GameObject ammoOutUI;
    bool HandgunActive = true;

    private void Awake()
    {
        transform.SetParent(hand);
        Cursor.lockState = CursorLockMode.Locked;
        presentAmmunition = maximumAmmunition;
    }

    private void Update()
    {
        if(HandgunActive == true)
        {
            animator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("GunAnimator");
        }

        onSurface = Physics.CheckSphere(surfaceCheck.position, surfaceDistance, surfaceMask);

        if (onSurface && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        //gravity
        velocity.y += gravity * Time.deltaTime;
        cC.Move(velocity * Time.deltaTime);

        playerMove();
        Jump();
        Sprint();

        if (setReloading)
        {
            return;
        }

        if(presentAmmunition <= 0)
        {
            StartCoroutine(Reload());
            return;
        }

        if(isMoving == false)
        {
            if (Input.GetButton("Fire1") && Time.time >= nextTimeToShoot)
            {
                animator.SetBool("Shoot", true);
                nextTimeToShoot = Time.time + 1f / fireCharge;
                Shoot();
            }
            else
            {
                animator.SetBool("Shoot", false);
            }   
        }
    }

    void playerMove()
    {
        float horizontal_axis = Input.GetAxisRaw("Horizontal");
        float vertical_axis = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(horizontal_axis, 0f, vertical_axis).normalized;

        if (direction.magnitude >= 0.1f)
        {
            animator.SetBool("WalkForward", true);
            animator.SetBool("RunForward", false);

            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + playerCamera.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(playerTransform.eulerAngles.y, targetAngle, ref turnCalmTime, turnCalmTime);
            playerTransform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            cC.Move(moveDirection * playerSpeed * Time.deltaTime);
            jumpRange = 0f;
            isMoving = true;
            handgun2.isMoving = true;
        }

        else
        {
            animator.SetBool("WalkForward", false);
            animator.SetBool("RunForward", false);
            jumpRange = 1f;
            isMoving = false;
            handgun2.isMoving = false;
        }
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump") && onSurface)
        {
            animator.SetBool("IdleAim", false);
            animator.SetTrigger("Jump");
            velocity.y = Mathf.Sqrt(jumpRange * -2 * gravity);
        }

        else
        {
            animator.SetBool("IdleAim", true);
            animator.ResetTrigger("Jump");
        }
    }

    void Sprint()
    {
        if (Input.GetButton("Sprint") && Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) && onSurface)
        {
            float horizontal_axis = Input.GetAxisRaw("Horizontal");
            float vertical_axis = Input.GetAxisRaw("Vertical");

            Vector3 direction = new Vector3(horizontal_axis, 0f, vertical_axis).normalized;

            if (direction.magnitude >= 0.1f)
            {
                animator.SetBool("WalkForward", false);
                animator.SetBool("RunForward", true);

                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + playerCamera.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(playerTransform.eulerAngles.y, targetAngle, ref turnCalmTime, turnCalmTime);
                playerTransform.rotation = Quaternion.Euler(0f, angle, 0f);

                Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                cC.Move(moveDirection * playerSprint * Time.deltaTime);
                jumpRange = 0f;
                isMoving = true;
                handgun2.isMoving = true;
            }
            else
            {
                animator.SetBool("WalkForward", true);
                animator.SetBool("RunForward", false);
                jumpRange = 1f;
                isMoving = false;
                handgun2.isMoving = false;
            }
        }
    }

    void Shoot()
    {
        if(mag == 0)
        {
            // show ammo out text/UI
            StartCoroutine(ShowAmmoOut());
            return;
        }

        presentAmmunition--;

        if(presentAmmunition == 0)
        {
            mag--;
        }

        muzzleSpark.Play();
        RaycastHit hitInfo;

        if(Physics.Raycast(cam.transform.position,cam.transform.forward, out hitInfo,shootingRange))
        {
            Debug.Log(hitInfo.transform.name);

            Object obj = hitInfo.transform.GetComponent<Object>();
            PoliceOfficer policeOfficer = hitInfo.transform.GetComponent<PoliceOfficer>();
            CharacterNavigatorScript characterNavigatorScript = hitInfo.transform.GetComponent<CharacterNavigatorScript>();
            PoliceOfficer2 policeOfficer2 = hitInfo.transform.GetComponent<PoliceOfficer2>();

            if (obj != null) 
            {
                obj.objectHitDamage(giveDamage);
                GameObject metalEffectGo = Instantiate(metalEffect, hitInfo.point,Quaternion.LookRotation(hitInfo.normal));
                Destroy(metalEffectGo, 1f);
            }
            else if(policeOfficer != null)
            {
                policeOfficer.characterHitDamage(giveDamage);
                GameObject bloodEffectGo = Instantiate(bloodEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
                Destroy(bloodEffectGo, 1f);
            }
            else if (characterNavigatorScript != null)
            {
                characterNavigatorScript.characterHitDamage(giveDamage);
                GameObject bloodEffectGo = Instantiate(bloodEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
                Destroy(bloodEffectGo, 1f);
            }
            else if (policeOfficer2 != null)
            {
                policeOfficer2.characterHitDamage(giveDamage);
                GameObject bloodEffectGo = Instantiate(bloodEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
                Destroy(bloodEffectGo, 1f);
            }
        }
    }

    IEnumerator Reload()
    {
        playerSpeed = 0f;
        playerSprint = 0f;
        setReloading = true;
        Debug.Log("Reloading...");
        animator.SetBool("Reload", true);
        yield return new WaitForSeconds(reloadingTime);
        Debug.Log("Done Reloading...");
        animator.SetBool("Reload", false);
        presentAmmunition = maximumAmmunition;
        playerSpeed = 1.1f;
        playerSprint = 5f;
        setReloading = false;
    }

    IEnumerator ShowAmmoOut()
    {   
        ammoOutUI.SetActive(true);
        yield return new WaitForSeconds(5f);
        ammoOutUI.SetActive(false);
    }
}
