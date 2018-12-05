using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class nextLevel : MonoBehaviour {

    public string sceneToLoad;

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Spieler")
        {
            Debug.Log("Nächstes Level");
            LoadLevel();
        }
    }
    void LoadLevel()
    {
        SceneManager.LoadScene(sceneToLoad);
        //Ist Level freigeschalten?
        if(PlayerPrefs.GetInt(sceneToLoad.ToString())== 0)
        {
            //Level freischalten
            PlayerPrefs.SetInt(sceneToLoad.ToString(), 1);
        }
        Debug.Log(PlayerPrefs.GetInt(sceneToLoad.ToString()));
    }
}
