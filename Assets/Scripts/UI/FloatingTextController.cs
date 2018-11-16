using UnityEngine;
using System.Collections;

public class FloatingTextController : MonoBehaviour {
    private static FloatingText popupText;
    private static RectTransform textParent;

    public static void Initialize(RectTransform TextParent, FloatingText PopupTextPrefab)
    {
        textParent = TextParent;
        if (!popupText)
            popupText = PopupTextPrefab;
    }

    public static void CreateFloatingText(string text, Transform location)
    {
        FloatingText instance = Instantiate(popupText);
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(new Vector3(-.5f + location.position.x + Random.Range(-.2f, .2f),0, .5f + location.position.z + Random.Range(-.2f, .2f)));

        instance.transform.SetParent(textParent.transform, false);
        instance.transform.position = screenPosition;
        instance.SetText(text);
    }
}