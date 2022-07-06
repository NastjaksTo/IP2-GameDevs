using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour {

	public int hits;

	private Animator anim;

	void Start (){
		anim = GetComponent<Animator> ();
	}

	void Update (){
		if (hits <= 0) {
			anim.SetBool ("isIdle", false);
			anim.SetBool ("isDead", true);
		}
	}

	void OnCollisionEnter (Collision co) {
		
		if (co.gameObject.tag == "Bullet") {
			if (hits > 0) {
				anim.SetBool ("isIdle", false);
				anim.SetBool ("isHit", true);
				hits--;
				StartCoroutine (ChangeState(anim.GetCurrentAnimatorClipInfo(0).Length, "isIdle", true));
				StartCoroutine (ChangeState(anim.GetCurrentAnimatorClipInfo(0).Length, "isHit", false));
			}
		}
	}

	IEnumerator ChangeState(float waitTime, string stateToChange, bool cond){

		yield return new WaitForSeconds (waitTime);
		anim.SetBool (stateToChange, cond);
	}
}
