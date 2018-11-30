using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractPowerup : MonoBehaviour
{
    [SerializeField] AudioSource PickupSound;
    [SerializeField] ParticleSystem particles;
    [SerializeField] MeshRenderer buffContainer;
    [SerializeField] GameObject innerParent;
    public float EffectMultiplier = 2f;
    public float BuffTimer = 10f;

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player"){
            var player = other.gameObject.GetComponent<Player>();
            var soundtime = PickupSound.clip.length;
            ApplyEffect(player);
            particles.Stop();
            buffContainer.enabled = false;
            innerParent.SetActive(false);
            Destroy(gameObject, soundtime + .1f);
            PickupSound.Play();
        }
    }
    public virtual void ApplyEffect(Player player){}
}
