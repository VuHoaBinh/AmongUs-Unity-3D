using UnityEngine;
using System.Collections;
using UnityEngine.AI;

[RequireComponent (typeof (NavMeshAgent))]
public class EnemyTM : LivingEntity {

	public enum State {Idle, Chasing, Attacking};
	State currentState;

    public Transform projectileSpawn; 
    public ProjectTitle projectTitle;
    public float muzzleVelocity = 10;
    public HealthBar healthBar;

	NavMeshAgent pathfinder;
	Transform target;
	LivingEntity targetEntity;
	Material skinMaterial;

	Color originalColour;

	float attackDistanceThreshold = .5f;
	float timeBetweenAttacks = 1;
	float damage = 1;

	float nextAttackTime;
	float myCollisionRadius;
	float targetCollisionRadius;

	bool hasTarget;

	// Game effects
	public ParticleSystem deathEffect;	
	public static event System.Action OnDeathStatic;


	void Awake() {
		pathfinder = GetComponent<NavMeshAgent> ();
		healthBar = GetComponent<HealthBar>();

		if (GameObject.FindGameObjectWithTag ("Player") != null) {
			hasTarget = true;

			target = GameObject.FindGameObjectWithTag ("Player").transform;
			targetEntity = target.GetComponent<LivingEntity> ();

			myCollisionRadius = GetComponent<CapsuleCollider> ().radius;
			targetCollisionRadius = target.GetComponent<CapsuleCollider> ().radius;

		}
	}
	protected override void Start () {
		base.Start ();
		if (hasTarget) {
			currentState = State.Chasing;
			// targetEntity.OnDeath += OnTargetDeath;
			StartCoroutine (UpdatePath ());
		}
	}

	public void SetCharacteristics(float moveSpeed, int hitsToKillPlayer, float enemyHealth, Color skinColour) {
		
		pathfinder.speed = moveSpeed;
		if (hasTarget) {
			//  Mathf.Ceil(targetEntity.startingHealth / hitsToKillPlayer);
            damage =2;
		}
		startingHealth = enemyHealth;
		deathEffect.startColor = new Color (skinColour.r, skinColour.g, skinColour.b, 1f);


		skinMaterial = GetComponent<Renderer> ().material;
		skinMaterial.color = skinColour;
		originalColour = skinMaterial.color;

	}

	void OnTargetDeath() {
		hasTarget = false;
		currentState = State.Idle;
	}

	public override void TakeHit(float damage, Vector3 hitPoint, Vector3 hitDirection) {
		// AudioManager.instance.PlaySound ("Impact", transform.position);
		if (health<=0) {
			if (OnDeathStatic != null) {
				OnDeathStatic();
			}
			AudioManager.instance.PlaySound ("Enemy Death", transform.position);

			Destroy(Instantiate (deathEffect.gameObject, hitPoint, Quaternion.FromToRotation(Vector3.forward ,hitDirection)) as GameObject, deathEffect.startLifetime);
		}
        
        
        
		base.TakeHit (damage, hitPoint, hitDirection);
        healthBar.TakeDamage(damage);
	}
	void Update () {

		if (hasTarget) {
            if (Time.time > nextAttackTime) {
                Vector3 directionToTarget = (target.position - transform.position).normalized;
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                if (Physics.Raycast(transform.position, directionToTarget, out RaycastHit hit, distanceToTarget)) {
                    Debug.DrawLine(transform.position, hit.point, Color.red, 1f); 
                    if (hit.collider.transform == target) {
                        nextAttackTime = Time.time + timeBetweenAttacks;
                        StartCoroutine(Attack());
                    }
                }
            }
        }
	}

    void Shoot() {
        Vector3 shootDirection = (target.position - projectileSpawn.position).normalized;
        ProjectTitle newProjectile = Instantiate(projectTitle, projectileSpawn.position, Quaternion.LookRotation(shootDirection)) as ProjectTitle;
        newProjectile.SetSpeed(muzzleVelocity);
    }



	IEnumerator Attack() {

		currentState = State.Attacking;
		pathfinder.enabled = false;

		Shoot();
        yield return new WaitForSeconds(timeBetweenAttacks);

		// skinMaterial.color = originalColour;
		currentState = State.Chasing;
		pathfinder.enabled = true;
	}


	IEnumerator UpdatePath() {
		float refreshRate = .25f;

		while (hasTarget) {
			if (currentState == State.Chasing) {
				Vector3 dirToTarget = (target.position - transform.position).normalized;
				Vector3 targetPosition = target.position - dirToTarget * (myCollisionRadius + targetCollisionRadius + attackDistanceThreshold/2);
				if (!dead) {
					pathfinder.SetDestination (targetPosition);
				}
			}
			yield return new WaitForSeconds(refreshRate);
		}
	}
}