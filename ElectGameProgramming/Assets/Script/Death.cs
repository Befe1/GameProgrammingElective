using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death : MonoBehaviour
{
	public float time;

    void Start()
    {
		StartCoroutine (timer());
    }

	IEnumerator timer(){
		yield return new WaitForSeconds (time);
		Destroy (gameObject);
	}}
