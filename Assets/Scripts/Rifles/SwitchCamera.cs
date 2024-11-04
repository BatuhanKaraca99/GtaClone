using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchCamera : MonoBehaviour
{
    [Header("Camera to Assign")]
    public GameObject AimCam;
    public GameObject ThirdpersonCamera;
    public Animator animator;

    private void Update()
    {
        if(Input.GetButton("Fire2") && Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            animator.SetBool("AimWalk", true);
            animator.SetBool("ShootAim", false);

            ThirdpersonCamera.SetActive(false);
            AimCam.SetActive(true);
        }
        else if (Input.GetButton("Fire2"))
        {
            animator.SetBool("ShootAim", true);
            animator.SetBool("AimWalk", true);

            ThirdpersonCamera.SetActive(false);
            AimCam.SetActive(true);
        }
        else
        {
            animator.SetBool("ShootAim", false);
            animator.SetBool("AimWalk", false);

            ThirdpersonCamera.SetActive(true);
            AimCam.SetActive(false);
        }
    }

}
