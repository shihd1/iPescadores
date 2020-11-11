using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotator : MonoBehaviour
{
    public Transform PlayerTransform;
    public Camera x;
    private Vector3 _cameraOffset;
    public float SmoothFactor = 0.5f;
    public bool canRotate = true;
    public float RotationSpeed = 5.0f;
    public bool onMouseDrag = false;

    // Start is called before the first frame update
    void Start()
    {
        _cameraOffset = x.transform.position - PlayerTransform.position;
    }

    private void LateUpdate()
    {
        //Debug.Log(onMouseDrag);
        if (onMouseDrag)
        {
            Quaternion canTurnAngle = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * RotationSpeed, Vector3.up);
            _cameraOffset = canTurnAngle * _cameraOffset;
        }
        Vector3 newPos = PlayerTransform.position + _cameraOffset;
        x.transform.position = Vector3.Slerp(x.transform.position, newPos, SmoothFactor);
        if (canRotate)
        {
            x.transform.LookAt(PlayerTransform);
        }
    }

    private void OnMouseDown()
    {
        //Debug.Log("MouseDown");
        onMouseDrag = true;
    }

    private void OnMouseUp()
    {
        //Debug.Log("MouseUp");
        onMouseDrag = false;
    }
}
