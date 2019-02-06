using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ghostEnemy : MonoBehaviour
{

    public Vector3 startPos, newPos, tempPos;
    public float speed;
    public SpriteRenderer sr;
    public EdgeCollider2D ec;
    public BoxCollider2D bc;
    public Animator an;

    // Use this for initialization
    void Start()
    {
        startPos = transform.position;
        newPos = startPos;

        //zufällige Geschwindigkeit
        speed = Random.Range(1f, 5f);

        sr = gameObject.GetComponent<SpriteRenderer>();
        ec = gameObject.GetComponent<EdgeCollider2D>();
        an = gameObject.GetComponent<Animator>();
        bc = gameObject.GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        newPos = startPos;

        if (!an.GetBool("dead"))
        {
            newPos.x = newPos.x + Mathf.PingPong(Time.time * speed, 2) - 3;
            transform.position = newPos;
            //Bewegung positiv
            if (newPos.x < tempPos.x)
            {
                sr.flipX = false;
            }
            //Bewegung negativ
            if (newPos.x > tempPos.x)
            {
                sr.flipX = true;
            }
            tempPos = newPos;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (ec.IsTouchingLayers())
            {

                an.SetBool("dead", true);
                bc.enabled = false;
                ec.enabled = false;
                //gameObject.transform.position = new Vector3(transform.position.x, 2, transform.position.y);
                Destroy(gameObject, 1);
            }
            else
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }
}
