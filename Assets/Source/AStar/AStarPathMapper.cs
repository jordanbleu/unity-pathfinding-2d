using Assets.Source.Components.NavMesh;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Source.AStar
{
    public class AStarPathMapper
    {

        private NavigationMeshComponent navigationMesh;

        private Node[][] nodes;
        private int gridWidth;
        private int gridHeight;

        public AStarPathMapper(NavigationMeshComponent navMesh) 
        {
            this.navigationMesh = navMesh;
        }

        public List<Node> FindPath(Vector2 startPosition, Vector2 targetPosition)  
        {
            // Perform all pathing operations on a clone of currrent A* grid
            nodes = navigationMesh.CloneGrid();
            gridWidth = navigationMesh.GridWidth;
            gridHeight = navigationMesh.GridHeight;

            // Find the nodes closest to our destination positions
            // if these are off the grid, will return the edge of the grid as close as it can get
            var startNodePosition =  navigationMesh.FindNearestNodeIndex(startPosition);
            var targetNodePosition = navigationMesh.FindNearestNodeIndex(targetPosition);
            var startNode = nodes[startNodePosition.ix][startNodePosition.iy];
            var targetNode = nodes[targetNodePosition.ix][targetNodePosition.iy];

            // The "open list" is the list of nodes that we need to visit
            var openList = new List<Node>() { startNode };

            // The "closed set" is the list of nodes that we have visited already
            var closedList = new List<Node>();

            // Loop until we have no more nodes to visit
            while (openList.Count > 0) 
            {
                // Grab whatever the next node on the list is
                var currentNode = openList.First();

                // Iterate through the open list starting from the second element to figure out what node we want to visit next
                // If there is no other element, this whole thing is skipped
                foreach (var node in openList.Skip(1)) 
                {
                    // if this node appears to get us to our destination quicker, visit that boy first
                    if (node.FCost <= currentNode.FCost && node.HCost < currentNode.HCost)
                    {
                        currentNode = node;
                    }
                }

                // mark this node as visited, remove it from the open list and add it to the closed list
                openList.Remove(currentNode);
                closedList.Add(currentNode);

                // if we find our target node
                if (currentNode == targetNode)
                {
                    // Crawl backwards to retrieve the total path
                    return TraceFinalPath(startNode, targetNode);
                }


                foreach (Node neighbor in FindNeighborNodes(currentNode))
                {
                    // Ignore any solid or previously visited nodes
                    if (neighbor.IsSolid || closedList.Contains(neighbor)) 
                    {
                        continue;
                    }

                    // For this implementation of A*, the GCost is the linear distance between node indices 
                    // Add the distance between currentNode and this particular neighbor 
                    var moveCost = currentNode.GCost + GetDistance(currentNode, neighbor);

                    // Check if this neighbor node should be where we visit next
                    if (moveCost < neighbor.GCost || !openList.Contains(neighbor)) 
                    {
                        neighbor.GCost = moveCost;
                        neighbor.HCost = GetDistance(neighbor, targetNode);
                        neighbor.Parent = currentNode;

                        if (!openList.Contains(neighbor))
                        {
                            openList.Add(neighbor);
                        }
                    }
                }
            }

            // If we managed to get here, it means we have visited every node and could not find a path.
            // Return an empty list, and leave it up to the implementor to decide what to do 
            return new List<Node>();
        }

        // Find the linear distance between two indices in the node array
        private int GetDistance(Node node1, Node node2)
        {
            var x = Mathf.Abs(node1.XIndex - node2.XIndex));
            var y = Mathf.Abs(node2.YIndex - node2.YIndex));
            return x + y;
        }

        // Follow the parent nodes backwards from destination => start
        private List<Node> TraceFinalPath(Node startNode, Node targetNode)
        {
            var path = new List<Node>();

            var currentNode = targetNode;

            while (currentNode != startNode)
            {
                path.Add(currentNode);
                currentNode = currentNode.Parent;
            }

            // reverse the list so we are going from start to finish 
            path.Reverse();

            return path;
        }

        // Moderately sloppy way to grab the neighboring nodes. 
        // todo: allow diagonals
        private IEnumerable<Node> FindNeighborNodes(Node node)
        {
            List<Node> neighbors = new List<Node>();
            int checkX;
            int checkY;

            //Check the right side of the current node.
            checkX = node.XIndex + 1;
            checkY = node.YIndex;
            if (checkX >= 0 && checkX < gridWidth)//If the XPosition is in range of the array
            {
                if (checkY >= 0 && checkY < gridHeight)//If the YPosition is in range of the array
                {
                    neighbors.Add(nodes[checkX][checkY]);//Add the grid to the available neighbors list
                }
            }
            //Check the Left side of the current node.
            checkX = node.XIndex - 1;
            checkY = node.YIndex;
            if (checkX >= 0 && checkX < gridWidth)//If the XPosition is in range of the array
            {
                if (checkY >= 0 && checkY < gridHeight)//If the YPosition is in range of the array
                {
                    neighbors.Add(nodes[checkX][checkY]);//Add the grid to the available neighbors list
                }
            }
            //Check the Top side of the current node.
            checkX = node.XIndex;
            checkY = node.YIndex + 1;
            if (checkX >= 0 && checkX < gridWidth)//If the XPosition is in range of the array
            {
                if (checkY >= 0 && checkY < gridHeight)//If the YPosition is in range of the array
                {
                    neighbors.Add(nodes[checkX][checkY]);//Add the grid to the available neighbors list
                }
            }
            //Check the Bottom side of the current node.
            checkX = node.XIndex;
            checkY = node.YIndex - 1;
            if (checkX >= 0 && checkX < gridWidth)//If the XPosition is in range of the array
            {
                if (checkY >= 0 && checkY < gridHeight)//If the YPosition is in range of the array
                {
                    neighbors.Add(nodes[checkX][checkY]);//Add the grid to the available neighbors list
                }
            }

            return neighbors;//Return the neighbors list.

        }
    }
}
