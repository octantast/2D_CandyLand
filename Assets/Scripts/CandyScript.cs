using System.Collections.Generic;
using UnityEngine;

public class CandyScript : MonoBehaviour
{
    public GeneralController general;
    public TouchController touch;

    public Vector2 touchPos;
    public Vector2 touchPosWorld;

    public bool isDragging = false;

    private Rigidbody2D currentRigidbody;
    private Collider2D thiscollider;
    public Sprite thisSprite;
    public SpriteRenderer thisrenderer;
    public string thistag;

    private Vector3 offset;
    public GameObject parentobj;

    private List<string> mergeTags = new List<string> { "Pink", "Violet", "Orange", "Yellow", "Green", "Blue" };


    private void Start()
    {
        currentRigidbody = GetComponent<Rigidbody2D>();
        thistag = gameObject.tag;
        mergeTags.Remove(thistag);
        thisSprite = thisrenderer.sprite;
        thiscollider = GetComponent<Collider2D>();
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
                    if (parentobj == general.candyHolder)
                    {
                        transform.position = new Vector3(touch.touchPosWorld.x, touch.touchPosWorld.y, 0);
                        currentRigidbody.velocity = Vector2.zero;
                    }
                    else
                    {
                        parentobj.transform.position = new Vector3(touch.touchPosWorld.x, touch.touchPosWorld.y, 0) + offset;
                    }
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
                if (parentobj == general.candyHolder)
                {
                    transform.position = new Vector3(touch.touchPosWorld.x, touch.touchPosWorld.y, 0);
                   //currentRigidbody.velocity = Vector2.zero;
                }
                else
                {
                    offset = parentobj.transform.position - transform.position;
                    parentobj.transform.position = new Vector3(touch.touchPosWorld.x, touch.touchPosWorld.y, 0) + offset;
                }
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
    void Update()
    {
        if (!general.paused)
        {
            touchInput();
            if (currentRigidbody != null)
            {
                currentRigidbody.velocity = new Vector2(0, -general.ui.gravityIndex);
            }

        }
        else if (currentRigidbody != null)
        {
            currentRigidbody.velocity = Vector2.zero;
        }

    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Bottom"))
        {
            if (parentobj == general.candyHolder && transform.childCount == 0) // 1 candy
            {
                general.destroyingEffect(transform.gameObject, 1);
            }
            else // group of candies
            {
                if (transform.childCount != 0 && parentobj == general.candyHolder) // parent of group
                {
                    Debug.Log(transform.childCount + 1);
                    general.destroyingEffect(transform.gameObject, transform.childCount + 1);
                }
                else if (transform.childCount == 0)// group member
                {
                    //parentobj.transform.GetComponent<CandyScript>().enabled = false;
                    Debug.Log(parentobj.transform.childCount + 1);
                    general.destroyingEffect(transform.parent.gameObject, parentobj.transform.childCount + 1);
                }
            }
        }
        if (collision.gameObject.CompareTag("Bomb"))
        {
            // дестрой бомби і цукерок
            if (parentobj == general.candyHolder && transform.childCount == 0) // 1 candy
            {
                general.destroyingEffect2(transform.gameObject, collision.gameObject);
            }
            else // all candies
            {
                if (transform.childCount != 0 && parentobj == general.candyHolder)
                {
                    general.destroyingEffect2(transform.gameObject, collision.gameObject);
                }
                else if (transform.childCount == 0 && parentobj != general.candyHolder)
                {
                    general.destroyingEffect2(transform.parent.gameObject, collision.gameObject);
                }
                else if(transform.childCount == 0 && parentobj == general.candyHolder)
                {
                    general.puffeffect(collision.gameObject.transform.position);
                    Destroy(collision.gameObject);
                    general.allcandies.Remove(this);
                    Destroy(gameObject);
                }
            }

        }
        if (collision.gameObject.CompareTag("Special"))
        {
            if (parentobj == general.candyHolder && transform.childCount == 0) // 1 candy
            {
                Destroy(collision.gameObject);
                general.allcandies.Remove(this);
                Destroy(gameObject);
                //general.destroyingEffect2(transform.gameObject, collision.gameObject);
            }
            else // all candies
            {
                if (transform.childCount != 0 && parentobj == general.candyHolder)
                {
                    general.destroyingEffect2(transform.gameObject, collision.gameObject);
                }
                else if (transform.childCount == 0 && parentobj != general.candyHolder)
                {
                    general.destroyingEffect2(transform.parent.gameObject, collision.gameObject);
                }
                else if (transform.childCount == 0 && parentobj == general.candyHolder)
                {
                    general.puffeffect(collision.gameObject.transform.position);
                    Destroy(collision.gameObject);
                    general.allcandies.Remove(this);
                    Destroy(gameObject);
                }
            }

        }


        if (collision.transform.IsChildOf(transform) || transform.IsChildOf(collision.transform))
        {
        }
        else
        {
            if (collision.gameObject.CompareTag(gameObject.tag)) // same candy
            {
                // злипаються
                if (parentobj == general.candyHolder)
                {

                    general.ui.sounds[4].Play();
                    // currentRigidbody.isKinematic = true;
                    Destroy(currentRigidbody);
                    if (collision.transform.IsChildOf(general.candyHolder.transform))
                    {
                        transform.parent = collision.transform;
                        parentobj = transform.parent.gameObject;
                    }
                    else
                    {
                      
                        transform.parent = collision.gameObject.transform.parent.transform;
                        parentobj = transform.parent.gameObject;
                    }
                }

            }
        }

        foreach (string tag in mergeTags)
        {
            if (tag == collision.gameObject.tag)
            {
                Debug.Log("lose" + collision.gameObject.tag);
                general.ui.lose();
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("Rainbow") && thisrenderer.sprite != general.universalCandy)
        {
            if (parentobj == general.candyHolder && transform.childCount == 0) // 1 candy changes
            {
                //collision.gameObject.tag = "Untagged";
                   gameObject.tag = mergeTags[Random.Range(0, mergeTags.Count)];
                mergeTags.Remove(gameObject.tag);
                if(gameObject.tag == "Green")
                {
                    thisrenderer.sprite = general.allsprites[0];
                }
                else if (gameObject.tag == "Pink")
                {
                    thisrenderer.sprite = general.allsprites[1];
                }
                else if (gameObject.tag == "Violet")
                {
                    thisrenderer.sprite = general.allsprites[2];
                }

                else if (gameObject.tag == "Orange")
                {
                    thisrenderer.sprite = general.allsprites[3];
                }

                else if (gameObject.tag == "Yellow")
                {
                    thisrenderer.sprite = general.allsprites[4];
                }

                else if (gameObject.tag == "Blue")
                {
                    thisrenderer.sprite = general.allsprites[5];
                }
            }

        }
    }
}
