﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour {

    [SerializeField] AudioClip coinPickupSound;
    [SerializeField] int pointsForCoinPickup;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(gameObject);
    }

}