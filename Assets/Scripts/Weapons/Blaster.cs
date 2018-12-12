using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blaster : AbstractWeapon {
    enum BlasterState {Ready, Firing, HittingValidTarget}

    BlasterState state = BlasterState.Ready;

    [Header("Cached references")]
    [SerializeField] ParticleSystem MuzzleEffect = null;
    public float pulseSpeed = 5f;
    public float DamageTickTime = 0.5f;

    [Header("State")]
    float timeTillNextShot = 0f;
    private GameObject FlashInstance = null;
    Ray ray = new Ray();
    RaycastHit rayHit = new RaycastHit();
    Vector3 muzzle = new Vector3();
    LineRenderer CurrentLine = null;
    GameObject HitPlayerEffect = null;
    Color origEmissionColor;
    float HittingValidTargetTime = 0f;
    AbstractCharacter targetBeingDamaged;
    bool initialDamageTick = false;
    
    void Awake(){
        MagazineSize = 12;
        AmmoCount = MagazineSize;   
        WeaponName = "Blaster";
        LeftHandIKTarget = IKTarget_L;
        RightHandIKTarget = IKTarget_R;
        //should change in base class to say hiteffect or something
        HitPlayerEffect = Instantiate(BulletHole);
        
        var tracerMat = bulletTracer.GetComponent<Renderer>().material;
        origEmissionColor = tracerMat.GetColor("_EmissionColor");
    }

    void Update()
    {
        timeTillNextShot -= Time.deltaTime;
        muzzle = transform.position + transform.forward * muzzleOffset;

        switch (state){
            case BlasterState.Ready:
                HittingValidTargetTime = 0;
                initialDamageTick = false;
                break;
            case BlasterState.Firing:
                HittingValidTargetTime = 0;
                initialDamageTick = false;
                DrawLine();
                CheckValidTarget();
                break;
            case BlasterState.HittingValidTarget:
                if(!initialDamageTick){
                    ApplyDamage();
                    initialDamageTick = true;
                } else {
                    HittingValidTargetTime += Time.deltaTime;
                    if(HittingValidTargetTime > DamageTickTime){
                        ApplyDamage();
                        HittingValidTargetTime = 0;
                    }
                }
                DrawLine();
                CheckValidTarget();
                break;
            default: break;
        }
    }

      public override void PullTrigger(Player player)
    {
        if (timeTillNextShot > 0) return;

        if(!fireSound.isPlaying)
            fireSound.Play();

        timeTillNextShot = fireRate;
        bulletTracer.enabled = true;
        state = BlasterState.Firing;
        DrawLine();
        MuzzleFlashLight.gameObject.SetActive(true);
        var meff = MuzzleEffect.emission;
        meff.enabled = true;
    }

    public override void ReleaseTrigger(Player player){
        bulletTracer.enabled = false;
        state = BlasterState.Ready;
        fireSound.Stop();
        MuzzleFlashLight.gameObject.SetActive(false);
        
        //disable particles
        AllowHitEffect(false);
        var meff = MuzzleEffect.emission;
        meff.enabled = false;
    }

    public void DrawLine(){
		ray.origin = muzzle;
		ray.direction = transform.forward;
        var didHit = Physics.Raycast(ray, out rayHit, Mathf.Infinity, layerMask);
        if (!didHit)
            return;
		if(bulletTracer != null){
        	bulletTracer.SetPosition(0, muzzle);
        	bulletTracer.SetPosition(1, rayHit.point);
            
            var tracerMat = bulletTracer.GetComponent<Renderer>().material;
            
            float floor = 0.01f;
            float ceiling = 1.0f;
            float emission = floor + Mathf.PingPong (Time.time * pulseSpeed, ceiling - floor);
            Color finalColor = origEmissionColor * Mathf.LinearToGammaSpace (emission);
            tracerMat.SetColor("_EmissionColor", finalColor);

		}
    }
    public void CheckValidTarget(){
        var isPlayer = rayHit.collider.CompareTag("PlayerHitbox");
		var isNPC = rayHit.collider.CompareTag("NPCHitbox");

        if (isPlayer || isNPC)
        {
			AbstractCharacter target;
			
			if (isNPC){
				target = rayHit.collider.GetComponent<BossMonster>();
                HitPlayerEffect.transform.position = rayHit.point;
            }
			else{
				target = rayHit.collider.GetComponentInParent<PlayerHitbox>().player;
                HitPlayerEffect.transform.position = target.transform.position;
            }
			if(!target.IsAlive()) return;
			
            targetBeingDamaged = target;
            HitPlayerEffect.transform.rotation = transform.rotation;
            AllowHitEffect(true);
            state = BlasterState.HittingValidTarget;
        } else {
		// if not player or npc then it hit terrain
            state = BlasterState.Firing;
            AllowHitEffect(false);
        }
    }
    void AllowHitEffect(bool allowed){
        var ps = HitPlayerEffect.GetComponentsInChildren<ParticleSystem>();
        foreach(ParticleSystem p in ps){
            var em = p.emission;
            em.enabled = allowed;
        }
    }

    void ApplyDamage(){
        //calculate damage and apply it here
        player.RegisterNewValidHit(player, targetBeingDamaged, DamageAmount);
    }



}
