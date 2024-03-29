﻿using System;
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
        DisableHP();
        hpTimer = 0;
    }
	
	// Update is called once per frame
	void Update () {
        if (IsAlive())
        {
            if (IsFacingLeft())
            {
                myRigidBody.velocity = new Vector2(-moveSpeed, 0f);
            }
            else
            {
                myRigidBody.velocity = new Vector2(moveSpeed, 0f);
            }

            if (hpEnabled)
            {
                hpTimer += Time.deltaTime;
            }

            if (hpTimer > 2f)
            {
                DisableHP();
                hpTimer = 0;
            }
        }
        else
        {
            myRigidBody.velocity = new Vector2(0f, 0f);
            GetComponent<Animator>().SetTrigger("Dying");
            StartFading();
        }
	}

    private void DisableHP()
    {
        if (healthPoints > 0)
        {
            for (int x = 0; x < healthPoints; x++)
            {
                transform.GetChild(x).gameObject.GetComponent<SpriteRenderer>().enabled = false;
            }
            hpEnabled = false;
        }
    }

    private void ShowHP()
    {
        if (healthPoints > 0)
        {
            for (int x = 0; x < healthPoints; x++)
            {
                transform.GetChild(x).gameObject.GetComponent<SpriteRenderer>().enabled = true;
            }
            hpEnabled = true;
        }
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

    private bool IsAlive()
    {
        return healthPoints > 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Fireball")
        {
            Instantiate(blood, transform.position, Quaternion.identity);
            HandleDamage();
            ShowHP();
        }
    }

    private void HandleDamage()
    {
        if (healthPoints > 0)
        {
            healthPoints--;
            Destroy(transform.GetChild(0).gameObject);
        }
    }

    bool IsFacingLeft()
    {
        return transform.localScale.x < 0;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        transform.localScale = new Vector2(-(Mathf.Sign(myRigidBody.velocity.x)), 1f);

    }
}
