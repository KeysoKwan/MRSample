using System;
using System.Collections;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;


namespace GCSeries
{
    /// <summary>
    /// 扩展的投屏3D相机
    /// </summary>
    public class ProjectCamera3D : GcCameraBase
    {
        /// <summary>
        /// 第二个相机，默认是右相机
        /// </summary>
        public Camera secondlyCamera;
        /// <summary>
        /// 瞳距
        /// </summary>
        public float eyeDistance = 0.06f;
        public override void ResetCameraProjMat()
        {
            mainCamera.transform.localPosition = new Vector3(eyeDistance / -2.0f, 0.0f, 0.0f);
            secondlyCamera.transform.localPosition = new Vector3(eyeDistance / 2.0f, 0.0f, 0.0f);
            base.ResetCameraProjMat();
            if (null != mrSystem && secondlyCamera != null)
                setCameraProjMat(secondlyCamera, FCore.glassPosition);
        }
        /// <summary>
        /// 激活/隐藏相机
        /// </summary>
        /// <param name="activeMain">主相机是否要激活</param>
        /// <param name="activeSecondly">次要相机是否要激活</param>
        public void ActiveCameras(bool activeMain, bool activeSecondly)
        {
            gameObject.SetActive(true);
            mainCamera.gameObject.SetActive(activeMain);
            secondlyCamera.gameObject.SetActive(activeSecondly);
        }
    }

}
