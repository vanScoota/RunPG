using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {

    public LevelManager levelmanager;

    public void Start()
    {
        levelmanager = FindObjectOfType<LevelManager>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            levelmanager.currentCheckpoint = gameObject;
            Debug.Log("Checkpoint getroffen!");
        }
    }
}
