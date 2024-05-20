using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionVizualizer : MonoBehaviour
{
    [Header("Actions Visualizations")]
    public PathVisualization moveActionVizualization;

    private List<GameObject> vizualizations = new();
    private GameObject currentVizualization = null;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void RerenderVisualizations(List<Action> actions)
    {
        Debug.Log("RERENDER");
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


    public void UpdateCurrentVizualization(Action nextAction, Action previousAction)
    {
        if (nextAction == null)
        {
            Destroy(currentVizualization);
            currentVizualization = null;
        }

        if (currentVizualization == null)
        {
            currentVizualization = RenderVizualization(nextAction);
            return;
        }

        var t = previousAction.GetType();
        var u = nextAction.GetType();

        if (t.IsAssignableFrom(u) || u.IsAssignableFrom(t))
        {
            if (nextAction is MoveAction)
            {
                currentVizualization.GetComponent<PathVisualization>().updatePath(((MoveAction)nextAction).Path);
            }
        }
        else
        {
            if (currentVizualization != null)
            {
                Destroy(currentVizualization);
            }
            currentVizualization = RenderVizualization(nextAction);
        }
    }
}
