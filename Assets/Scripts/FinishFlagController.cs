using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This class handles the trigger of the finish flags at the end of each level.
/// </summary>
public class FinishFlagController : MonoBehaviour
{
	public string nextSceneName;

	/// <summary>
	/// Checks for collisions with other game objects.
	/// If the other object is the player, load the next scene.
	/// </summary>
	/// <param name="collider">Collider of the colliding object.</param>
	void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.gameObject.tag == "Player")
		{
			SceneManager.LoadSceneAsync(nextSceneName);

            if (PlayerPrefs.GetInt(nextSceneName.ToString()) == 0)
            {
                //Level not activ -> enable
                PlayerPrefs.SetInt(nextSceneName.ToString(), 1);
            }
        }
	}
}
