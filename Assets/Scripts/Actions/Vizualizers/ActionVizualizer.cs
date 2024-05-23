using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ActionVizualizer : MonoBehaviour
{
    [Header("Actions Visualizations")]
    public PathVisualization moveActionVizualization;

    private List<GameObject> vizualizations = new();
    private GameObject currentVizualization = null;
    private Action actionMemory = null;

    public void RerenderVisualizations(List<Action> actions)
    {
        clearVizualizations();
        foreach (Action action in actions)
        {
            vizualizations.Add(RenderVizualization(action));
        }

    }

    private GameObject RenderVizualization(Action action)
    {
        if (action is MoveAction)
        {
            MoveAction nextMove = (MoveAction)action;
            PathVisualization viz = Instantiate(moveActionVizualization, this.transform);
            viz.updatePath(nextMove.Path);
            return viz.gameObject;
        }

        return null;
    }

    public void clearVizualizations()
    {
        foreach (GameObject viz in vizualizations)
        {
            Destroy(viz);
        }

        vizualizations.Clear();
    }


    public void UpdateCurrentVizualization(Action nextAction)
    {        
        if (nextAction == null)
        {
            Destroy(currentVizualization);
            currentVizualization = null;
            return;
        }

        if (currentVizualization == null)
        {
            currentVizualization = RenderVizualization(nextAction);
            return;
        }
                
        bool actionUpdated = false;

        if (actionMemory != null)
        {
            var t = actionMemory.GetType();
            var u = nextAction.GetType();
            if (t.IsAssignableFrom(u) || u.IsAssignableFrom(t))
            {
                UpdateCurrentVizualizationData(nextAction);
                actionUpdated = true;
            }
        }

        if(!actionUpdated)
        {
            ReplaceCurrentVizualization(nextAction);
        }

        actionMemory = nextAction;
    }

    private void UpdateCurrentVizualizationData(Action nextAction)
    {
        if (nextAction is MoveAction)
        {
            currentVizualization.GetComponent<PathVisualization>().updatePath(((MoveAction)nextAction).Path);
        }
    }

    private void ReplaceCurrentVizualization(Action nextAction)
    {
        if (currentVizualization != null)
        {
            Destroy(currentVizualization);
        }
        currentVizualization = RenderVizualization(nextAction);
    }

    private bool MustUpdateVizuationData()
    {
        return false;
    }
}
