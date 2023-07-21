using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemText : MonoBehaviour
{
    TextMeshProUGUI text;

    void Start()
    {
        //text = GetComponent<TextMeshProUGUI>();

        //ReadText(text);

        text = GetComponent<TextMeshProUGUI>();
        if (text == null)
        {
            Debug.LogError("TextMeshPro component not found on the GameObject.");
            return;
        }

        ReadText(text);


    }

    private void ReadText(TextMeshProUGUI text)
    {
        TextAsset textAsset = Resources.Load<TextAsset>("UI/Text/textTest");
        if (textAsset != null)
        {
            // Access textAsset.text to get the content as a string
            string textContent = textAsset.text;

            text.text = textContent;
        }
    }
}
