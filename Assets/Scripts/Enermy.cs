using UnityEngine;
using System.Collections;
using UnityEngine.AI;


// [RequireComponent (typeof (NavMeshAgent))]
public class Enermy : LivingEntity
{
    NavMeshAgent pathfinder;
	Transform target;

	protected override void Start () {

		base.Start ();

		pathfinder = GetComponent<NavMeshAgent> ();
		target = GameObject.FindGameObjectWithTag ("Player").transform;

		StartCoroutine (UpdatePath ());
	}

	void Update () {

	}

	IEnumerator UpdatePath() {
		float refreshRate = 1;

		while (target != null) {
			Vector3 targetPosition = new Vector3(target.position.x,0,target.position.z);
			if(!dead){
				pathfinder.SetDestination (targetPosition);
			}
			yield return new WaitForSeconds(refreshRate);
		}
	}
}
