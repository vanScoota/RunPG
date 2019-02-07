using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    Transform CamTransform;
    public Transform Player;
    public Vector3 minCameraPos = new Vector3(0,0,-10);
    public Vector3 maxCameraPos = new Vector3(32,0,-10);

    void Start()
    {
        CamTransform = Camera.main.transform;
    }

    void Update()
    {
        CamTransform.position = new Vector3(Mathf.Clamp(Player.position.x, minCameraPos.x, maxCameraPos.x), 
        Mathf.Clamp(CamTransform.position.y, minCameraPos.y, maxCameraPos.y), Mathf.Clamp(CamTransform.position.z, minCameraPos.z, maxCameraPos.z));
    }
}
