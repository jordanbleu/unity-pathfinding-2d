using Assets.Source.AStar;
using UnityEngine;

namespace Assets.Source.Components.NavMesh
{
    public class NavigationMeshComponent : MonoBehaviour
    {
        // how many tiles going horizontally 
        [SerializeField]
        private int gridWidth = 20;

        // how many tiles going vertically
        [SerializeField]
        private int gridHeight = 15;

        // how many unity "units" each tile should be
        [SerializeField]
        private float tileSize = 0.5f;

        private Node[][] nodes;

        private void Start()
        {
            nodes = new Node[gridWidth][];
            GenerateNavigationMesh();
        }

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
        private bool TileContainsSolid(Vector2 center) 
        {
            //var collider = Physics2D.OverlapCollider()
            //    //(center, new Vector2(tileSize/2, tileSize/2), 0f);

            
            var collider = Physics2D.OverlapArea(new Vector2(center.x - (tileSize / 2), center.y - (tileSize / 2)),
                                                new Vector2(center.x + (tileSize / 2), center.y + (tileSize / 2)));
            return collider != null;
        }

        private void Update()
        {

        }

        private void OnDrawGizmosSelected()
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
