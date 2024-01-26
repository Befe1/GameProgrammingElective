using UnityEngine;
using System.Collections;
using UnityEngine.AI;
using System.Collections.Generic;

public class AITank : MonoBehaviour {
 	public delegate void DamageTakenEventHandler(int currentHealth);
    public event DamageTakenEventHandler OnDamageTaken;
    public delegate void TargetAcquiredEventHandler(GameObject target);
    public event TargetAcquiredEventHandler OnTargetAcquired;
    public delegate void TankDestroyedEventHandler();
    public event TankDestroyedEventHandler OnTankDestroyed;

    public GameObject currentTarget, wheelL, wheelR, gun, frontPos, spawner, opposingSpawner, lookAtGO, smoothMoveGO, destroyedGO;
    public float distance, idleWaitTime, smoothMoveSpeed, lookSpeed;
    public NavMeshAgent agent;
    public bool tankDead, firing, allied, finding, evading;
    public int health;
	public Renderer tankRenderer;
    //public ParticleSystem hitPS;
    public GameObject[] targetCache;

    private Vector3 startPosition;
    private int startHealth;
    private float velocity;

	// Use this for initialization
	void Start () {
		startPosition = transform.position;
		if(health==0){
			Destroy(gameObject);
		}
		startHealth = health;
		smoothMoveGO.transform.parent = null;
		lookAtGO.transform.parent = null;
		StartCoroutine (turnonAgent());
		if(agent == null){
			agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
		}
		if(agent.isOnNavMesh==true){
			agent.SetDestination(new Vector3(transform.position.x + UnityEngine.Random.Range(-25f,25f), transform.position.y, transform.position.z + UnityEngine.Random.Range(-25f,25f)));
		}
	}

	IEnumerator turnonAgent(){
		yield return new WaitForSeconds (0.5f);
		GetComponent<UnityEngine.AI.NavMeshAgent> ().enabled = true;
	}


	  public void TakeHit(int damage) {
        health -= damage;
        //hitPS.Emit(25);

        OnDamageTaken?.Invoke(health);
		StartCoroutine(FlashRed());

        if (health == 0) {
            destroyed();
        }
		
    }
	// Coroutine to flash the tank red
    IEnumerator FlashRed() {
        // Change color to red
        tankRenderer.material.color = Color.red;

        // Wait for one second
        yield return new WaitForSeconds(1f);

        // Revert color to original (assuming white, change as needed)
        tankRenderer.material.color = Color.white;
    }

	IEnumerator evade(){
		evading = true;
		yield return new WaitForSeconds(idleWaitTime);
		evading = false;
	}

	 void destroyed() {
        	tankDead = true;
    evading = false;
    Instantiate(destroyedGO, transform.position, transform.rotation);

    OnTankDestroyed?.Invoke();

    // Completely remove the tank from the game
    Destroy(gameObject);
    }


	void FixedUpdate(){
		//WHEELS ROTATION//
		velocity = agent.velocity.magnitude;
		if(velocity > 0.5f && Time.timeScale > 0f){
			wheelL.transform.Rotate(new Vector3(0,-10,0));
			wheelR.transform.Rotate(new Vector3(0,10,0));
		}
		//end wheels rotation//

		//LOOK AT LOGIC//
		if(currentTarget!=null){
			smoothMoveGO.transform.position = Vector3.MoveTowards(smoothMoveGO.transform.position, currentTarget.transform.position, Time.deltaTime * smoothMoveSpeed);
		}else{
			smoothMoveGO.transform.position = Vector3.MoveTowards(smoothMoveGO.transform.position, frontPos.transform.position, Time.deltaTime * smoothMoveSpeed);
		}
		lookAtGO.transform.position = Vector3.MoveTowards(lookAtGO.transform.position, smoothMoveGO.transform.position, Time.deltaTime * lookSpeed);
		if(evading == false){
			transform.LookAt(lookAtGO.transform.position);
			gun.transform.LookAt(lookAtGO.transform.position);
		}
		//END LOOK AT LOGIC//

		//DISTANCE CHECK AND ENEMIES TARGETING//
		if (currentTarget != null) {
			distance = Vector3.Distance (transform.position, currentTarget.transform.position);
		} else {//current target is null
			if(opposingSpawner!=null &&	opposingSpawner.GetComponent<tankSpawner>().npcs.Count > 0){
				currentTarget = opposingSpawner.GetComponent<tankSpawner>().npcs[UnityEngine.Random.Range(0,opposingSpawner.GetComponent<tankSpawner>().npcs.Count-1)];
				agent.enabled = true;
			}else{//either no opposing spawner or no units in opposing spawner
				if(finding == false){
					findEnemies ();
				}
			}
		}
		//END DISTANCE AND ENEMY CHECK//
	}

	void LateUpdate(){
		//NAVMESH CHECK AND MOVEMENT LOGIC//
		if (agent.isOnNavMesh == true) {
			if(currentTarget!=null){
				if(evading==false){
					agent.SetDestination (currentTarget.transform.position);
				}else{
					agent.SetDestination (startPosition);
				}
			}else{
				if(evading==true){
					agent.SetDestination (startPosition);
				}
			}
		}else{
			RaycastHit hit;
			if(Physics.Raycast(transform.position,transform.TransformDirection(Vector3.down),out hit,Mathf.Infinity)){
				if(hit.collider.CompareTag("Ground")){
					transform.position = hit.point;
					agent.Warp(hit.point);
					agent.enabled = true;
				}
			}
		}
		//END NAVMESH CHECK AND MOVEMENT LOGIC//

		//WEAPON FIRING LOGIC//
		if(evading == false){
		if(currentTarget!=null){
			if(distance < 75f){
				if(gun.GetComponent<genericGun> ().firing == false){
					gun.GetComponent<genericGun> ().firing = true;
				}
			}else{
				if(gun.GetComponent<genericGun> ().firing == true){
					gun.GetComponent<genericGun> ().firing = false;
				}
			}
		}else{
			if(gun.GetComponent<genericGun> ().firing == true){
				gun.GetComponent<genericGun> ().firing = false;
			}
		}
		}else{
			if(gun.GetComponent<genericGun> ().firing == true){
				gun.GetComponent<genericGun> ().firing = false;
			}
		}
		//END WEAPON FIRING LOGIC//
	}

	void OnTriggerEnter(Collider target){
		//PERCEPTION BASED TARGETING LOGIC//
		if(target.isTrigger==false){
			if (allied == true) {//if its an enemy tank
				if (target.CompareTag ("Enemy")) {
					currentTarget = target.gameObject;
				}
			} else {
				if(target.CompareTag("Allied")){
					currentTarget = target.gameObject;
				}
			}
		}
		OnTargetAcquired?.Invoke(currentTarget);
		//PERCEPTION BASED END TARGETING LOGIC//
	}

	public void findEnemies(){
		finding = true;
		if (allied) {
			targetCache = GameObject.FindGameObjectsWithTag ("Enemy");
			if(targetCache.Length>0){
				currentTarget = targetCache[UnityEngine.Random.Range(0,targetCache.Length-1)];
			}
		} else {
			targetCache = GameObject.FindGameObjectsWithTag ("Allied");
			if(targetCache.Length>0){
				currentTarget = targetCache[UnityEngine.Random.Range(0,targetCache.Length-1)];
			}
		}
		finding = false;
	}



}

