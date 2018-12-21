using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorFlickerAlpha : MonoBehaviour
{
    public float flickertime = 0.5f;
    public float minAlpha = .25f;
    public float maxAlpha = 1f;
    public SpriteRenderer sp;
    Color color;
    float timer = 0f;

    void Start(){
        color = sp.color;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if(timer >= 0 && timer <= flickertime)
            LerpToMin(flickertime,timer);
        else
            LerpToMax(flickertime, timer % flickertime);
        if(timer >= flickertime * 2)
            timer = 0f;
    }

    void LerpToMin(float flickertime, float timer){       
        var newCol = sp.color;
        newCol.a = Mathf.Lerp(newCol.a, minAlpha, timer / flickertime);
        sp.color = newCol;
    }
    void LerpToMax(float flickertime, float timer){       
        var newCol = sp.color;
        newCol.a = Mathf.Lerp(newCol.a, maxAlpha, timer / flickertime);
        sp.color = newCol;
    }

}
