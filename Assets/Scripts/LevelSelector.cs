using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour {

    public string sceneToLoad;

	// Use this for initialization
	void Start () {

        PlayerPrefs.SetInt("Level1", 1);

        if (PlayerPrefs.GetInt(sceneToLoad.ToString()) == 1)
        {
            this.GetComponent<Button>().interactable = true;
        }

        else
        {
            this.GetComponent<Button>().interactable = false;
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void LoadLevel()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
