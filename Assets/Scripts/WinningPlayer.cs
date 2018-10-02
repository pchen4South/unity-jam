using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinningPlayer : MonoBehaviour
{
    [SerializeField] ParticleSystem[] WinParticles;
    [SerializeField] Animator anim;
    [SerializeField] AudioSource WinMusic;
    public void StartWinSequence(){
        anim.SetTrigger("PlayWinAnimation");
        foreach(var w in WinParticles){
            w.Play();
        }
        WinMusic.Play();
    }

}
