using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour {

    public LevelManager LevelManager;
	// Use this for initialization
	void Start () {
        LevelManager = FindObjectOfType<LevelManager>();
	}

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Debug.Log("Hit Killzone");
            LevelManager.RespawnPlayer();
        }
    }
}
