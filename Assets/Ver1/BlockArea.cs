using UnityEngine;
using System.Collections;
using UnityEditor;

namespace Ver1
{
    public class BlockArea : MonoBehaviour
    {
        [SerializeField]
        Transform wallParent;
        [SerializeField]
        int X = 0, Y = 0, Z = 0;
        [SerializeField]
        float blockSize = 0;

        [SerializeField]
        Block[,,] blocks;

        public GameObject prefab;

        private void Start()
        {
            blocks = new Block[X, Y, Z];
            Profiler.BeginSample("create");
            //CreateBaseBlockArea();
            GameObject wall = Instantiate(prefab);
            wall.transform.parent = transform;
            wallParent = wall.transform;
            wall.transform.localPosition = Vector3.zero;
            Block[] wallBlocks = wall.GetComponentsInChildren<Block>();
            foreach (Block block in wallBlocks)
            {
                blocks[block.x, block.y, block.z] = block;
            }
            Profiler.EndSample();
        }

        private void CreateBaseBlockArea()
        {
            ClearBaseBlocks();
            for (int i = 0; i < X; i++)
            {
                for (int j = 0; j < Y; j++)
                {
                    for (int k = 0; k < Z; k++)
                    {
                        if (i == 0 || i == X - 1 || j == 0 || j == Y - 1 || k == 0 || k == Z - 1)
                        {
                            GameObject obj = new GameObject();
                            Block curBlock = obj.AddComponent<Block>();
                            BoxCollider col = obj.AddComponent<BoxCollider>();
                            col.size = new Vector3(blockSize, blockSize, blockSize);
                            obj.transform.parent = wallParent;
                            obj.transform.localPosition = new Vector3((i - 0.5f) * blockSize, (j - 0.5f) * blockSize, (k - 0.5f) * blockSize);
                            blocks[i, j, k] = curBlock;
                            curBlock.x = i;
                            curBlock.y = j;
                            curBlock.z = k;
                            curBlock.isWall = true;
                        }
                    }
                }
            }
        }

        private void ClearBaseBlocks()
        {
            Block[] blocks = GetComponentsInChildren<Block>();
            foreach (Block block in blocks)
            {
                DestroyImmediate(block.gameObject);
            }
        }

#if UNITY_EDITOR
        [CustomEditor(typeof(BlockArea))]
        public class BlockAreaEditor : Editor
        {
            BlockArea o;
            int x, y, z;
            private void OnEnable()
            {
                o = target as BlockArea;

                x = o.X;
                y = o.Y;
                z = o.Z;

                o.blocks = new Block[x, y, z];
            }

            public override void OnInspectorGUI()
            {
                base.OnInspectorGUI();
                if (GUILayout.Button("create"))
                {
                    o.CreateBaseBlockArea();
                }
                if (GUILayout.Button("clear"))
                {
                    o.ClearBaseBlocks();
                }
            }
        }

#endif
    }

}