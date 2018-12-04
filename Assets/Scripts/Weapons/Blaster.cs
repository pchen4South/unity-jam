using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blaster : AbstractWeapon {
    enum BlasterState {Firing, Ready}

    BlasterState state = BlasterState.Ready;

    [Header("Cached references")]
    [SerializeField]    GameObject muzzleFlash = null;
    [SerializeField]    Light muzzleFlashLight = null;

    [Header("State")]
    float timeTillNextShot = 0f;
    private GameObject FlashInstance = null;
    Ray ray = new Ray();
    RaycastHit rayHit = new RaycastHit();
    Vector3 muzzle = new Vector3();
    LineRenderer CurrentLine = null;
    void Awake(){
        MagazineSize = 12;
        AmmoCount = MagazineSize;   
        FlashInstance = Instantiate(muzzleFlash, transform);     
        WeaponName = "Blaster";
        LeftHandIKTarget = IKTarget_L;
        RightHandIKTarget = IKTarget_R;
    }

    void Update()
    {
        timeTillNextShot -= Time.deltaTime;
        muzzle = transform.position + transform.forward * muzzleOffset;
        if (state == BlasterState.Firing)   
            DrawLine();
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
    }

    public override void ReleaseTrigger(Player player){
        bulletTracer.enabled = false;
        state = BlasterState.Ready;
        fireSound.Stop();
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
		}
    }
}
