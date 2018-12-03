using UnityEngine;

public static class InputHelpers
{
    public static Vector3 VectorFromAxes(Rewired.Player p, int a1, int a2)
    {
       return new Vector3(p.GetAxis(a1), 0, p.GetAxis(a2));
    }

    public static Vector3? DirectionFromAxes(Rewired.Player p, int a1, int a2)
    {
        //this code is for radial deadzone
        float deadzone = 0.25f;
        Vector3 stickInput =  Vector3.Normalize(VectorFromAxes(p, a1, a2));
        
        if(stickInput.magnitude < deadzone)
             stickInput = Vector3.zero;
        else
            stickInput = stickInput.normalized * ((stickInput.magnitude - deadzone) / (1 - deadzone));

        //var v = Vector3.Normalize(VectorFromAxes(p, a1, a2));
        return stickInput == Vector3.zero ? Vector3.zero : stickInput;
    }

    public static void BasicMove(PlayerState p)
    {
        if(!p.player.IsAlive()) return;
        p.player.Move(VectorFromAxes(p.playerController, 0, 1));
    }

    public static void BasicRotate(PlayerState p)
    {
        // p.player.transform.forward
        //     =  DirectionFromAxes(p.playerController, 5, 6)             // try looking stick
        //     ?? DirectionFromAxes(p.playerController, 0, 1)             // fall back to move stick
        //     ?? p.player.transform.forward;                 // use the current value
        if(!p.player.IsAlive()) return;
        if( DirectionFromAxes(p.playerController, 5, 6)   != Vector3.zero){
             p.player.transform.forward
            =  DirectionFromAxes(p.playerController, 5, 6).Value;
        } else if(DirectionFromAxes(p.playerController, 0, 1)   != Vector3.zero){
             p.player.transform.forward
            =  DirectionFromAxes(p.playerController, 0, 1).Value;
        } else {
            p.player.transform.forward = p.player.transform.forward;
        }
    }

    public static void BasicDash(PlayerState p)
    {
        if(!p.player.IsAlive()) return;
        if (p.playerController.GetButtonDown("Dash"))
        {
            p.player.Dash(DirectionFromAxes(p.playerController, 0, 1).Value);
        }
    }

    public static void BasicPullTrigger(PlayerState p)
    {
        if(!p.player.IsAlive()) return;
        if (p.playerController.GetButton("Fire") || p.playerController.GetButtonRepeating("Fire"))
        {
            p.player.Weapon?.PullTrigger(p.player);
        }
    }

    public static void BasicReleaseTrigger(PlayerState p)
    {
        if(!p.player.IsAlive()) return;
        if (p.playerController.GetButtonUp("Fire"))
        {
            p.player.Weapon.ReleaseTrigger(p.player);
        }
    }
    public static void BasicReload(PlayerState p){
        if(!p.player.IsAlive()) return;
        
        if (p.playerController.GetButtonDown("Reload"))
        {
            p.player.Weapon.Reload();
        }
    }


    public static bool MenuDownOrRight(Rewired.Player p){
        if (p.GetAxis(0) > .5f || p.GetAxis(1) < -.5f){
            return true;
        } 
        return false;
    }

    public static bool MenuUpOrLeft(Rewired.Player p){
        if (p.GetAxis(0) < -.5f || p.GetAxis(1) > .5f){
            return true;
        } 
        return false;
    }

    public static bool MenuAccept(Rewired.Player p){
        // can add other buttons to this condition, so leaving it like this
        if (p.GetButton("Fire")){
            return true;
        } 
        return false;
    }


}