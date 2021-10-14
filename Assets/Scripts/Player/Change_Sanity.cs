using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Events;

public enum StateChangeTexture
{
	startTexture = 1,
	finalTexture = 2
}

public class Change_Sanity : MonoBehaviour
{
	[Header("Assignables")]
	[SerializeField] private Slider sanity;
	[SerializeField] private MovementBuka Buka;
	[SerializeField] private Transform orientation;
	[SerializeField] private Light sun;
	[SerializeField] private PostProcessVolume pp;

	[Header("Variables")]
	[SerializeField] private float forceChange = 0.01f;
	[SerializeField] private float maxForce = 0.07f;
	[SerializeField] private float valueToSpawnBuka = 0f;
	[SerializeField] private float changeLightSpeed = 5f;

	public UnityAction<StateChangeTexture> OnChangeSanity;

	private Player_Caress_Cat playerCC;
	private float SanityValue;
	private Vignette vignetteLayer = null;

	private bool _bukaSpawns;

	private void Start() {
		playerCC = GetComponent<Player_Caress_Cat>();

		SanityValue = sanity.value;
		playerCC.OnIncreaseSanity += IncreaseSanity;
		pp.profile.TryGetSettings(out vignetteLayer);

		StartCoroutine(nameof(DecreaseSanity));
	}

    private void FixedUpdate()
    {
		//IncreaseDifficulty
		forceChange = Mathf.Min(forceChange + Time.deltaTime / 1000f, maxForce);
    }


    private IEnumerator DecreaseSanity() {
		while(true) {
			yield return new WaitForSeconds(Time.deltaTime);
			vignetteLayer.intensity.value = 0.5f - sanity.value / 2;
			SanityValue = Mathf.Max(SanityValue - forceChange * Time.deltaTime, 0f);
			sanity.value = SanityValue;

			if(sanity.value <= valueToSpawnBuka && !_bukaSpawns) {
				//ChangeTexture
				OnChangeSanity?.Invoke(StateChangeTexture.finalTexture);
				
				//SpawnBuka
				Buka.gameObject.SetActive(true);
				Buka.transform.localPosition = transform.localPosition - orientation.transform.forward * 10;
				_bukaSpawns = true;

				//ChangeLight
				StopCoroutine(nameof(ChangeLight));
				StartCoroutine(nameof(ChangeLight), 0.1f);

				//Audio
				FindObjectOfType<AudioLevelManager>().Play("DarkAmbient");
				FindObjectOfType<AudioLevelManager>().Play("DarkMusic");
				FindObjectOfType<AudioLevelManager>().StopPlay("NormAmbient");
			}
		}
	}

	private IEnumerator ChangeLight(float value)
    {
		while (RenderSettings.ambientIntensity != value)
        {
			RenderSettings.ambientIntensity = Mathf.Lerp(RenderSettings.ambientIntensity, value, Time.deltaTime * changeLightSpeed);
			sun.intensity = Mathf.Lerp(sun.intensity, value, Time.deltaTime * changeLightSpeed);
			yield return null;
        }
    }

	private void IncreaseSanity(float valueChange) {
		SanityValue = Mathf.Min(SanityValue + valueChange,1);
		sanity.value = SanityValue;

		if(sanity.value > valueToSpawnBuka && _bukaSpawns) {
			//ChangeTexture
			OnChangeSanity?.Invoke(StateChangeTexture.startTexture);

			//DespawnBuka
			Buka.gameObject.SetActive(false);
			_bukaSpawns = false;

			//ChangeLight
			StopCoroutine(nameof(ChangeLight));
			StartCoroutine(nameof(ChangeLight), 1.5f);

			//Audio
			FindObjectOfType<AudioLevelManager>().StopPlay("DarkAmbient");
			FindObjectOfType<AudioLevelManager>().StopPlay("DarkMusic");
			FindObjectOfType<AudioLevelManager>().Play("NormAmbient");
		}
	}
}
