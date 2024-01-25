using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
public class TouchController : MonoBehaviour
{
    public GeneralController general;
    public Vector2 touchPos;
    public Vector2 touchPosWorld;

    public bool blocked;
    public bool touchbegan;
    public bool touchcontinues;

    public void Update()
    {
        if (!general.paused)
        {
            touchInput();
        }
    }

   public void touchInput()
    {
        //float screenWidth = Screen.width;
        //float screenHeight = Screen.height;

        if (Application.isEditor)
        {
            if (Input.GetMouseButtonDown(0))
            {
                touchPos = Input.mousePosition;
                touchPosWorld = Camera.main.ScreenToWorldPoint(touchPos);
                startTouch();
            }
            if (Input.GetMouseButton(0))
            {
                touchPos = Input.mousePosition;
                touchPosWorld = Camera.main.ScreenToWorldPoint(touchPos);
                continueTouch();
            }
            if (Input.GetMouseButtonUp(0))
            {
                endTouch();
            }

        }

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:

                    touchPos = touch.position;
                    touchPosWorld = Camera.main.ScreenToWorldPoint(touchPos);
                    startTouch();
                    break;
                case TouchPhase.Moved:
                    touchPos = touch.position;
                    touchPosWorld = Camera.main.ScreenToWorldPoint(touchPos);
                    continueTouch();
                    break;
                case TouchPhase.Ended:
                    endTouch();

                    break;

            }
        }
    }

    public void startTouch()
    {
        if (!general.paused)
        {
            if (!blocked)
            {
                touchbegan = true;
                touchcontinues = false;


            }
        }
    }
    public void continueTouch()
    {
        if (!general.paused)
        {
            touchbegan = false;
            touchcontinues = true;

        }
    }

    public void endTouch()
    {
        if (!general.paused)
        {
            if (!general.ui.settingScreen.activeSelf)
            {
                blocked = false;
            }
            if (!blocked)
            {
                touchbegan = false;
                touchcontinues = false;
            }
        }
    }

    
}
