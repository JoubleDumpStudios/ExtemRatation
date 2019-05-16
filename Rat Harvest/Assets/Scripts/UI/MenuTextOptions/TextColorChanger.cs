using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextColorChanger : MonoBehaviour
{
    private Text buttonText;
    private Color originalColor;
    // Start is called before the first frame update
    void Start()
    {
        buttonText = GetComponentInChildren<Text>();
        originalColor = buttonText.color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PointerEnter()
    {
        buttonText.color = Color.yellow;
    }

    public void PointerExit()
    {
        buttonText.color = originalColor;
    }
}
