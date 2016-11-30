using UnityEngine;
using System.Collections;

namespace Ver2
{
    public class BlockAreaV2 : MonoBehaviour
    {
        [SerializeField]
        int x = 0, y = 0, z = 0;
        [SerializeField]
        float blockSize = 1;

        public float BlockSize
        {
            get
            {
                return blockSize;
            }
        }

        public Transform wallParent;
        public Transform blockParent;

        public BlockV2 testObj;

        public BoxCollider wall_x_zero;
        public BoxCollider wall_x_max;
        public BoxCollider wall_y_zero;
        public BoxCollider wall_y_max;
        public BoxCollider wall_z_zero;
        public BoxCollider wall_z_max;

        private float wallThickness = 0.1f;

        private RaycastHit[] result=new RaycastHit[1];
        Vector3 normalX;
        Vector3 normalY;
        Vector3 normalZ;

        public Quaternion RedoQuaternoin
        {
            get
            {
                Quaternion rot = transform.rotation;
                rot = new Quaternion(-rot.x, -rot.y, -rot.z, rot.w);
                return rot;
            }
        }

        private void Start()
        {
            InitWalls();
            normalX = transform.rotation * Vector3.right;
            normalY = transform.rotation * Vector3.up;
            normalZ = transform.rotation * Vector3.forward;
        }

        private void InitWalls()
        {
            float xLong = x * blockSize;
            float yLong = y * blockSize;
            float zLong = z * blockSize;
            wall_x_zero = InitWall(new Vector3(wallThickness, yLong, zLong), new Vector3(BlockSize / 2 - wallThickness / 2, yLong / 2, zLong / 2));
            wall_x_max = InitWall(new Vector3(wallThickness, yLong, zLong), new Vector3(xLong + wallThickness / 2 - BlockSize / 2, yLong / 2, zLong / 2));
            wall_y_zero = InitWall(new Vector3(xLong, wallThickness, zLong), new Vector3(xLong / 2, BlockSize / 2 - wallThickness / 2, zLong / 2));
            wall_y_max = InitWall(new Vector3(xLong, wallThickness, zLong), new Vector3(xLong / 2, yLong + wallThickness / 2 - BlockSize / 2, zLong / 2));
            wall_z_zero = InitWall(new Vector3(xLong, yLong, wallThickness), new Vector3(xLong / 2, yLong / 2, BlockSize / 2 - wallThickness / 2));
            wall_z_max = InitWall(new Vector3(xLong, yLong, wallThickness), new Vector3(xLong / 2, yLong / 2, zLong + wallThickness / 2 - BlockSize / 2));
        }

        private BoxCollider InitWall(Vector3 size, Vector3 center)
        {
            GameObject obj = new GameObject();
            obj.transform.parent = wallParent;
            obj.transform.localPosition = center;
            obj.AddComponent<BlockWall>().area = this;
            BoxCollider col = obj.AddComponent<BoxCollider>();
            col.size = size;
            return col;
        }

        public void PlaceItem(GameObject prefab, int xNum, int yNum, int zNum)
        {
            if (xNum < 0 || xNum >= x || yNum < 0 || yNum >= y || zNum < 0 || zNum >= z)
            {
                return;
            }
            GameObject obj = Instantiate(prefab);
            obj.gameObject.SetActive(true);
            obj.transform.parent = blockParent;
            obj.transform.localPosition = new Vector3((xNum + 0.5f) * BlockSize, (yNum + 0.5f) * BlockSize, (zNum + 0.5f) * BlockSize);
            obj.transform.localEulerAngles = Vector3.zero;
            BlockV2 block = obj.GetComponent<BlockV2>();
            block.SetXYZ(xNum, yNum, zNum);
            block.area = this;
            GetArroundBlock(block, block.transform.position);
        }

        public void GetArroundBlock(BlockV2 block ,Vector3 blockPos)
        {
            block.x_add = RayCastSide(blockPos, normalX);
            if (block.x_add != null)
            {
                block.x_add.x_sub = block;
            }
            block.x_sub = RayCastSide(blockPos, -normalX);
            if (block.x_sub != null)
            {
                block.x_sub.x_add = block;
            }
            block.y_add = RayCastSide(blockPos, normalY);
            if (block.y_add != null)
            {
                block.y_add.y_sub = block;
            }
            block.y_sub = RayCastSide(blockPos, -normalY);
            if (block.y_sub != null)
            {
                block.y_sub.y_add = block;
            }
            block.z_add = RayCastSide(blockPos, normalZ);
            if (block.z_add != null)
            {
                block.z_add.z_sub = block;
            }
            block.z_sub = RayCastSide(blockPos, -normalZ);
            if (block.z_sub != null)
            {
                block.z_sub.z_add = block;
            }
        }

        public BlockV2 RayCastSide(Vector3 pos, Vector3 dir)
        {
            result = new RaycastHit[1];
            Physics.RaycastNonAlloc(pos, dir, result, blockSize);

            if (result[0].transform == null)
            {
                return null;
            }

            Debug.DrawLine(pos, result[0].point);
            BlockV2 block = result[0].transform.GetComponent<BlockV2>();
            return block;
        }

        private void Update()
        {
            if (testObj != null)
            {
                GetArroundBlock(testObj, testObj.transform.position);
            }
        }
    }    
}