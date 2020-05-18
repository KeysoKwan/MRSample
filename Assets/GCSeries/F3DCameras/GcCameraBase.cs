using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

namespace GCSeries
{
    /// <summary>
    /// 屏幕2D相机基类
    /// </summary>
    public class GcCameraBase : MonoBehaviour
    {
        /// <summary>
        /// 系统原点
        /// </summary>
        /// 
        /// 
        public MRSystem mrSystem;
        /// <summary>
        /// 主相机
        /// </summary>
        public Camera mainCamera;
        /// <summary>
        /// 原始的投影矩阵
        /// </summary>
        Matrix4x4 originalProjection;
        void Start()
        {
            StartCoroutine(InitCamera());
        }
        /// <summary>
        /// 是否激活相机
        /// </summary>
        /// <param name="activeMain"></param>
        public virtual void ActiveCameras(bool activeMain)
        {
            mainCamera.gameObject.SetActive(activeMain);
        }
        /// <summary>
        /// 等待一帧结束重置相机
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerator InitCamera()
        {
            if (mainCamera == null) yield break;
            yield return new WaitForEndOfFrame();
            ResetCameraProjMat();
        }
        /// <summary>
        /// 设置主相机投影矩阵
        /// </summary>
        public virtual void ResetCameraProjMat()
        {
            originalProjection = mainCamera.projectionMatrix;
            if (mrSystem == null) mrSystem = FindObjectOfType<MRSystem>();
            if (null != mrSystem)
            {
                transform.rotation = mrSystem.transform.rotation * Quaternion.Euler(FCore.slantAngle, 0, 0);//让相机和屏幕平面垂直
                setCameraProjMat(mainCamera, FCore.glassPosition);
            }

        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary> 设置相机投影矩阵. </summary>
        ///
        /// <remarks> Dx, 2017/11/7. </remarks>
        ///
        /// <param name="cam"> The camera. </param>
        ///-------------------------------------------------------------------------------------------------
        protected void setCameraProjMat(Camera cam, Vector3 camPosition)
        {
            Matrix4x4 w2cam = cam.worldToCameraMatrix;
            float[] s4p = new float[] { FCore.screenPointLeftTop.x, FCore.screenPointLeftTop.y, FCore.screenPointLeftTop.z,
                                        FCore.screenPointLeftBotton.x, FCore.screenPointLeftBotton.y, FCore.screenPointLeftBotton.z,
                                        FCore.screenPointRightTop.x, FCore.screenPointRightTop.y, FCore.screenPointRightTop.z,
                                        FCore.screenPointRightBotton.x, FCore.screenPointRightBotton.y, FCore.screenPointRightBotton.z};
            Matrix4x4 p = originalProjection;
            matFun6(ref w2cam, s4p, ref p);
            cam.projectionMatrix = p;
        }


        [DllImport("FSCore")]
        static extern void matFun6(ref Matrix4x4 A, float[] B, ref Matrix4x4 result);
    }
}