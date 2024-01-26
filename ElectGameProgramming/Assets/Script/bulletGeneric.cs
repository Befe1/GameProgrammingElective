using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletGeneric : MonoBehaviour
{
    public float speed;
	public genericGun parentGun;
	public int damage;

	void Start () {
		if(speed == 0f){
			speed = 75f;
		}
		if(damage == 0){
			damage = 1;
		}
	}

	void Update () {
		transform.position += transform.forward * Time.deltaTime * speed;
	}

	void OnTriggerEnter(Collider target){
		if(target.isTrigger == false){
			if(target.GetComponent<AITank>()!=null){
				target.GetComponent<AITank>().TakeHit(damage);
			}
			//if(target.GetComponent<playerTank>()!=null){
				//target.GetComponent<playerTank>().TakeHit(damage);//
			//}//
			parentGun.resetMe(this);
		}
	}

	void OnEnable(){
		StartCoroutine(lifeTimer());
	}

	IEnumerator lifeTimer(){
		yield return new WaitForSeconds(3f);
		parentGun.resetMe(this);
	}
}
