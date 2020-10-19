using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class GraphUpdater : MonoBehaviour
{
    private GridGraph targetGraph;

    private void Awake()
    {        
        InvokeRepeating("UpdateGraph", 1f, 0.125f);
    }

    public void UpdateGraph()
    {
        if(targetGraph == null)
        {
            targetGraph = AstarPath.active.data.gridGraph;
        }

        //var graphToScan = AstarPath.active.data.gridGraph;
        AstarPath.active.Scan(targetGraph);
    }
}
