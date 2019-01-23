using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrouchColliderControler : MonoBehaviour {

    public bool enter = false;

    new private BoxCollider2D collider;

    // Use this for initialization
    void Start () {
        collider = GetComponent<BoxCollider2D>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D collider)
    {
        enter = true;
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        enter = false;
    }
}
