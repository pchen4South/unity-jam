using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class RaycastBeams{
    public RaycastHit[] hits = new RaycastHit[255];
    public RaycastHit validHit {get; set;}
    public bool shouldBeProcessed = false;
    public bool hasValidTarget = false;
}

public class WeaponTargettingArea : MonoBehaviour
{   
    enum TargettingState { Setup, NoneInRange, TargetsInRange, TargetsInLOS }
    [SerializeField] AbstractWeapon weapon;
    public float weaponRange = 10f;
    public float weaponArc = 50f;
    public float raycasts = 10f; 
    public bool showverts = false;
    [SerializeField] MeshFilter meshfilter = null;
    Mesh mesh;

    Vector3 rightBound = new Vector3();
    Vector3 leftBound = new Vector3();
    Vector3 lb = new Vector3();
    Vector3 rb = new Vector3();
    RaycastBeams[] beams = new RaycastBeams[255];
    Player playertarget = null;

    [Header("State")]
   
    TargettingState WeaponTargettingState = TargettingState.Setup;

    void Start(){
        mesh = meshfilter.mesh;
        //meshfilter.mesh = mesh;
        for(int i = 0; i < beams.Length; i++){
            beams[i] = new RaycastBeams();
            for(int j = 0; j < beams[i].hits.Length; j++){
                beams[i].hits[j] = new RaycastHit();
            }
        }
    }
    void Update(){
        if(!weapon) return;
        Debug.Log(weapon.player.ID + " " + WeaponTargettingState);
        
        switch (WeaponTargettingState){
            case (TargettingState.Setup):
                break;
            case (TargettingState.NoneInRange):
                if(playertarget != null)
                    playertarget.DangerIndicatorToggle(false);
                DrawTargettingArea();
                break;
            case (TargettingState.TargetsInRange):
                if(playertarget != null)
                    playertarget.DangerIndicatorToggle(false);
                DrawTargettingArea();    
                break;
            case (TargettingState.TargetsInLOS):
                playertarget.DangerIndicatorToggle(true);
                DrawTargettingArea();
                break;
            default:
                break;        
        }
    }
    
    // this will be called after the object is instatiated by SetWeapon on the weapon (if aim assist is on)
    public void Initialize(AbstractWeapon setWeapon){
        weapon = setWeapon;
        transform.position = weapon.transform.position;
        transform.localPosition = Vector3.zero;
        weaponRange = weapon.weaponRange;
        weaponArc = weapon.weaponArc;
        WeaponTargettingState = TargettingState.NoneInRange;
    }

    void DrawTargettingArea(){
        var rAngle = weaponArc / 2;
        var lAngle = -rAngle;
        
        Quaternion rotationR = Quaternion.AngleAxis(rAngle, Vector3.up); 
        Quaternion rotationL = Quaternion.AngleAxis(lAngle, Vector3.up); 
        Vector3 addDistanceToDirectionR = rotationR * transform.forward * weaponRange;
        Vector3 addDistanceToDirectionL = rotationL * transform.forward * weaponRange;

        rightBound = transform.position + addDistanceToDirectionR;
        leftBound = transform.position + addDistanceToDirectionL;
        rb = transform.InverseTransformPoint(rightBound);
        lb = transform.InverseTransformPoint(leftBound);

        // maybe will not need r/l line and just use the generated mesh
        // rLine.SetPosition(0, transform.position);
        // rLine.SetPosition(1, rightBound);
        
        // lLine.SetPosition(0, transform.position);
        // lLine.SetPosition(1, leftBound);
        // //
        
        Vector3[] verts = new Vector3[]{transform.InverseTransformPoint(transform.position), lb, rb};
        int[] tris =new int[]{0,1,2};
        Vector3[] normals = new Vector3[]{ Vector3.up, Vector3.up, Vector3.up};

        mesh.Clear();
        mesh.vertices = verts;
        mesh.triangles = tris;
        mesh.normals = normals;
        mesh.RecalculateBounds();

        var coll = GetComponent<MeshCollider>();
        coll.sharedMesh = null;
        coll.sharedMesh = mesh;
    }

    void OnTriggerExit(Collider other) {
        WeaponTargettingState = TargettingState.NoneInRange;    
    }

    void OnTriggerStay(Collider other)
    {
        var player = weapon.player;

        if(other.gameObject.tag == "Player" && other.gameObject.GetComponent<Player>().ID != player.ID){
            var target = other.gameObject.GetComponent<Player>();
            WeaponTargettingState = TargettingState.TargetsInRange;

            if(target.ID != player.ID){
                //cast rays
                CastRays();
                //TODO do something with this guy
                var abc = ProcessRays();
                if(abc.hasValidTarget != false){
                    var id = abc.validHit.collider.GetComponentInParent<PlayerHitbox>().player;
                    playertarget = id;
                }
            }
        }
    }

    // CastRays is called when a target is colliding with teh targetting area
    // the rays are to check if the target is also within LOS of the originating player
    // if a ray hits it should mark the target as hittable
    // if the trigger is pulled (PullTrigger() on the weapon is called)
    // <AbstractWeapon>.PullTrigger should be passed the rayhit from CastRays() ? or maybe just needs to know true/false if valid target is in range
    // it should cast all the rays every time because multiple targets can be in the range
    // maybe can optimize if there is only 1 target, not sure
    void CastRays(){
        float angleBetweenRays = weaponArc / raycasts;
        int rightBoundAngle = Mathf.RoundToInt(weaponArc / 2);
        int leftBoundAngle = -1 * rightBoundAngle;

        for(int i = 0; i < raycasts; i++){
            Quaternion rotationR = Quaternion.AngleAxis(leftBoundAngle + i * angleBetweenRays, Vector3.up); 
            Vector3 dir = rotationR * transform.forward * weaponRange;
            Ray newRay = new Ray(transform.position, dir);
            //create new struct beam of type RaycastBeams
            RaycastBeams beam = beams[i];
            
            //add beam to the beams array at index i      
            //cast the ray and put the results into the hits array
            var hitsArr = beam.hits;

            var raybeam = Physics.RaycastNonAlloc(newRay, hitsArr, weaponRange, weapon.layerMask);
            if(raybeam > 0){            
                beams[i] = beam;
                beam.shouldBeProcessed = true;
            }
            //debug
            Debug.DrawRay(transform.position, dir, Color.green);
            //draw one additional ray at the rightBound
            if (i == raycasts - 1){
                Vector3 rightBound =  Quaternion.AngleAxis(rightBoundAngle, Vector3.up) * transform.forward * weaponRange;
                Ray finalRay = new Ray(transform.position, rightBound);
                RaycastBeams finalBeam = new RaycastBeams();

                Physics.RaycastNonAlloc(finalRay,finalBeam.hits, weaponRange, weapon.layerMask);
                beams[i+1] = finalBeam;
                //debug
                Debug.DrawRay(transform.position, rightBound, Color.green);
            }
        }        
    }

    // processRays should process all the RaycastHits of every RaycastBeams 
    // the goal is to produce either null for no valid target
    // or return the RaycastHit of the closest valid target
    RaycastBeams ProcessRays(){
        RaycastBeams closestValidRaycastBeam = new RaycastBeams();
        float closestValidTargetDistance = 1000f;
        List<RaycastBeams> beamsWithValidTargetsAsTheClosest = new List<RaycastBeams>();
        
        //RaycastBeams closestPlayerRaycastHit;
        
        for(int i = 0; i < beams.Length; i++){

            //skip the beam if already processed
            if(beams[i] == null || beams[i].shouldBeProcessed == false) continue;
            //Debug.Log("processing beam at " + i);
            //these are for iterating thru the raycasthit array of each beam
            float closestTargetDistance = 1000f;
            RaycastHit closestHit = new RaycastHit();
            RaycastBeams validBeam = null;

            //cycle thru all the RaycastNonAlloc hits to find the closest
            for(int j = 0; j < beams[i].hits.Length; j++){
                var theHit = beams[i].hits[j];
                if(theHit.distance == 0) continue;
                //Debug.Log("theHit " + theHit.distance) ;
                if( theHit.distance <= closestTargetDistance){
                    closestTargetDistance = theHit.distance;
                    closestHit = theHit;
                    beams[i].validHit = theHit;
                    validBeam = beams[i];
                }
            }

            //Debug.Log("the closest hit is " + closestHit.collider.tag);
            if(closestHit.collider == null) continue;
            
            
            //if the target is valid
            var isPlayer = closestHit.collider.CompareTag("PlayerHitbox");
		    var isNPC = closestHit.collider.CompareTag("NPCHitbox");

            if(isPlayer || isNPC){
                beamsWithValidTargetsAsTheClosest.Add(validBeam);
                //else dont do anything
               
            } 
            beams[i].shouldBeProcessed = false; 
        }
        // Went thru all the beams and have a list of beams with valid targets
        // now go thru this list and find the closest one
        
        for(int k = 0; k < beamsWithValidTargetsAsTheClosest.Count; k++){
            var theBeam = beamsWithValidTargetsAsTheClosest[k];
            if(theBeam.validHit.distance < closestValidTargetDistance){
                closestValidTargetDistance = theBeam.validHit.distance;
                closestValidRaycastBeam = theBeam;
                closestValidRaycastBeam.hasValidTarget = true;
                Debug.Log("hitting player " + theBeam.validHit.collider.GetComponentInParent<Player>().ID);
            }
        }

        if(beamsWithValidTargetsAsTheClosest.Count > 0){
            WeaponTargettingState = TargettingState.TargetsInLOS;
        }         

        return closestValidRaycastBeam;
    }

}

