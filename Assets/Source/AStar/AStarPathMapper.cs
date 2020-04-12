using Assets.Source.Components.NavMesh;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Source.AStar
{
    public class AStarPathMapper
    {

        private NavigationMeshComponent navigationMesh;
        
        public AStarPathMapper(NavigationMeshComponent navMesh) 
        {
            this.navigationMesh = navMesh;
        }

        //public List<Node> FindPath(Vector2 startPosition, Vector2 targetPosition)  
        //{
            
        //}

    }
}
