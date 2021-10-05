using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeTexture : MonoBehaviour
{
	[SerializeField] private Change_Sanity PlayerCS;
	[SerializeField] private Texture CTexture;

	private Texture _sTexture;

	private void Start() {
		_sTexture = GetComponent<MeshRenderer>().material.mainTexture;
	}

	private void ChangeThisTexture(StateChangeTexture index) {
		if(index == StateChangeTexture.startTexture) {
			GetComponent<MeshRenderer>().material.mainTexture = _sTexture;
		} else if(index == StateChangeTexture.finalTexture) {
			GetComponent<MeshRenderer>().material.mainTexture = CTexture;
		}
	}

	private void OnEnable() {
		PlayerCS.OnChangeSanity += ChangeThisTexture;
	}
	private void OnDisable() {
		PlayerCS.OnChangeSanity -= ChangeThisTexture;
	}
}
