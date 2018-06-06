using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour {

    public float mouseSensitivity = 1;
    public float movementSpeed;
    public float jumpForce;
    public float runMultiplier;

    private Rigidbody rb;

    private bool isGrounded = true;




    /////////////////////////////////////
    private Transform cam;

    public float ammo;
    public float range;
    public float fireRate;
    public float impactForce;

    private float nextTimeToFire = 0;
    /////////////////////////////////////

    /////////////////////////////////////
    private Quaternion playerTargetRotation;
    private Quaternion cameraTargetRotation;


    /////////////////////////////////////


    void Start () {
        /////////////////////////////////////
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        rb = GetComponent<Rigidbody>();
        cam = transform.GetChild(0);
        /////////////////////////////////////

        /////////////////////////////////////
        playerTargetRotation = transform.localRotation;
        cameraTargetRotation = cam.localRotation;
    }

    void Update () {
        GroundCheck();





        /////////////////////////////////////
        float moveX = 0;
        float moveY = 0;

        if (Input.GetKey(KeyCode.W))
        {
            moveX += 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            moveX -= 1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            moveY += 1;
        }
        if (Input.GetKey(KeyCode.A))
        {
            moveY -= 1;
        }
        if (Input.GetKey(KeyCode.LeftShift))
            moveX = moveX * runMultiplier;
        /////////////////////////////////////



        /////////////////////////////////////
        Vector3 x = transform.forward * moveX;
        Vector3 y = transform.right * moveY;
        rb.AddForce((x + y) * movementSpeed);


        /////////////////////////////////////
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded == true)
            rb.AddForce(0, jumpForce, 0);
        /////////////////////////////////////

        /////////////////////////////////////
        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
        }
        /////////////////////////////////////



        /////////////////////////////////////
        float mouseX = Input.GetAxis("Mouse Y") * -1 * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse X") * mouseSensitivity;

        playerTargetRotation *= Quaternion.Euler(0, mouseY, 0);
        cameraTargetRotation *= Quaternion.Euler(mouseX, 0, 0);

        cameraTargetRotation = ClampRotationAroundXAxis(cameraTargetRotation);

        transform.localRotation = playerTargetRotation;
        cam.localRotation = cameraTargetRotation;
        /////////////////////////////////////
    }

    void GroundCheck()
    {
        RaycastHit hitInfo;

        if (Physics.SphereCast(transform.position, GetComponent<CapsuleCollider>().radius, Vector3.down, out hitInfo, 1.02f, Physics.AllLayers, QueryTriggerInteraction.Ignore))
        {
            isGrounded = true;
            rb.drag = 5;
        }
        else
        {
            isGrounded = false;
            rb.drag = 2;
        }
    }

    void Shoot()
    {
        RaycastHit hit;

        if (Physics.Raycast(cam.position, cam.forward, out hit, range))
        {
            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * impactForce);
            }
            //GameObject impactGo = Instantiate()
        }
    }

    Quaternion ClampRotationAroundXAxis(Quaternion q)
    {
        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1f;

        float angleX = 2f * Mathf.Rad2Deg * Mathf.Atan(q.x);

        angleX = Mathf.Clamp(angleX, -80f, 80f);

        q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

        return q;
    }
}
