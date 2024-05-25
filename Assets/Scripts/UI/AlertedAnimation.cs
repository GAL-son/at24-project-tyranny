using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AlertedAnimation : MonoBehaviour
{
    [Range(0f, 1f)]
    public float maximumHeightOffset;
    [Range(-1f, 0f)]
    public float minimumHeightOffset;
    public float heightChangeSpeed = 1;

    private float statringPosition;
    private bool movingUp = false;


    // Start is called before the first frame update
    void Start()
    {
        statringPosition = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        Rotate();
        UpdateHeight();
    }

    private void Rotate()
    {
        transform.Rotate(0, 45 * Time.deltaTime, 0);
    }

    private void UpdateHeight()
    {
        Vector3 currPos = transform.position;
        float currHeightOffset = currPos.y - statringPosition;

        if (currHeightOffset <= minimumHeightOffset)
        {
            movingUp = true;
        }
        else if (currHeightOffset >= maximumHeightOffset)
        {
            movingUp = false;
        }

        float updateValue = movingUp ? 1 : -1;
        updateValue *= heightChangeSpeed * Time.deltaTime;

        currPos.y += updateValue;
        transform.position = currPos;
    }

    public void Activate()
    {
        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
