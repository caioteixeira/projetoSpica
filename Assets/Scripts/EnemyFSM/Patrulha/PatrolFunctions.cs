using UnityEngine;
using System.Collections;

[System.Serializable]
public class PatrolValues
{
	[HideInInspector] public float walkSpeed;
	public Transform[] waypoints;
	public bool loop;
	public float patrolTime;
	public float restTime;
}

public class PatrolFunctions{
	
	private float updateTime;
	public float walkSpeed;
	public Transform[] waypoints;
	public int current = 0;
	public EnemyActions enemyActions;
	public bool loop;
	public float patrolTime;
	private float sPatrolTime;
	public float restTime;
	private float sRestTime;
	private Transform transform;
	private GameObject gameObject;
	private bool customUpdateTime;
	
	
	public PatrolFunctions(GameObject gameObject, PatrolValues patrolValues) {
		walkSpeed = patrolValues.walkSpeed;
		waypoints = patrolValues.waypoints;
		loop = patrolValues.loop;
		patrolTime = patrolValues.patrolTime;
		restTime = patrolValues.restTime;
		
		transform = gameObject.transform;
		this.gameObject = gameObject;
		enemyActions = new EnemyActions(gameObject);
		sPatrolTime = patrolTime;
		sRestTime = restTime;
		customUpdateTime = false;

	}
	public PatrolFunctions(GameObject gameObject, PatrolValues patrolValues, float updateTime) {
		walkSpeed = patrolValues.walkSpeed;
		waypoints = patrolValues.waypoints;
		loop = patrolValues.loop;
		patrolTime = patrolValues.patrolTime;
		restTime = patrolValues.restTime;
		
		transform = gameObject.transform;
		this.gameObject = gameObject;
		enemyActions = new EnemyActions(gameObject);
		sPatrolTime = patrolTime;
		sRestTime = restTime;
		customUpdateTime = true;
		this.updateTime = updateTime;
	}
	
	public void UpdatePatrol () {
		if(!customUpdateTime)
		{
			updateTime = Time.deltaTime;	
		}
		if(patrolTime > 0){
			//Debug.Log(current);
			patrolTime -= updateTime;
			if(EnemyFunctions.EnemyRay(1.0f, gameObject, Color.gray))
			{
				current++;	
			}
			if(current < waypoints.Length)
			{
				Vector3 target = waypoints[current].position;
				Vector3 moveDirection = target - transform.position;
			
				if(moveDirection.magnitude < 1){
					current++;
				}
				else
				{
					enemyActions.Walk(target, walkSpeed);
				}		
			}
			else
			{
				if(loop)
				{
					current = 0;	
				}
			}
		}
		else
		{
			enemyActions.Idle();
			if(restTime > 0){
			restTime -= updateTime;	
			}
			else
			{
				patrolTime = sPatrolTime;
				restTime = sRestTime;
			}
		}
	}
}
