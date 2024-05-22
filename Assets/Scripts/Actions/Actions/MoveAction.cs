using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MoveAction : Action
{
    public const int COST_PER_CELL = 5;

    protected List<Vector3Int> path;

    public MoveAction(List<Vector3Int> path) : this(path, COST_PER_CELL) { }

    public MoveAction(List<Vector3Int> path, int costPerCell) : base(costPerCell * path.Count, path.Last())
    {
        this.path = path;
    }

    public List<Vector3Int> Path { get { return path; } }
}
