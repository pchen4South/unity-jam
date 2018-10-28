using UnityEngine;

public abstract class Minigame : MonoBehaviour
{
    public virtual void HandleMove(PlayerState p) {}
    public virtual void HandleDash(PlayerState p) {}
    public virtual void HandleRotate(PlayerState p) {}
    public virtual void HandleFire(PlayerState p) {}
    public virtual void HandlePlayerDamage(PlayerState attacker, PlayerState victim) {}
}