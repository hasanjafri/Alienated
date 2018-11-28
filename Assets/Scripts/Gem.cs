using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour {

    [SerializeField] AudioClip coinPickupSound;
    [SerializeField] Switch connectedSwitch;

    private void Start()
    {
        GetComponent<SpriteRenderer>().enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(gameObject);
        connectedSwitch.Disable();
    }

    public void Appear()
    {
        GetComponent<SpriteRenderer>().enabled = true;
    }

    public void Disappear()
    {
        GetComponent<SpriteRenderer>().enabled = false;
    }
}
