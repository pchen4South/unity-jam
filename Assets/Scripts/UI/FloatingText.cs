using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FloatingText : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] Text floatingText;

    void Start(){
        AnimatorClipInfo[] clipinfo = animator.GetCurrentAnimatorClipInfo(0);
        var clipLength = clipinfo[0].clip.length;

        Destroy(gameObject, clipLength);
    }   

    public void SetText(string text){
        floatingText.text = text;
    }
}
