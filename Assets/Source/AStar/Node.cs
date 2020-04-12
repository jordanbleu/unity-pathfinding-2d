using UnityEngine;

namespace Assets.Source.AStar
{
    public class Node
    {
        public Node(Vector2 center, bool isSolid = false) {
            Center = center;
            IsSolid = isSolid;
        }


        public Vector2 Center { get; }

        public bool IsSolid { get; set; }

    }
}
