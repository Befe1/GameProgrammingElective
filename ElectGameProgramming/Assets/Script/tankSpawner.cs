using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tankSpawner : MonoBehaviour
{
public List<GameObject> npcs = new List<GameObject>();
	public int maxActive;
	public int currentActive;
	public GameObject tankToSpawn;
	public GameObject spawnpoint;
	public bool active;
	public float spawnPrompt;
	public float spawnRange;
	public GameObject opposingSpawner;
	public float delay;
	private GameObject newNPC;
	private GameObject newNPC123;
	public float interval;
	 public float currentActivePercentage;
	 
    public List<GameObject> npcsAllied = new List<GameObject>();

   

	void OnEnable(){
		StartCoroutine(spawnNewNPCS());
		StartCoroutine(InitializeDelayAndSpawn());
		maxActive = Random.Range(1, 101);
		spawnRange = Random.Range(1, 50);
		delay = Random.Range(1, 20);
		interval = Random.Range(1, 8);

    // Update spawnPrompt based on the new maxActive value
    // Example: spawnPrompt is 75% of maxActive
    spawnPrompt = maxActive * 0.25f;
	
    if(spawnpoint == null){
        spawnpoint = gameObject;
    }
    if (spawnpoint.GetComponent<MeshRenderer>() != null) {            
        spawnpoint.GetComponent<MeshRenderer>().enabled = false;
    }
    if(GetComponent<MeshRenderer>() != null){
        GetComponent<MeshRenderer>().enabled = false;
    }

	}
	

	void Start () {
		if(spawnpoint==null){
			spawnpoint = gameObject;
		}
		if(interval==0f){
			interval = 1f;	
		}

		if(spawnRange == 0f){spawnRange = 15f;}
		if (spawnpoint.GetComponent<MeshRenderer> () != null) {			
			spawnpoint.GetComponent<MeshRenderer> ().enabled = false;
		}
		if(GetComponent<MeshRenderer>()!=null){
			GetComponent<MeshRenderer> ().enabled = false;
		}
	}

	void LateUpdate(){
		spawnPrompt = maxActive - 1;
		if(spawnPrompt<0){
			spawnPrompt = 0;
		}
		currentActive = npcs.Count;
		npcs.RemoveAll(GameObject => GameObject == null);
		if(active && currentActive >= maxActive){
			active = false;
		}

		currentActivePercentage = (float)currentActive / maxActive * 100;


	}
	
	void Update () {
		if(currentActive <= spawnPrompt){
			if(!active){
				StartCoroutine(spawnNewNPCS());
			}
		}			
	}


    IEnumerator InitializeDelayAndSpawn()
    {
        // Wait until ChatGptManager has at least one chapter number
        yield return new WaitUntil(() => ChatGptManager.Instance.ChapterNumbers.Count > 0);

        // Set delay to the first chapter number
        delay = ChatGptManager.Instance.ChapterNumbers[2];

        // Now start spawning NPCs
        StartCoroutine(spawnNewNPCS());
    }




	IEnumerator spawnNewNPCS(){
		active = true;
		yield return new WaitForSeconds (delay);
		while (currentActive < maxActive) {
			newNPC = Instantiate (tankToSpawn, new Vector3 (spawnpoint.transform.position.x + Random.Range (-spawnRange, spawnRange), spawnpoint.transform.position.y, spawnpoint.transform.position.z + Random.Range (-spawnRange/2, spawnRange/2)), spawnpoint.transform.rotation) as GameObject;
			if (newNPC.GetComponent<UnityEngine.AI.NavMeshAgent> () != null) {
				newNPC.GetComponent<UnityEngine.AI.NavMeshAgent> ().Warp (newNPC.transform.position);
			}
			npcs.Add (newNPC);
				if (opposingSpawner != null) {
					newNPC.GetComponent<AITank> ().opposingSpawner = opposingSpawner;
				}
			currentActive = currentActive - 1;
			yield return new WaitForSeconds (interval);
		}

	}

}
