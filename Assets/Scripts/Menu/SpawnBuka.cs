using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBuka : MonoBehaviour
{
    [SerializeField] private GameObject buka;
    [SerializeField] private Camera camera;
    [SerializeField] private Light sun;
    [SerializeField] private float changeLightSpeed = 1f;
    [SerializeField] private GameObject[] spawnPoints;

    private int prevRandomValue = -1;
    private int randomValue = -1;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(nameof(Spawn));
    }

    private IEnumerator Spawn()
    {
        while (true)
        {
            //sun.intensity = Mathf.Max(sun.intensity - Time.deltaTime * changeLightSpeed, 0.1f);
            //RenderSettings.ambientIntensity = Mathf.Max(RenderSettings.ambientIntensity - Time.deltaTime * changeLightSpeed, 0.1f);
            StopCoroutine(nameof(ChangeLight));
            StartCoroutine(nameof(ChangeLight), 1.5f);
            yield return new WaitForSeconds(Random.Range(1.5f,3f));
            //sun.intensity = 0.1f;
            //RenderSettings.ambientIntensity = 0.1f;
            while (randomValue == prevRandomValue)
            {
                randomValue = Random.Range(0, spawnPoints.Length);  
            }
            prevRandomValue = randomValue;
            GameObject instBuka = Instantiate(buka, spawnPoints[randomValue].transform.position, Quaternion.identity, spawnPoints[randomValue].transform);
            instBuka.transform.LookAt(camera.transform);
            instBuka.GetComponent<AudioSource>().enabled = false;
            foreach (var particle in instBuka.GetComponentsInChildren<ParticleSystem>())
            {
                particle.Stop();
            }
            StopCoroutine(nameof(ChangeLight));
            StartCoroutine(nameof(ChangeLight), 0.1f);
            yield return new WaitForSeconds(Random.Range(0.5f, 1f));
            //RenderSettings.ambientIntensity = 1.5f;
            //sun.intensity = 1f;
            //sun.intensity = Mathf.Min(sun.intensity + Time.deltaTime * changeLightSpeed, 1f);
            //RenderSettings.ambientIntensity = Mathf.Min(RenderSettings.ambientIntensity + Time.deltaTime * changeLightSpeed, 1.5f);
            Destroy(instBuka);
        }
    }

    private IEnumerator ChangeLight(float value)
    {
        while (RenderSettings.ambientIntensity != value)
        {
            RenderSettings.ambientIntensity += (value - RenderSettings.ambientIntensity) / 100f * Time.deltaTime * changeLightSpeed;
            sun.intensity += (value / 1.5f - sun.intensity) / 100f * Time.deltaTime * changeLightSpeed;
            yield return null;
        }
    }
}
