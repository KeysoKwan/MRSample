using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace GCSeries
{

    /// <summary>
    /// 扩展的一体机3D跟踪相机
    /// </summary>
    public class Camera3D : GcCameraBase
    {
        /// <summary>
        /// 第二个相机，默认是右相机
        /// </summary>
        public Camera secondlyCamera;
        /// <summary>
        /// 瞳距
        /// </summary>
        public float eyeDistance = 0.06f;

        /// <summary>
        /// 重写重置投影矩阵方法
        /// </summary>
        public override void ResetCameraProjMat()
        {
            eyeDistance = Mathf.Clamp(eyeDistance, 0.025f, 0.08f);
            FCore.PupilDistance = eyeDistance;
            mainCamera.rect = new Rect(0, 0, 0.5f, 1);
            secondlyCamera.rect = new Rect(0.5f, 0, 0.5f, 1);
            mainCamera.transform.localPosition = new Vector3(eyeDistance / -2.0f, 0.0f, 0.0f);
            secondlyCamera.transform.localPosition = new Vector3(eyeDistance / 2.0f, 0.0f, 0.0f);
            base.ResetCameraProjMat();
            if (null != mrSystem && secondlyCamera != null)
                setCameraProjMat(secondlyCamera, FCore.glassPosition);


        }
        /// <summary>
        /// 记录上一个跟踪协程
        /// </summary>
        private Coroutine routineHandle = null;
        /// <summary>
        /// 跟踪相机位置协程
        /// </summary>
        /// <returns></returns>
        private IEnumerator<WaitForEndOfFrame> TrackCamPosition()
        {
            while (mainCamera.gameObject.activeSelf)
            {
                if (OCVData._data.GlassStatus == 1)
                {
                    transform.position = FCore.glassPosition;
                    transform.rotation = FCore.anchorRQuat * Quaternion.Euler(FCore.slantAngle, 0, 0);//让相机和屏幕平面垂直
                }
#if UNITY_EDITOR
                eyeDistance = Mathf.Clamp(eyeDistance, 0.02f, 0.08f);
#endif
                FCore.PupilDistance = eyeDistance;
                mainCamera.transform.localPosition = new Vector3(-FCore.PupilDistance / 2.0f, 0, 0) * FCore.ViewerScale;
                secondlyCamera.transform.localPosition = new Vector3(FCore.PupilDistance / 2.0f, 0, 0) * FCore.ViewerScale;
                mainCamera.transform.position = FCore.eyeLeftPosition;
                secondlyCamera.transform.position = FCore.eyeRightPosition;
                mainCamera.transform.rotation = FCore.anchorRQuat * Quaternion.Euler(FCore.slantAngle, 0, 0);
                secondlyCamera.transform.rotation = FCore.anchorRQuat * Quaternion.Euler(FCore.slantAngle, 0, 0);

                setCameraProjMat(mainCamera, FCore.eyeLeftPosition);
                setCameraProjMat(secondlyCamera, FCore.eyeRightPosition);
                yield return new WaitForEndOfFrame();
            }
        }

        /// <summary>
        /// 激活/隐藏相机
        /// </summary>
        /// <param name="activeAll">是否要激活</param>
        public override void ActiveCameras(bool activeAll)
        {
            gameObject.SetActive(true);
            mainCamera.gameObject.SetActive(activeAll);
            secondlyCamera.gameObject.SetActive(activeAll);
            if (activeAll)
            {
                routineHandle = StartCoroutine(TrackCamPosition());
            }
            else
            {
                if (routineHandle != null)
                    StopCoroutine(routineHandle);
            }

        }
    }

}