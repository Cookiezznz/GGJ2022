using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class damagePopup : MonoBehaviour
{
    private float lifespan = 0.5f;
    private float moveYSpeed = 2;
    private Color textColor;

    private TextMeshPro popupText;

    public void Setup(int damageAmount, Color color)
    {
        popupText = GetComponent<TextMeshPro>();
        popupText.text = damageAmount.ToString();
        textColor = color;
        popupText.color = textColor;

    }

    private void Update()
    {
        transform.position += new Vector3(0, moveYSpeed) * Time.deltaTime;
        lifespan -= Time.deltaTime;
        if (lifespan < 0)
        {
            float disappearSpeed = 3f;
            textColor.a -= disappearSpeed * Time.deltaTime;
            popupText.color = textColor;
        }
        if (textColor.a == 0)
        {

        }
    }
}
