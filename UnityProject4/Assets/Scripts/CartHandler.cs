using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartHandler : MonoBehaviour
{
    private static readonly float PanSpeed = 20f;
    private static readonly float ZoomSpeedTouch = 0.1f;
    private static readonly float ZoomSpeedMouse = 20f;

    public float[] ZoomBounds;

    public Camera cam;

    private Vector3 lastPanPosition;
    private int panFingerId; // Touch mode only

    private bool wasZoomingLastFrame; // Touch mode only
    private Vector2[] lastZoomPositions; // Touch mode only

    public float sensitivity;

    public bool canPan;

    public CinemachineVirtualCamera vcam;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (canPan)
        {
            if (Input.touchSupported && Application.platform != RuntimePlatform.WebGLPlayer)
            {
                //HandleTouch();
            }
            else
            {
                HandleMouse();
            }
        }
        GetComponent<Cinemachine.CinemachineDollyCart>().m_Speed = (float) (GetComponent<Cinemachine.CinemachineDollyCart>().m_Speed * 0.1);
    }
    void HandleMouse()
    {
        // On mouse down, capture it's position.
        // Otherwise, if the mouse is still down, pan the camera.
        if (Input.GetMouseButtonDown(0))
        {
            lastPanPosition = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0))
        {
            PanCamera(Input.mousePosition);
        }

        // Check for scrolling to zoom the camera
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        ZoomCamera(scroll, ZoomSpeedMouse);
    }
    void PanCamera(Vector3 newPanPosition)
    {
        // Determine how much to move the camera
        Vector3 offset = cam.ScreenToViewportPoint(lastPanPosition - newPanPosition);
        Vector3 move = new Vector3(offset.x * PanSpeed, 0, offset.y * PanSpeed);
        
        // Perform the movement
        transform.Translate(move, Space.World);
        GetComponent<Cinemachine.CinemachineDollyCart>().m_Position+=(offset.x*sensitivity);
        GetComponent<Cinemachine.CinemachineDollyCart>().m_Speed += (offset.x*sensitivity);
        /*
        // Ensure the camera remains within bounds.
        Vector3 pos = transform.position;
        //pos.x = Mathf.Clamp(transform.position.x, BoundsX[0], BoundsX[1]);
        //pos.z = Mathf.Clamp(transform.position.z, BoundsZ[0], BoundsZ[1]);
        transform.position = pos;
        */

        // Cache the position
        lastPanPosition = newPanPosition;
    }

    void ZoomCamera(float offset, float speed)
    {
        if (offset == 0)
        {
            return;
        }
        vcam.GetCinemachineComponent<CinemachineTrackedDolly>().m_PathOffset = new Vector3(vcam.GetCinemachineComponent<CinemachineTrackedDolly>().m_PathOffset.x+offset * speed, 0, 0);
        //cam.fieldOfView = Mathf.Clamp(cam.fieldOfView - (offset * speed), ZoomBounds[0], ZoomBounds[1]);
    }
    public void setCanPan(bool status)
    {
        canPan = status;
    }
}
