using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// This class handles the trigger of the buttons in the levelselector scene.
/// </summary>
/// 



public class LevelSelector : MonoBehaviour {

    public string sceneToLoad;
    /// <summary>
    /// Checks availability of each scene.
    /// </summary>
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

    public void LoadLevel()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
