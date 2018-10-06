using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinningPlayer : MonoBehaviour
{
    [SerializeField] ParticleSystem[] WinParticles;
    [SerializeField] Animator anim;
    [SerializeField] AudioSource WinMusic;
    [SerializeField] Material mat;
    public void StartWinSequence(Player winningPlayer){
        anim.SetTrigger("PlayWinAnimation");
        mat.color = winningPlayer.meshRenderer.material.color;

        foreach(var w in WinParticles){
            w.Play();
        }
        WinMusic.Play();
    }

}
