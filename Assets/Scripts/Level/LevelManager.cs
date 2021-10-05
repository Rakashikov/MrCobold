using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
	[SerializeField] private Player_Caress_Cat PlayerCC;
	[SerializeField] private Step_Cat Cat;
	[SerializeField] private List<Transform> SpawnPoints;
	[SerializeField] private float startSanityRestore = 1f;
	[SerializeField] private float minSanityRestore = 0.4f;

	private GameObject _spawnCat;

	private int prevRandomValue = -1;
	private int randomValue = -1;

	private void Start() {
		PlayerCC = GameObject.FindWithTag("Player").GetComponent<Player_Caress_Cat>();
		StartCoroutine(nameof(Spawns));
		StartCoroutine(nameof(IncreaseDif));
	}

    private IEnumerator Spawns() {
		while(true) {
			yield return new WaitForSeconds(1f);
			if(_spawnCat == null) {
				while (randomValue == prevRandomValue)
				{
					randomValue = Random.Range(0, SpawnPoints.Count);
				}
				prevRandomValue = randomValue;
				_spawnCat = Instantiate(Cat.gameObject, SpawnPoints[randomValue]);
				Cat.SetPlayerCC = PlayerCC;
				_spawnCat.GetComponent<Step_Cat>().SetPlayerCC = PlayerCC;
			}
		}
	}

	private IEnumerator IncreaseDif()
    {
        while (true)
        {
			yield return new WaitForSeconds(1f);
			startSanityRestore -= 0.001f;
			_spawnCat.GetComponent<Step_Cat>().RestoreSanity = Mathf.Max(startSanityRestore, 0.4f);
        }
    }
}
