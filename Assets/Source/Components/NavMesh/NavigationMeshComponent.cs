using Assets.Source.AStar;
using UnityEngine;

namespace Assets.Source.Components.NavMesh
{
    public class NavigationMeshComponent : MonoBehaviour
    {
        [Tooltip("How many tiles going horizontally")]
        [SerializeField]
        private int gridWidth = 20;

        [Tooltip("How many tiles going vertically")]
        [SerializeField]
        private int gridHeight = 15;

        [Tooltip("The size of each tile in units.  Should be large enough for a pathfinder to pass through without clipping the edge of corners.")]
        [SerializeField]
        private float tileSize = 0.5f;

        [Tooltip("Recalculates the entire navigation mesh every frame.  Leave this on if things in your level move around, " +
            "or set to false if the level is static for a performance boost.")]
        [SerializeField]
        private bool syncEveryFrame = true;

        private Node[][] nodes;

        private void Start()
        {
            nodes = new Node[gridWidth][];
            GenerateNavigationMesh();
        }

        // Generate the initial nav mesh
        private void GenerateNavigationMesh()
        {
            for (var ix = 0; ix < gridWidth; ix++)
            {
                nodes[ix] = new Node[gridHeight];
                for (var iy = 0; iy < gridHeight; iy++)
                {
                    var center = new Vector2(x: (ix  * tileSize) + transform.position.x, y: (iy * tileSize) + transform.position.y);
                    var isSolid = TileContainsSolid(center);

                    nodes[ix][iy] = new Node(center, isSolid);
                }
            }            
        }

        // Returns true if any solid exists in a radius of 'tileSize' around the specified center position
        private bool TileContainsSolid(Vector2 center) => Physics2D.OverlapArea(new Vector2(center.x - (tileSize / 2), center.y - (tileSize / 2)),
                                                                                new Vector2(center.x + (tileSize / 2), center.y + (tileSize / 2))) !=null;   
        

        private void Update()
        {
            if (syncEveryFrame)
            {
                UpdateTiles();
            }
        }

        // For each tile, update its "isSolid" status.
        private void UpdateTiles() 
        {
            foreach (var nodeRow in nodes) {
                foreach (var node in nodeRow)
                {
                    node.IsSolid = TileContainsSolid(node.Center);
                }
            }
        }

        // todo: change to this line because otherwise that grid will get annoying 
        //private void OnDrawGizmosSelected()
        private void OnDrawGizmos()
        {
            if (nodes != null)
            {
                for (var ix = 0; ix < nodes.Length; ix++)
                {
                    for (var iy = 0; iy < nodes[0].Length; iy++) 
                    {
                        Node node = nodes[ix][iy];

                        if (node.IsSolid)
                        {
                            Gizmos.color = Color.red;
                        }
                        else 
                        {
                            Gizmos.color = Color.grey;
                        }

                        Gizmos.DrawWireCube(node.Center, new Vector2(tileSize*0.95f,tileSize*0.95f));
                    }                
                }
            }
        }
    }
}
