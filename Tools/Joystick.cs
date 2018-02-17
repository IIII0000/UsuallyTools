using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


namespace Tools.Helper
{
    public class Joystick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public event Action<Vector3> JoystickHandler;
        //摇杆滑动范围
        public int joystickRange = 100;
        //是否允许X轴方向
        private bool useAxisX = true;
        //是否允许Y轴方向
        private bool useAxisY = true;
        //摇杆背景
        public RectTransform joystickBackground;
        //摇杆按钮
        public RectTransform joystickController;
        //摇杆状态
        private bool joystickState = false;
        //触摸ID
        private int touchId;
        //摇杆默认位置
        private Vector3 joystickDefaultPosition;
        //摇杆初始位置
        private Vector3 joystickStartPostion;
        //背景偏移位置
        private Vector3 backgroundOffsetPosition;
        //摇杆更新位置
        private Vector3 joystickNewPosition;
        //摇杆当前位置
        private Vector3 joystickCurrentPostion;
        private void Start()
        {
            backgroundOffsetPosition = joystickBackground.position - joystickController.position;
            joystickDefaultPosition = joystickController.position;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (joystickState)
            {
                return;
            }
            joystickState = true;
            touchId = eventData.pointerId;
            joystickController.position = eventData.position;
            joystickBackground.position = backgroundOffsetPosition + joystickController.position;
            joystickStartPostion = joystickController.position;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (!joystickState || eventData.pointerId != touchId)
            {
                return;
            }

            joystickState = false;
            touchId = -1;
            joystickController.position = joystickDefaultPosition;
            joystickBackground.position = backgroundOffsetPosition + joystickController.position;

        }

        private void Update()
        {
            if (!joystickState)
            {
                return;
            }
            //平台区分
            joystickNewPosition = Vector3.zero;
            joystickCurrentPostion = Input.mousePosition;
            if (Application.isMobilePlatform)
            {
                joystickCurrentPostion = Input.touches[touchId].position;
            }
            //更新摇杆位置
            joystickCurrentPostion -= joystickStartPostion;
            joystickCurrentPostion = Vector3.ClampMagnitude(joystickCurrentPostion, joystickRange);
            if (useAxisX)
            {
                joystickNewPosition.x = joystickCurrentPostion.x;
            }
            if (useAxisY)
            {
                joystickNewPosition.y = joystickCurrentPostion.y;
            }
            joystickController.position = joystickNewPosition + joystickStartPostion;

            var delta = joystickStartPostion - joystickController.position;
            delta /= joystickRange;
            //更新轴数据
            JoystickHandler?.Invoke(delta);
        }
    }
}
