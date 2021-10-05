using UnityEngine;

public class Camera_Move: MonoBehaviour {

    [SerializeField] private Transform player;

    void Update() {
        transform.position = player.position;
        try
        {
            GetComponentInChildren<Camera>().transform.rotation = transform.rotation;
        }
        catch
        {
            Debug.Log("Can't find camera");
        }
        
    }
}
