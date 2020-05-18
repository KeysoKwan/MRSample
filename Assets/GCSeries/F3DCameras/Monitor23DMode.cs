using GCSeries;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GCSeries
{
    /// <summary>
    /// 监听2、3D模式
    /// 开启和关闭相应的模式
    /// </summary>
    public class Monitor23DMode : MonoBehaviour
    {
        /// <summary>
        /// 单例
        /// </summary>
        public static Monitor23DMode instance { get; private set; } = null;
        /// <summary>
        /// 默认眼镜格式为非2，3D模式
        /// 一开始进行状态设置
        /// </summary>
        int lastGlassStatus = 2;
        EventSystem eventSystem;
        StandaloneInputModule standaloneInputModule;
        [HideInInspector]
        public GCSeriesInputModule f3DSpaceInputModule;

        [HideInInspector]
        public bool is3D = false;

        private bool _is3D = true;

        /// <summary>
        /// 第一次进行检测
        /// </summary>
        bool flag = true;

        GameObject penRayObj;
        /// <summary>
        /// 触笔gameObject
        /// </summary>
        public GameObject PenRayObj
        {
            get
            {
                if (penRayObj == null) penRayObj = GameObject.FindObjectOfType<PenRay>()?.gameObject;
                return penRayObj;
            }
        }

        private Camera3D camera3D;
        public Camera3D refCamera3D
        {
            get { if (camera3D == null) camera3D = FindObjectOfType<Camera3D>(); return camera3D; }
        }

        private Camera2D camera2D;
        public Camera2D refCamera2D
        {
            get { if (camera2D == null) camera2D = FindObjectOfType<Camera2D>(); return camera2D; }
        }

        private void Awake()
        {
            instance = this;
            eventSystem = FindObjectOfType<EventSystem>();

            standaloneInputModule = eventSystem.GetComponent<StandaloneInputModule>();
            if (standaloneInputModule)
            {
                Destroy(standaloneInputModule);
            }

            f3DSpaceInputModule = eventSystem.GetComponent<GCSeriesInputModule>();
            if (f3DSpaceInputModule == null)
            {
                f3DSpaceInputModule = eventSystem.gameObject.AddComponent<GCSeriesInputModule>();
            }

            //默认先设置为2D模式
            SetCameraAccordingTo23DState(false);
        }

        void Update()
        {
            SwitchCameraBy23DState();
            Set23DUIModel();
        }


        /// <summary>
        /// 根据2/3D的状态
        /// 设置f3DSpaceInputModule visibale
        /// Cursor.visible
        /// 投屏状态
        /// </summary>
        void Set23DUIModel()
        {
            if (_is3D != is3D || flag) //默认第一次执行一次判断
            {
                flag = false;
                _is3D = is3D;

                PenRayObj?.SetActive(is3D);
                Cursor.visible = !is3D;
                SetCameraAccordingTo23DState(is3D);
            }
        }

        void SwitchCameraBy23DState()
        {
            if (OCVData._data.GlassStatus != lastGlassStatus)
            {
                if (OCVData._data.GlassStatus == 1)
                {
                    is3D = true;
                }
                else
                {
                    is3D = false;
                }
                lastGlassStatus = OCVData._data.GlassStatus;
            }
        }

        /// <summary>
        /// 设置2/3D相机使能
        /// </summary>
        /// <param name="is3D"></param>
        public void SetCamera23DState(bool is3D)
        {
            refCamera2D.ActiveCameras(!is3D);
            refCamera3D.ActiveCameras(is3D);
        }

        /// <summary>
        /// 设置2、3D相机的状态
        /// </summary>
        /// <param name="is3D"></param>
        void SetCameraAccordingTo23DState(bool is3D)
        {
            refCamera2D.ActiveCameras(!is3D);
            refCamera3D.ActiveCameras(is3D);
            if (is3D)
                FCore.SetScreen3DSelf();
            else
                FCore.SetScreen2DSelf();
        }
    }
}
