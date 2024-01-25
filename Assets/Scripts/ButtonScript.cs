using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonScript : MonoBehaviour
{
    public Image thisImage;
    private float startSize;
    private Color32 startColor;

    public TextMeshProUGUI thisLevelNumberText;
    public LevelController levelController;
    public float thisLevelNumber;

    public List<Image> starSprites;

    void Start()
    {
        thisImage = transform.GetComponent<Image>();
        startSize = transform.localScale.x;
        startColor = thisImage.color;
        thisLevelNumberText.text = thisLevelNumber.ToString("0");
    }

    private void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(startSize, startSize, startSize),
            10 * Time.deltaTime);
        thisImage.color = Color.Lerp(thisImage.color, startColor, 10 * Time.deltaTime);
    }

    public void OnEnter()
    {
        transform.localScale = new Vector3(startSize + 0.1f, startSize + 0.1f, startSize + 0.1f);
        thisImage.color = new Color32(255, 255, 255, 130);
    }
}