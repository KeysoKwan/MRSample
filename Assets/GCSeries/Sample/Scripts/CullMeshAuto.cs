using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GCSeries;

namespace GCSeries
{
    /// <summary>
    /// 生成一个方形的遮挡面,给FView使用
    /// </summary>
    public class CullMeshAuto : MonoBehaviour
    {
        /// <summary>
        /// 宽
        /// </summary>
        public float width = 20;

        /// <summary>
        /// 高
        /// </summary>
        public float height = 20;

        /// <summary>
        /// 深
        /// </summary>
        public float deep = 40;

        /// <summary>
        /// 遮挡面的mesh
        /// </summary>
        public Mesh mesh;

        void Start()
        {
            if (mesh == null)
            {
                mesh = new Mesh();
                //mesh.vertices = new Vector3[] { new Vector3(-width, -height, 0), new Vector3(-width, height, 0),
                //                         new Vector3(width, -height, 0), new Vector3(width, height, 0),
                //                         new Vector3(-FCore.screenWidth/2, 0, 0),new Vector3(-FCore.screenWidth/2, FCore.screenHeight, 0),
                //                         new Vector3(FCore.screenWidth/2, 0, 0),new Vector3(FCore.screenWidth/2, FCore.screenHeight, 0),
                //                         new Vector3(-width, -height, -deep), new Vector3(-width, height, -deep),
                //                         new Vector3(width, -height, -deep), new Vector3(width, height, -deep)};
                DelaySetting();
                //SetMeshVertices();
                mesh.triangles = new int[] {//正面
                                     0,1,5,
                                     0,5,4,
                                     0,4,6,
                                     0,6,2,
                                     1,3,5,
                                     5,3,7,
                                     6,7,3,
                                     6,3,2,
                                     //左侧右侧
                                     8,9,1,
                                     8,1,0,
                                     2,3,11,
                                     2,11,10,
                                     //顶上,底下
                                     1,9,11,
                                     1,11,3,
                                     0,2,8,
                                     8,2,10};
            }
            MeshFilter mf = GetComponent<MeshFilter>();
            if (mf == null)
            {
                mf = gameObject.AddComponent<MeshFilter>();
            }
            mf.mesh = mesh;
            MeshRenderer mr = GetComponent<MeshRenderer>();
            if (mr == null)
            {
                mr = gameObject.AddComponent<MeshRenderer>();
            }
            Shader shaderHide = Shader.Find("Custom/Hide");
            if (shaderHide == null)
            {
                //Log.Instance.Output(Log.Level.ERROR, "CullMeshAuto.Start", "找不到FView的shader->Custom/Hide");
            }
            mr.material = new Material(shaderHide);
            mr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        }

        public void SetMeshVertices()
        {
            Invoke("DelaySetting", 0.02f);
        }
        bool flag = true;
        private void OnEnable()
        {
            if (!flag)
                DelaySetting();
            flag = false;
        }

        public float addHeight = 1;

        void DelaySetting()
        {
            mesh.vertices = new Vector3[] { new Vector3(-width, -height + addHeight, 0), new Vector3(-width, height + addHeight, 0),
                                        new Vector3(width, -height + addHeight, 0), new Vector3(width, height + addHeight, 0),
                                        new Vector3(-FCore.screenWidth/2, 0, 0),new Vector3(-FCore.screenWidth/2, FCore.screenHeight, 0),
                                        new Vector3(FCore.screenWidth/2, 0, 0),new Vector3(FCore.screenWidth/2, FCore.screenHeight, 0),
                                        new Vector3(-width, -height + addHeight, -deep), new Vector3(-width, height + addHeight, -deep),
                                        new Vector3(width, -height + addHeight, -deep), new Vector3(width, height + addHeight, -deep)};
        }

        float lastViewScale = 1;
        FAR fAR;

        float lastSlantAngle = 1;
        void Update()
        {
            if (lastViewScale != FCore.ViewerScale || lastSlantAngle != FCore.slantAngle)
            {
                lastViewScale = FCore.ViewerScale;
                lastSlantAngle = FCore.slantAngle;
                DelaySetting();

                if (fAR == null) fAR = FindObjectOfType<FAR>();
                fAR?.ResetFARPosFollowViewScale();
            }
        }
    }

}