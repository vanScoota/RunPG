using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour {

    public string sceneToLoad;

	// Use this for initialization
	void Start () {
		//if(Player)
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void LoadLevel()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
