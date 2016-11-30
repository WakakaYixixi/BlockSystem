using UnityEngine;
using System.Collections;

namespace Ver2
{
    public class BlockClick : MonoBehaviour
    {
        public GameObject prefab;

        public enum BoxSide
        {
            x_add,
            x_sub,
            y_add,
            y_sub,
            z_add,
            z_sub
        }

        private void GetPos()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Physics.Raycast(ray ,out hit);
            if (hit.collider == null) return;
            BlockWall wall = hit.collider.GetComponent<BlockWall>();
            if (wall != null)
            {
                Quaternion trans = wall.area.RedoQuaternoin;

                Vector3 localPos = trans * (hit.point- wall.area.transform.position);
                int targetX = (int)(localPos.x / wall.area.BlockSize);
                int targetY = (int)(localPos.y / wall.area.BlockSize);
                int targetZ = (int)(localPos.z / wall.area.BlockSize);

                wall.area.PlaceItem(prefab,targetX,targetY,targetZ);
            }
            BlockV2 block = hit.collider.GetComponent<BlockV2>();
            if (block != null)
            {
                Vector3 dir = hit.point - block.transform.position;
                dir = block.area.RedoQuaternoin * dir;
                BoxSide side = GetSite(dir);
                int targetX = block.x;
                int targetY = block.y;
                int targetZ = block.z;
                switch (side)
                {
                    case BoxSide.x_add:targetX++ ;break;
                    case BoxSide.x_sub:targetX--; break;
                    case BoxSide.y_add:targetY++; break;
                    case BoxSide.y_sub:targetY--; break;
                    case BoxSide.z_add:targetZ++; break;
                    case BoxSide.z_sub:targetZ--; break;
                }
                block.area.PlaceItem(prefab, targetX, targetY, targetZ);
            }
        }

        private BoxSide GetSite(Vector3 dir)
        {
            float curTopNum=dir.x;
            BoxSide result;
            result = dir.x > 0 ? BoxSide.x_add : BoxSide.x_sub;
            if (Mathf.Abs(curTopNum) < Mathf.Abs(dir.y))
            {
                result = dir.y > 0 ? BoxSide.y_add : BoxSide.y_sub;
                curTopNum = dir.y;
            }
            if (Mathf.Abs(curTopNum) < Mathf.Abs(dir.z))
            {
                result = dir.z > 0 ? BoxSide.z_add : BoxSide.z_sub;
                curTopNum = dir.y;
            }
            return result;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                GetPos();
            }
        }
    }
}