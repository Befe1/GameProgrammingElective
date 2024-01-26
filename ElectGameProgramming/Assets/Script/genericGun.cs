using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class genericGun : MonoBehaviour
{
    	public bool firing;
	private bool fired;
	public float fireRate;
	public ParticleSystem muzzleFire;
	public Transform firePosition;
	private Vector3 updatedFirePosition;
	private Quaternion updatedFireRotation;
	public int ammo;
	private bool usesAmmo;
	public List<bulletGeneric> bullets = new List<bulletGeneric>();
	private int currentBulletNumber;
	private Vector3 defaultBulletPos;
	public bool particleGun;
	public ParticleSystem particleSys;
	public GameObject damageZone;
	public bool userControlled;
	public Transform defaultBulletParent;

	void Start(){
		if(ammo > 0){
			usesAmmo = true;
		}
		if(bullets.Count>0){
			defaultBulletParent = bullets[0].transform.parent;
			defaultBulletPos = bullets[0].transform.localPosition;
		}
	}

	void Update(){
		if(userControlled){
			if(Input.GetMouseButton(0)){
				firing = true;
			}else{
				firing = false;
			}
		}

		if(particleGun==false){
			if(firing && !fired){
				StartCoroutine(fireMe());
			}
		}else{
			if(firing){
				//particleSys.enableEmission = true;
				damageZone.SetActive(true);
			}else{
				//particleSys.enableEmission = false;
				damageZone.SetActive(false);
			}
		}
	}

	void OnEnable(){
		if(firing){
			firing = false;
		}
		if(fired){
			fired = false;
		}
	}


    void LateUpdate()
    {
		updatedFirePosition = firePosition.position;
		updatedFireRotation = firePosition.rotation;
    }


	IEnumerator fireMe(){

		if(usesAmmo==true && ammo <= 0){
			gameObject.SetActive(false);
			yield break;
		}

		fired = true;
		bullets[currentBulletNumber].transform.parent = null;
		bullets[currentBulletNumber].gameObject.SetActive(true);
		currentBulletNumber++;
		if(currentBulletNumber > bullets.Count - 1){
			currentBulletNumber = 0;
		}
		if(muzzleFire!=null){
			muzzleFire.GetComponent<ParticleSystem> ().Emit (50);
		}
		if(usesAmmo==true){
			ammo--;
		}
		yield return new WaitForSeconds(fireRate);
		fired = false;
	}

	public void resetMe(bulletGeneric bullet){
		bullet.transform.parent = defaultBulletParent;
		bullet.transform.localPosition = defaultBulletPos;
		bullet.transform.rotation = transform.rotation;
		bullet.gameObject.SetActive(false);
	}

}
