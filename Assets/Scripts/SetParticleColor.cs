using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetParticleColor : MonoBehaviour
{
    [SerializeField] ParticleSystem particles;
    public Color startColor;


    public void SetColor(){
        var main = particles.main;
        main.startColor  = startColor;
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
