using UnityEngine;

public class PlayerHUD : MonoBehaviour 
{
	[SerializeField]
	RectTransform rectTransform;
	[SerializeField]
	GraphBar lifeBar;

	public Vector2 screenSpaceOffset = Vector2.zero;
	public Color healthyColor = Color.green;
	public Color deadColor = Color.red;

	public void UpdateHealth(int currentHealth, int maxHealth)
	{
		var normalizedHealth = (float) currentHealth / (float) maxHealth;
		var barColor = Color.Lerp(deadColor, healthyColor, normalizedHealth);

		lifeBar.UpdateBar(barColor, normalizedHealth);
	}

	public void UpdatePosition(Camera cam, RectTransform parent, Vector3 worldPosition)
	{
		Vector2 position = cam.WorldToViewportPoint(worldPosition);

		position -= new Vector2(.5f, .5f);
		position *= parent.sizeDelta;
		position += screenSpaceOffset;
		rectTransform.anchoredPosition = position;
	}
}