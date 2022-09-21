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
        if (Input.GetMouseButtonDown(0))
        {
            _swipeStartPos = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            _swipeEndPos = Input.mousePosition;
            Game.Instance.OnSwipe(_swipeStartPos,_swipeEndPos);
            _swipeStartPos = Vector3.zero;
            _swipeEndPos = Vector3.zero;
        }
    }
}
