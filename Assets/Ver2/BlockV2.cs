using UnityEngine;
using System.Collections;

namespace Ver2
{
    public class BlockV2 : MonoBehaviour
    {
        public BlockAreaV2 area;
        public GameObject SetItem;

        public int x = 0, y = 0, z = 0;

        public BlockV2 x_add;
        public BlockV2 x_sub;
        public BlockV2 y_add;
        public BlockV2 y_sub;
        public BlockV2 z_add;
        public BlockV2 z_sub;

        public void SetXYZ(int inputX,int inputY,int inputZ)
        {
            x = inputX;
            y = inputY;
            z = inputZ;
        }
    }
}