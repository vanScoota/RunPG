using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

    public GameObject currentCheckpoint;

    public GameObject player;

    public void RespawnPlayer()
    {
        player.transform.position = currentCheckpoint.transform.position;
        Debug.Log("Respawn");
    }
}
