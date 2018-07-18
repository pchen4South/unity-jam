using UnityEngine;
using UnityEngine.UI;
public class WeaponBar : MonoBehaviour 
{
	[Header("Prefabs")]
    Camera m_Camera;
	
    public Image img;
    public Slider slider;
	public Player player;

    void Start() 
    {
        m_Camera = Camera.main;
        slider.value = 0;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawRay(transform.position, Vector3.left);
    }
    
    void Update()
    {
		transform.position = player.transform.position + Vector3.up * 2f;
        transform.LookAt(m_Camera.transform.position, Vector3.left);
    }
}