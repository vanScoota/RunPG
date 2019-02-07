using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishFlagController : MonoBehaviour
{
	public string nextSceneName;
	new private BoxCollider2D collider;

	void Start()
	{
		collider = GetComponent<BoxCollider2D>();
	}
	
	void Update()
	{
		
	}

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
		}
	}
}
