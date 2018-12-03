using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinningPlayer : MonoBehaviour
{
    [SerializeField] ParticleSystem[] WinParticles = null;
    [SerializeField] Animator anim = null;
    [SerializeField] AudioSource WinMusic = null;
    [SerializeField] Material mat = null;
    public void StartWinSequence(Player winningPlayer){
        anim.SetTrigger("PlayWinAnimation");
        mat.color = winningPlayer.meshRenderer.material.color;

        foreach(var w in WinParticles){
            w.Play();
        }
        WinMusic.Play();
    }

}
