using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player_Caress_Cat : MonoBehaviour
{
	public UnityAction OnStepCat;
	public UnityAction<float> OnIncreaseSanity;

	[SerializeField] private GameObject handsCat;
	[SerializeField] private Animator animatorHands;
	[SerializeField] private Animator animatorCat;

	private void Update() {
		if(Input.GetButtonDown("Caress") && OnStepCat?.GetInvocationList().Length > 0) {
			OnStepCat?.Invoke();
			StartCoroutine(nameof(AnimHands));
		}
	}

	IEnumerator AnimHands()
    {
		handsCat.SetActive(false);
		handsCat.SetActive(true);
		yield return null;
    }
}
