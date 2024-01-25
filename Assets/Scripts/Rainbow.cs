using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rainbow : MonoBehaviour
{
    private float timer = 5;
    void Start()
    {
        
    }

    void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, new Vector2(0.15f, 0.15f), 1);
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            Destroy(transform.gameObject);
        }
    }
}
