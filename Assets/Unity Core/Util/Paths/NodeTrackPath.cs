using UnityEngine;
using System.Collections.Generic;

namespace Paths
{
    sealed public class NodeTrackPath : Path
    {
        private static int[] ADVANCE_TABLE = { 0, 1, 5, 2, 4, 3 };
        private static int[] ODD_MOVEMENT_TABLE = { 0x21, 0x20, 0x10, 0x01, 0x12, 0x22 };
        private static int[] EVEN_MOVEMENT_TABLE = { 0x21, 0x10, 0x00, 0x01, 0x02, 0x12 };

        public bool Complete { get; private set; }

        private readonly Dictionary<int, Vector3> nodeMap = new Dictionary<int, Vector3>();
        private int currentX;
        private int currentY;
        private int direction;

        public void AddNode(int x, int y, Vector3 position)
        {
            nodeMap[ComputeNodeKey(x, y)] = position;
        }

        public void Start(int x, int y, int direction)
        {
            currentX = x;
            currentY = y;
            this.direction = direction;
        }

        public Vector3 Peek()
        {
            return nodeMap[ComputeNodeKey(currentX, currentY)];
        }

        public Vector3 Advance(float distance)
        {
            var currentPosition = Peek();

            FindNextValidNode();

            return currentPosition;
        }

        private int ComputeNodeKey(int x, int y)
        {
            return (y << 4) | x;
        }

        private void FindNextValidNode()
        {
            var movementTable = ((currentY & 1) == 1) ? ODD_MOVEMENT_TABLE : EVEN_MOVEMENT_TABLE;
            var length = movementTable.Length;

            foreach (var offset in ADVANCE_TABLE)
            {
                var testDirection = (direction + offset) % length;
                var encodedMovement = movementTable[testDirection];
                var testX = currentX + ((encodedMovement >> 4) & 0x0f) - 1;
                var testY = currentY + (encodedMovement & 0x0f) - 1;

                if (nodeMap.ContainsKey(ComputeNodeKey(testX, testY)))
                {
                    currentX = testX;
                    currentY = testY;
                    direction = testDirection;

                    break;
                }
            }
        }
    }
}
