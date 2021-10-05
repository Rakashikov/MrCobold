using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Step_Cat : MonoBehaviour
{
	public float RestoreSanity = 1;
	private AudioSource audioSource;
    public Player_Caress_Cat SetPlayerCC { set => PlayerCC = value; }
	[SerializeField] private Player_Caress_Cat PlayerCC;

	private void Start()
	{
		audioSource = GetComponent<AudioSource>();
		StartCoroutine(nameof(PlaySound));
	}

	IEnumerator PlaySound()
	{
		while (true)
		{
			audioSource.Play();
			yield return new WaitForSeconds(5f);
		}
	}


	private void OnTriggerEnter(Collider other) {
		if(other.CompareTag("Player")) {
			PlayerCC.OnStepCat += StepCat;
		}
	}

	private void OnTriggerExit(Collider other) {
		if(other.CompareTag("Player")) {
			PlayerCC.OnStepCat -= StepCat;
		}
	}

	private void StepCat() {
		PlayerCC.OnIncreaseSanity(RestoreSanity);
		PlayerCC.OnStepCat -= StepCat;
		if (gameObject != null) Destroy(gameObject);
	}
	private void OnDestroy() {
		if(PlayerCC)
			PlayerCC.OnStepCat -= StepCat;
	}
}
