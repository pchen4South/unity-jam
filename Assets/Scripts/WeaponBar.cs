using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class WeaponBar : MonoBehaviour 
{
	[Header("Prefabs")]
    Camera m_Camera;
	
    public Image img;
    public Slider slider;
	public Player player;

    [Header("State")]
    private bool IsFlashing = false;
    public Color maxBarColor;
    private CanvasGroup cg;
        
    [Header("Config")]
    public float flashTime = .25f;
    void Start() 
    {
        cg = gameObject.GetComponent<CanvasGroup>();
        cg.alpha = 0;
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

        if(slider.value == 0){
            cg.alpha = 0;
            IsFlashing = false;
        } else if (slider.value > 0 && slider.value < 1){
            cg.alpha = 1;
            IsFlashing = false;
        } else if(slider.value == 1 ){
            if(!IsFlashing){
                StartCoroutine(FlashBarToWhite());       
            }
        } 
    }

    IEnumerator FlashBarToWhite(){
        IsFlashing = true;
        img.color = Color.white;       
        yield return new WaitForSeconds(flashTime);
        StartCoroutine(FlashBarToOriginal());

    }
     IEnumerator FlashBarToOriginal(){
        img.color = maxBarColor;
        yield return new WaitForSeconds(flashTime);
        IsFlashing = false;
     }



}