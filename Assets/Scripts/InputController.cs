using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
            Down(Input.mousePosition);
        }

        if (Input.GetMouseButton(0))
        {
            Hold(Input.mousePosition);
        }

        if (Input.GetMouseButtonUp(0))
        {
            Up(Input.mousePosition);
        }
#else

        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                Down(touch.position);
            }

            if (touch.phase == TouchPhase.Moved)
            {
                Hold(touch.position);
            }

            if (touch.phase == TouchPhase.Ended)
            {
                Up(touch.position);
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

    public void Down(Vector3 position)
    {
        if (Game.Instance.GameState == GameState.WaitingForInput && Game.Instance.IsMouseOverGrid(position))
        {
            if (Game.Instance.IsDestroyBoosterOn() && !Game.Instance.IsColorBoostOn())
            {
                Game.Instance.DestroyBooster(position);
            }
            else if (!Game.Instance.IsDestroyBoosterOn() && Game.Instance.IsColorBoostOn())
            {
                Game.Instance.ChangeColorBooster(position);
            }
            else
            {
                Game.Instance.StartTrail(position);
                _isTrailOn = true;
                _swipeStartPos = position; 
            }
        }
    }
    public void Hold(Vector3 position)
    {
        if (_isTrailOn && Game.Instance.GameState == GameState.WaitingForInput)
        {
            Game.Instance.UpdateTrail(position);
        }
    }
    public void Up(Vector3 position)
    {
        if (_isTrailOn)
        {
            _isTrailOn = false;
            _swipeEndPos = position;
            OnSwipe();
            Game.Instance.EndTrail();
        }
    }
}
