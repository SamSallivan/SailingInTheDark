using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointNode
{
    public GameObject obj;
    // public List<WaypointNode> adjacentNodes;
    // public WaypointNode()
    // {
    //     adjacentNodes = new List<WaypointNode>();
    // }
    public WaypointNode previous;
    public WaypointNode next;

}

public class WaypointHandler : MonoBehaviour
{
    public List<WaypointNode> waypoints;
    public GameObject boat;

    //get all waypoints, make a graph, connect them all to adjacent nodes (order in parent), then connect to boat
    private void Awake()
    {
        waypoints = new List<WaypointNode>();

        // WaypointNode boatNode = new WaypointNode();
        // boatNode.waypoint = boat;
        // waypoints.Add(boatNode);

        WaypointNode startNode = new WaypointNode();
        WaypointNode prevNode = startNode;
        WaypointNode currNode = startNode;

        //get all waypoints
        foreach (Transform childTransform in gameObject.transform)
        {
            waypoints.Add(currNode);
            currNode.obj = childTransform.gameObject;
            // boatNode.adjacentNodes.Add(currNode);
            // currNode.adjacentNodes.Add(boatNode);

            if (prevNode != currNode)
            {

                prevNode.next = currNode;
                currNode.previous = prevNode;
                prevNode = currNode;
                currNode = new WaypointNode();
            }
            else
            {
                currNode = new WaypointNode();
            }
        }

        //add last node to first previous
        startNode.previous = prevNode;
        prevNode.next = startNode;
    }

    private void PrintGraph()
    {
        // foreach (WaypointNode node in waypoints)
        // {
        //     print(node.waypoint.name + ", " + node.adjacentNodes.Count);
        //     foreach (WaypointNode n in node.adjacentNodes)
        //     {
        //         print("adj: " + n.waypoint.name);
        //     }
        // }
    }

}
