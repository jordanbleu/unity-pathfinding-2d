using Assets.Source.AStar;
using Assets.Source.Components.NavMesh;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Source.Components.Pathfinder
{
    public class PathfinderBehavior : MonoBehaviour
    {
        [SerializeField]
        private GameObject navigationMeshObject;

        [SerializeField]
        private LayerMask clickableLayers;

        [SerializeField]
        private bool canMoveDiagonally = false;
        
        private NavigationMeshComponent navigationMesh;
        private AStarPathMapper pathMapper;

        private List<Node> lastMappedPath;

        private Vector3 destination;

        private void Awake()
        {
            destination = new Vector3(0f, 0f, 0f);
            navigationMesh = navigationMeshObject?.GetComponent<NavigationMeshComponent>()
                ?? throw new UnityException("Navigation mesh is missing required Navigation Mesh Component");

            pathMapper = new AStarPathMapper(navigationMesh);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 mouse = Input.mousePosition;
                Ray castPoint = Camera.main.ScreenPointToRay(mouse);

                destination = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                
            }

            // Try moving solids around and checking out how the path updates
            lastMappedPath = pathMapper.FindPath(transform.position, destination, canMoveDiagonally);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;

            if (lastMappedPath != null && lastMappedPath.Any()) 
            {
                foreach (Node node in lastMappedPath) 
                {
                    Gizmos.DrawWireSphere(node.Center, 0.2f);            
                }          
            }
        }


    }
}
