﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider : MonoBehaviour {

    [SerializeField] float moveSpeed = 1f;

    Rigidbody2D myRigidBody;

	// Use this for initialization
	void Start () {
        myRigidBody = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
        if (isFacingLeft())
        {
            myRigidBody.velocity = new Vector2(-moveSpeed, 0f);
        }
        else
        {
            myRigidBody.velocity = new Vector2(moveSpeed, 0f);
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
