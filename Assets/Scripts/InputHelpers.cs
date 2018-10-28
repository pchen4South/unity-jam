using UnityEngine;

public static class InputHelpers
{
    public static Vector3 VectorFromAxes(Rewired.Player p, int a1, int a2)
    {
       return new Vector3(p.GetAxis(a1), 0, p.GetAxis(a2));
    }

    public static Vector3? DirectionFromAxes(Rewired.Player p, int a1, int a2)
    {
        var v = Vector3.Normalize(VectorFromAxes(p, a1, a2));

        return v == Vector3.zero ? Vector3.zero : v;
    }

    public static void BasicMove(PlayerState p)
    {
        p.player.Move(VectorFromAxes(p.playerController, 0, 1));
    }

    public static void BasicRotate(PlayerState p)
    {
        p.player.transform.forward
            =  DirectionFromAxes(p.playerController, 5, 6) // try looking stick
            ?? DirectionFromAxes(p.playerController, 0, 1) // fall back to move stick
            ?? p.player.transform.forward;                 // use the current value
    }

    public static void BasicDash(PlayerState p)
    {
        if (p.playerController.GetButtonDown("Dash"))
        {
            p.player.Dash();
        }
    }

    public static void BasicPullTrigger(PlayerState p)
    {
        if (p.playerController.GetButton("Fire") || p.playerController.GetButtonRepeating("Fire"))
        {
            p.player.Weapon?.PullTrigger(p.player);
        }
    }

    public static void BasicReleaseTrigger(PlayerState p)
    {
        if (p.playerController.GetButtonUp("Fire"))
        {
            p.player.Weapon.ReleaseTrigger(p.player);
        }
    }
}