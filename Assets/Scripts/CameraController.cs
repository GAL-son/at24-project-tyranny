using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class CameraController : MonoBehaviour
{
    [SerializeField] public float moveSpeed = 10f;

    [SerializeField] public float rotationIncrement = 45.0f;
    [SerializeField] public bool invertRotation = false;

    [SerializeField] public float zoomIncremet = 1.0f;
    [SerializeField] public float zoomInLimit = 5.0f;
    [SerializeField] public float zoomOutLimit = 25.0f;
    [SerializeField] public float defaultZoom = 10.0f;
    [SerializeField] public bool invertZoom = false;

    private Vector2 mouseDelta;

    private bool isMovingKeyboard;
    private bool isMovingMouse;
    private Vector3 moveDirection;

    private bool isRotating = false;
    private bool canRotate = true;
    private float rotationDirection;

    private bool isZooming = false;
    private bool canZoom = true;
    private float zoomDirection;

    private float xRotation;

    private void Awake()
    {
        xRotation = transform.rotation.eulerAngles.x;
        GetComponentInChildren<Camera>().orthographicSize = defaultZoom;
    }
    private void LateUpdate()
    {
        if (isMovingMouse)
        {
            moveDirection = convertInputToMove(-mouseDelta, scaleMovementWithZoom(GetComponentInChildren<Camera>().orthographicSize));
        }
        if (isMovingKeyboard || isMovingMouse)
        {         
            transform.position += moveDirection;
        }

        if(canRotate && isRotating)
        {
            float rotation = rotationIncrement * rotationDirection;
            transform.rotation = Quaternion.Euler(xRotation, transform.rotation.eulerAngles.y + rotation, 0.0f);
            canRotate = false;
        }

        if (canZoom && isZooming)
        {
            float newZoom = GetComponentInChildren<Camera>().orthographicSize + (zoomDirection) * zoomIncremet;

            GetComponentInChildren<Camera>().orthographicSize = Mathf.Clamp(newZoom, zoomInLimit, zoomOutLimit);

            canZoom = false;
        }

    }

    public void OnMoveKeyboard(InputAction.CallbackContext context)
    {
        isMovingKeyboard = context.started || context.performed;

        moveDirection = convertInputToMove(context.ReadValue<Vector2>(), moveSpeed);
    }

    public void OnUpdateMouse(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }

    public void OnMoveMouse(InputAction.CallbackContext context) 
    {
        isMovingMouse = context.started || context.performed;        
    }

    public void OnRotate(InputAction.CallbackContext context)
    {
        isRotating = context.started || context.performed;
        canRotate = (!context.canceled);

        rotationDirection = (invertRotation ? 1 : -1 ) * context.ReadValue<float>();

    }

    public void OnZoom(InputAction.CallbackContext context)
    {
        isZooming = context.started || context.performed;
        canZoom = (!context.canceled);

        float scrollValue = (invertRotation ? 1 : -1) * context.ReadValue<Vector2>().y;
 
        zoomDirection = scrollValue == 0 ? 0 : Mathf.Sign(scrollValue);        
    }

    private Vector3 convertInputToMove(Vector2 input, float moveScale)
    {
        Debug.Log(moveScale);
        Vector2 transformendInput = Quaternion.Euler(0f, 0f, -transform.rotation.eulerAngles.y) * (input * moveScale * Time.deltaTime);

        return new Vector3(transformendInput.x, 0.0f, transformendInput.y);
    }

    private float scaleMovementWithZoom(float zoom)
    {
        // TODO: Figure out how to get screen scale;
        return zoom*0.5f;
    }
}
