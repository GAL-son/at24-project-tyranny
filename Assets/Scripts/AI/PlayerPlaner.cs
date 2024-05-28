using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Planer))]
public class PlayerPlaner : MonoBehaviour
{
    public LayerMask mouse;

    private TurnController turnController = null;
    private EnviromentController enviromentController = null;
    private Planer planer = null;
    private Vector2 mousePos;
    // Start is called before the first frame update
    void Start()
    {
        turnController = TurnController.Instance;
        enviromentController = EnviromentController.Instance;
        planer = gameObject.GetComponent<Planer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (turnController.isStagePlanning())
        {
            UpdatePlanedAction();
        }
    }

    public void UpdateMousePosition(InputAction.CallbackContext context)
    {
        mousePos = context.ReadValue<Vector2>();
    }

    public void SaveAction(InputAction.CallbackContext context)
    {
        //nextAction != null && 
        if (context.started && turnController.isStagePlanning())
        {
            planer.SaveAction();
        }
    }

    public void UpdatePlanedAction()
    {
        GameObject pointed = GetPointedGameObject();
        if (pointed != null)
        {
            Vector3Int targetCell = enviromentController.worldGrid.WorldToCell(pointed.transform.position);
            // If pointing at waklable enviroment
            EnviromentTile tile = pointed.GetComponentInParent<EnviromentTile>();
            if (tile != null && tile.isWalkable)
            {
                planer.PlanMove(targetCell);
            }
            else
            {
                planer.ClearPlanedAction();
                // If Targtet is somewhere far
                /*if (targetCell != actions.Last().ActionTarget)
                {
                    // Move to that cell
                    PlanMove(targetCell);
                    // And then
                }*/
                // Add new Action
                // If pointing at enemy

                // if pointing at interactabe

                // if pointing at item
            }
        }
    }

    private GameObject GetPointedGameObject()
    {
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, mouse))
        {
            return hit.transform.gameObject;
        }

        return null;
    }



}
