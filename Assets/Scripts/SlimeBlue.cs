using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeBlue : MonoBehaviour {

    [SerializeField] float moveSpeed = 1f;
    [SerializeField] GameObject blood;

    Rigidbody2D myRigidBody;
    SpriteRenderer mySpriteRenderer;
    int healthPoints;
    float hpTimer;
    bool hpEnabled;

	// Use this for initialization
	void Start () {
        myRigidBody = GetComponent<Rigidbody2D>();
        healthPoints = transform.childCount;
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        disableHP();
	}
	
	// Update is called once per frame
	void Update () {
        if (isAlive())
        {
            if (isFacingLeft())
            {
                myRigidBody.velocity = new Vector2(-moveSpeed, 0f);
                print(healthPoints);
            }
            else
            {
                myRigidBody.velocity = new Vector2(moveSpeed, 0f);
                print(healthPoints);
            }

            if (hpEnabled)
            {
                hpTimer += Time.deltaTime;
            }

            if (hpTimer > 2f)
            {
                disableHP();
            }
        }
        else
        {
            myRigidBody.velocity = new Vector2(0f, 0f);
            GetComponent<Animator>().SetTrigger("Dying");
            StartFading();
        }
	}

    private void disableHP()
    {
        for (int x = 0; x < healthPoints; x++)
        {
            transform.GetChild(x).gameObject.SetActive(false);
        }
        hpEnabled = false;
        hpTimer = 0;
    }

    private void showHP()
    {
        for (int x = 0; x < healthPoints; x++)
        {
            transform.GetChild(x).gameObject.SetActive(true);
        }
        hpEnabled = true;
    }

    IEnumerator FadeOut()
    {
        for (float f = 1f; f >= -0.05f; f -= 0.05f)
        {
            Color myColor = mySpriteRenderer.material.color;
            myColor.a = f;
            mySpriteRenderer.material.color = myColor;
            yield return new WaitForSeconds(0.05f);
        }
        Destroy(gameObject, 1f);
    }

    private void StartFading()
    {
        StartCoroutine(FadeOut());
    }

    private bool isAlive()
    {
        return healthPoints > 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Fireball")
        {
            Instantiate(blood, transform.position, Quaternion.identity);
            handleDamage();
            showHP();
        }
    }

    private void handleDamage()
    {
        if (healthPoints > 0)
        {
            healthPoints--;
            Destroy(transform.GetChild(0).gameObject);
        }
    }

    bool isFacingLeft()
    {
        return transform.localScale.x < 0;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        transform.localScale = new Vector2(-(Mathf.Sign(myRigidBody.velocity.x)), 1f);

    }
}
