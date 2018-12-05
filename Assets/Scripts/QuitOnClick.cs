using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitOnClick : MonoBehaviour {

	public void Quit()
    {
        Debug.Log("Button geglickt - Spiel beenden ");
        Application.Quit();
    }
}
