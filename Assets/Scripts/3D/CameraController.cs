using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //References
    Transform m_player;
    Transform m_target;
    Camera m_camera;
    Transform m_tilt;

    //Camera Variables
    [Header("Camera")]
    [SerializeField] float cameraHeight;
    [SerializeField] float cameraMaxTilt;
    [Range(0, 4)]
    [SerializeField] float cameraSpeed;
    const float DISTANCE = 5;
    float currentPan, currentTilt = 10;

    //LockTarget
    [Header("Lock Target")]
    [SerializeField] LayerMask targetMask;
    public bool isTargeting = false;

    //Collisions
    [Header("Collisions")]
    [SerializeField] LayerMask collisionMask;
    public bool collisionDebugg;
    float collisionCushion = 0.35f;
    float adjustedDistance;
    Ray camRay;
    RaycastHit camRayHit;

    //SmoothLerp
    float timeCount = 0.0f;
    float speedRotation = 0.8f;

    private void Start()
    {
        m_player = GameObject.FindGameObjectWithTag("Player").transform;
        m_tilt = transform.Find("Tilt").transform;
        m_camera = GetComponentInChildren<Camera>();

        Cursor.lockState = CursorLockMode.Locked;

        transform.position = m_player.position + Vector3.up * cameraHeight;
        transform.rotation = m_player.rotation;

        m_tilt.eulerAngles = new Vector3(currentTilt, transform.eulerAngles.y, transform.eulerAngles.z);
        m_camera.transform.position += m_tilt.forward * -DISTANCE;
    }

    private void Update()
    {
        CameraCollisions();

        if (!isTargeting)
        {
            currentPan += Input.GetAxis("Mouse X") * cameraSpeed;

            currentTilt -= Input.GetAxis("Mouse Y") * cameraSpeed;
            currentTilt = Mathf.Clamp(currentTilt, -cameraMaxTilt, cameraMaxTilt);
        }
        else
        {
            currentPan = transform.eulerAngles.y;
            currentTilt = transform.eulerAngles.x;

            m_tilt.eulerAngles = new Vector3(currentTilt, transform.eulerAngles.y, transform.eulerAngles.z);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isTargeting)
            {
                isTargeting = false;
            }
            else
            {
                LockTarget();
            }
        }
    }

    void LateUpdate()
    {
        //Follow player
        transform.position = m_player.transform.position + Vector3.up * cameraHeight;

        if (!isTargeting)
        {
            //Rotate around Player
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, currentPan, transform.eulerAngles.z);
            m_tilt.eulerAngles = new Vector3(currentTilt, m_tilt.eulerAngles.y, m_tilt.eulerAngles.z);

            timeCount = 0;
        }
        else
        {
            Vector3 relativePos = m_target.position - transform.position;

            Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, timeCount * speedRotation);
            timeCount = timeCount + Time.deltaTime;
        }

        m_camera.transform.position = transform.position + m_tilt.forward * -adjustedDistance;
    }

    void CameraCollisions()
    {
        float camDistance = DISTANCE + collisionCushion;  //To avoid clips & smooth the camera collisions

        camRay.origin = transform.position;
        camRay.direction = -m_tilt.forward;               //Shoot at the opposit direction

        if (Physics.Raycast(camRay, out camRayHit, camDistance, collisionMask))
        {
            adjustedDistance = Vector3.Distance(camRay.origin, camRayHit.point) - collisionCushion;
        }
        else
        {
            adjustedDistance = DISTANCE;
        }


        if (collisionDebugg)
        {
            Debug.DrawLine(camRay.origin, camRay.origin + camRay.direction * camDistance, Color.cyan);
        }
    }

    void LockTarget()
    {
        RaycastHit hit;

        if (Physics.SphereCast(m_camera.transform.position, 0.01f, m_camera.transform.forward, out hit, 100, targetMask))
        {
            m_target = hit.transform;
            isTargeting = true;
        }
        else if (Physics.SphereCast(m_camera.transform.position, 0.5f, m_camera.transform.forward, out hit, 100, targetMask))
        {
            m_target = hit.transform;
            isTargeting = true;
        }
        else if (Physics.SphereCast(m_camera.transform.position, 1f, m_camera.transform.forward, out hit, 100, targetMask))
        {
            m_target = hit.transform;
            isTargeting = true;
        }
        else if (Physics.SphereCast(m_camera.transform.position, 2f, m_camera.transform.forward, out hit, 100, targetMask))
        {
            m_target = hit.transform;
            isTargeting = true;
        }
        else if (Physics.SphereCast(m_camera.transform.position, 3f, m_camera.transform.forward, out hit, 100, targetMask))
        {
            m_target = hit.transform;
            isTargeting = true;
        }
    }
}
