using UnityEngine;
using UnityEngine.UI;
public class WeaponBar : MonoBehaviour {
	
	[Header("Prefabs")]
	public Player player;
    Camera m_Camera;
	

    void Start() {
        m_Camera = Camera.main;
        var slider = gameObject.GetComponentInChildren<Slider>();
        slider.value = 0;
    }
    
    void Update()
    {
        transform.LookAt(transform.position + m_Camera.transform.rotation * Vector3.forward,
                         m_Camera.transform.rotation * Vector3.up);

		transform.position = player.transform.position + Vector3.up * 2f;
        var slider = gameObject.GetComponentInChildren<Slider>();
    }
}
