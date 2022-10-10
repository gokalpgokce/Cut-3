using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    private Vector3 _swipeStartPos = Vector3.zero;
    private Vector3 _swipeEndPos = Vector3.zero;
    private bool _isTrailOn;
    private void Update()
    {
#if UNITY_EDITOR

        if (Input.GetMouseButtonDown(0))
        {
            Down();
        }

        if (Input.GetMouseButton(0))
        {
            Hold();
        }

        if (Input.GetMouseButtonUp(0))
        {
            Up();
        }
        
        // if (Input.GetMouseButtonDown(0))
        // {
        //     _swipeStartPos = Input.mousePosition;
        //     if (Game.Instance.GameState == GameState.WaitingForInput)
        //     {
        //         Game.Instance.StartTrail();
        //         _isTrailOn = true;
        //     }
        // }
        //
        // if (_isTrailOn && Game.Instance.GameState == GameState.WaitingForInput)
        // {
        //     Game.Instance.UpdateTrail();
        // }
        //
        // if (Input.GetMouseButtonUp(0))
        // {
        //     _isTrailOn = false;
        //     _swipeEndPos = Input.mousePosition;
        //     OnSwipe();
        //     Game.Instance.EndTrail();
        // }
#else

        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                Down();
            }

            if (touch.phase == TouchPhase.Moved)
            {
                Hold();
            }

            if (touch.phase == TouchPhase.Ended)
            {
                Up();
            }
        }

        // if (Input.touchCount == 1)
        // {
        //     Touch touch = Input.GetTouch(0);
        //     if (touch.phase == TouchPhase.Began)
        //     {
        //         _swipeStartPos = touch.position;
        //     }
        //     
        //     if (touch.phase == TouchPhase.Ended)
        //     {
        //         _swipeEndPos = touch.position;
        //         OnSwipe();
        //     }
        // }
#endif
    }

    private void OnSwipe()
    {
        Game.Instance.OnSwipe(_swipeStartPos,_swipeEndPos);
        _swipeStartPos = Vector3.zero;
        _swipeEndPos = Vector3.zero;
    }

    public void Down()
    {
        _swipeStartPos = Input.mousePosition;
        if (Game.Instance.GameState == GameState.WaitingForInput)
        {
            Game.Instance.StartTrail();
            _isTrailOn = true;
        }
    }
    public void Hold()
    {
        if (_isTrailOn && Game.Instance.GameState == GameState.WaitingForInput)
        {
            Game.Instance.UpdateTrail();
        }
    }
    public void Up()
    {
        _isTrailOn = false;
        _swipeEndPos = Input.mousePosition;
        OnSwipe();
        Game.Instance.EndTrail();
    }
}
