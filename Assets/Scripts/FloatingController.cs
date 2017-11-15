using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingController : MonoBehaviour {

    private static FloatingText popupText;
    private static GameObject canvas;
    
    public static void Initialize()
    {
        canvas = GameObject.Find("MainCanvas");

        if (!popupText)
        {
            popupText = Resources.Load<FloatingText>("Prefabs/DamagePopup");
        }
        //popupText = Resources.Load<FloatingText>("Prefabs/DamagePopup");

    }
    
    public static void CreateFloatingText(string Text,Transform location)
    {
        FloatingText instance = Instantiate(popupText);
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(location.position);
        instance.transform.SetParent(canvas.transform, false);
        instance.transform.position = screenPosition;
        instance.transform.GetChild(0).GetComponent<Text>().color = Color.red;
        instance.setText(Text);
    }
}
