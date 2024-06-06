using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class CameraController : MonoBehaviour
{
    public float moveSpeed = 10f;
    public Vector3 moveBoundOffset = new Vector3(5f, 5f, 5f);

    public float rotationIncrement = 45.0f;
    public bool invertRotation = false;

    public float zoomIncremet = 1.0f;
    public float zoomInLimit = 5.0f;
     public float zoomOutLimit = 25.0f;
    public float defaultZoom = 10.0f;
    public bool invertZoom = false;

    public Tilemap enviroment = null;

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

    private Vector3 offset;

    private void Start()
    {
        xRotation = transform.rotation.eulerAngles.x;     

        GetComponentInChildren<Camera>().orthographicSize = defaultZoom;
        offset = GetComponentInChildren<Camera>().transform.position;
    }

    private void LateUpdate()
    {
        if (isMovingMouse)
        {
            moveDirection = convertInputToMove(-mouseDelta, scaleMovementWithZoom(GetComponentInChildren<Camera>().orthographicSize));
        }
        if (isMovingKeyboard || isMovingMouse)
        {
            Vector3 newPosition = (transform.position + moveDirection);
            if (enviroment == null)
            {
                transform.position = newPosition;
            }
            else
            {
                transform.position = newPosition;
            }
        }

        if (canRotate && isRotating)
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

        rotationDirection = (invertRotation ? 1 : -1) * context.ReadValue<float>();

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
        Vector2 transformendInput = Quaternion.Euler(0f, 0f, -transform.rotation.eulerAngles.y) * (input * moveScale * Time.deltaTime);

        return new Vector3(transformendInput.x, 0.0f, transformendInput.y);
    }

    private float scaleMovementWithZoom(float zoom)
    {
        // TODO: Figure out how to get screen scale;
        return zoom * 0.5f;
    }

    public void moveTo(Vector3 position)
    {
        position.y = 0;
        transform.position = position;
    }
}
