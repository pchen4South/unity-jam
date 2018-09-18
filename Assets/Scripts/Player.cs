using UnityEngine;

public class Player : MonoBehaviour 
{
    public enum PlayerStatus { Alive, Dead, Invincible }

    [SerializeField] PlayerIndicator playerIndicator;
    [SerializeField] SkinnedMeshRenderer meshRenderer;
    [SerializeField] CharacterController controller;
    [SerializeField] Animator animator;
    [SerializeField] AudioSource takeDamageSound;
    [SerializeField] AudioSource deathSound;
    [SerializeField] AudioSource spawnSound;

    public int MaxHealth = 3;
    public float MoveSpeed = 2f;
    public System.Action<int, int> OnDeath;

    public AbstractWeapon Weapon;
    public int PlayerNumber = 0;
    public int Health = 1;
    public bool canMove = true;
    public bool canRotate = true;
	public PlayerStatus status = PlayerStatus.Alive;

    Vector3 GROUNDED_DOWNWARD_VELOCITY = new Vector3(0, -10f, 0);
    
    void Update() 
    {
        controller.Move(GROUNDED_DOWNWARD_VELOCITY);
    }

    public void SetColor(Color color)
    {
        playerIndicator.meshRenderer.material.color = color;
        meshRenderer.material.color = color;
    }

    public void Move(float xAxis, float yAxis)
    {
        var m = Vector3.zero;

        m.x = xAxis * Time.deltaTime * MoveSpeed;
        m.z = yAxis * Time.deltaTime * MoveSpeed;
        controller.Move(m);
        animator.SetFloat("Forward", m.magnitude);
    }

    public void SetWeapon(AbstractWeapon newWeapon)
    {
        if (Weapon)
        {
            Destroy(Weapon.gameObject);
        }
        Weapon = Instantiate(newWeapon, transform);
        Weapon.player = this;
        canRotate = true;
        canMove = true;
    }

    public void Damage(int amountOfDamage, int attackerIndex)
    {
        if (status == PlayerStatus.Dead)
            return;

        Health = Mathf.Max(0, Health - amountOfDamage);
        animator.SetTrigger("Hit");
        takeDamageSound.Play();

        if (Health <= 0)
        {
            Kill(attackerIndex);
        }
    }

    public void Kill(int attackerIndex)
    {
        status = PlayerStatus.Dead;
        Health = 0;
        canMove = false;
        canRotate = false;

        OnDeath.Invoke(PlayerNumber, attackerIndex);
        animator.SetBool("PlayDeathAnimation", true);
        deathSound.Play();
    }

	public void Spawn(Transform t)
	{
        status = PlayerStatus.Alive;
		Health = MaxHealth;
        canMove = true;
        canRotate = true;

		transform.SetPositionAndRotation(t.position, t.rotation);
        animator.SetBool("PlayDeathAnimation", false);
        spawnSound.Play();
	}
}