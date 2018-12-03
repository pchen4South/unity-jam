using UnityEngine;

public class FloatingTextController : Object
{
    public static void CreateFloatingText(string text, FloatingText PopupTextPrefab, RectTransform parent, Transform location)
    {
        Vector3 worldPosition = new Vector3(location.position.x, 0, location.position.z);
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(worldPosition);
        FloatingText instance = Instantiate(PopupTextPrefab, parent, false);

        instance.transform.position = screenPosition;
        instance.SetText(text);
    }
}