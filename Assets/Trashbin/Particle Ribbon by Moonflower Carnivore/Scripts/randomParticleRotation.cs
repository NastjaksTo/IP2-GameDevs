using System;
using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;
using static CombatSystem;

//randomize object rotation when particle system is inactive (which requires "Looping" unchecked as well).
public class randomParticleRotation : MonoBehaviour {
	public bool x=false;
	public bool y=false;
	public bool z=false;

	public float damage;
	void OnEnable() {
		if (x) {
			this.transform.localEulerAngles += new Vector3 (Random.value * 360f,0f,0f);
		}
		if (y) {
			this.transform.localEulerAngles += new Vector3 (0f,Random.value * 360f,0f);
		}
		if (z) {
			this.transform.localEulerAngles += new Vector3 (0f,0f,Random.value * 360f);
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			combatSystem.LoseHealth(damage);
		}
	}

	private void OnTriggerStay(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			combatSystem.LoseHealth(damage);
		}
	}
}