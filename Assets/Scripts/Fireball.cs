using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour {

    [SerializeField] float velX = 5f;
    [SerializeField] GameObject collisionVFX;
    [SerializeField] AudioClip collisionSound;

    float velY = 0f;
    Rigidbody2D myRigidBody;

	// Use this for initialization
	void Start () {
        myRigidBody = GetComponent<Rigidbody2D>();	
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Foreground")
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update () {
        myRigidBody.velocity = new Vector2(velX, velY);
        transform.Rotate(0, 0, 11420 * Time.deltaTime);
        Destroy(gameObject, 2f);
	}
}
