using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    private Vector3 _swipeStartPos = Vector3.zero;
    private Vector3 _swipeEndPos = Vector3.zero;
    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            _swipeStartPos = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            _swipeEndPos = Input.mousePosition;
            OnSwipe();
        }
#else
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                _swipeStartPos = touch.position;
            }
            
            if (touch.phase == TouchPhase.Ended)
            {
                _swipeEndPos = touch.position;
                OnSwipe();
            }
        }
#endif
    }

    private void OnSwipe()
    {
        Game.Instance.OnSwipe(_swipeStartPos,_swipeEndPos);
        _swipeStartPos = Vector3.zero;
        _swipeEndPos = Vector3.zero;
    }
}
