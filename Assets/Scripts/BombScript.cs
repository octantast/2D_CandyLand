using System.Collections.Generic;
using UnityEngine;

public class BombScript : MonoBehaviour
{
    public GeneralController general;
    public TouchController touch;
    private Rigidbody2D currentRigidbody;
    public bool isDragging = false;
    public Vector2 touchPos;
    public Vector2 touchPosWorld;
    private Collider2D thiscollider;
    private void Start()
    {
        currentRigidbody = GetComponent<Rigidbody2D>();
        thiscollider = GetComponent<Collider2D>();
    }
    void Update()
    {
        if (!general.paused)
        {
            touchInput();
                currentRigidbody.velocity = new Vector2(0, -general.ui.gravityIndex);
            

        }
        else
        {
            currentRigidbody.velocity = Vector2.zero;
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bottom"))
        {
            general.DestroyBomb(transform.gameObject);
        }

        if (collision.gameObject.CompareTag("Special"))
        {
            general.destroyingEffect2(transform.gameObject, collision.gameObject);
        }
    }

    public void touchInput()
    {
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
            if (!touch.blocked)
            {
                var touchRay = Camera.main.ScreenPointToRay(touch.touchPos);
                if (thiscollider.bounds.IntersectRay(touchRay))
                {
                    isDragging = true;
                    transform.position = new Vector3(touch.touchPosWorld.x, touch.touchPosWorld.y, 0);
                    currentRigidbody.velocity = Vector2.zero;
                }
            }
        }
    }
    public void continueTouch()
    {
        if (!general.paused && isDragging)
        {
            if (isDragging)
            {
                transform.position = new Vector3(touch.touchPosWorld.x, touch.touchPosWorld.y, 0);
            }
        }
    }

    public void endTouch()
    {
        if (!general.paused)
        {
            if (!general.ui.settingScreen.activeSelf)
            {
                touch.blocked = false;
            }
            if (!touch.blocked)
            {
                isDragging = false;
            }
        }
    }
}
