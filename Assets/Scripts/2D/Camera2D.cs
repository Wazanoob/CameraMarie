using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera2D : MonoBehaviour
{
    //References
    Transform m_player;
    SimpleMovement m_playerMovement;

    //Camera Variables
    const float DISTANCE = 8f;
    const float cameraHeight = 1.5f;
    float yAxis; //Player Y axis


    //SmoothLerp
    bool isRunningTriggered = false;
    float smoothLerp = 50f;

    // Start is called before the first frame update
    void Start()
    {
        m_player = GameObject.FindGameObjectWithTag("Player").transform;
        m_playerMovement = m_player.GetComponent<SimpleMovement>();

        transform.position = m_player.position + Vector3.right * -DISTANCE + Vector3.up * cameraHeight;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (m_playerMovement.isRunning && m_playerMovement.isGrounded)
        {
            if (!isRunningTriggered)
            {
                smoothLerp = 0f;
                isRunningTriggered = true;
            }

            if (smoothLerp <= 20f)
            {
                smoothLerp += 7.5f * Time.deltaTime;
                smoothLerp = Mathf.Clamp(smoothLerp, 0f, 20f);
            }
        }
        else if(!m_playerMovement.isRunning && m_playerMovement.isGrounded)
        {
            smoothLerp = 20f;
            isRunningTriggered = false;
        }

        if (!m_playerMovement.isGrounded)
        {
            smoothLerp = 8f;

            if (yAxis <= m_player.position.y)
            {
                yAxis = m_player.position.y;
            }
     
            Vector3 nextPos = new Vector3(m_player.position.x -DISTANCE, yAxis + cameraHeight, m_player.position.z);
            transform.position = Vector3.Lerp(transform.position, nextPos, smoothLerp * Time.deltaTime);
        }
        else
        {
            Vector3 nextPos = m_player.position + Vector3.right * -DISTANCE + Vector3.up * cameraHeight;
            transform.position = Vector3.Lerp(transform.position, nextPos, smoothLerp * Time.deltaTime);
        }


    }
}
