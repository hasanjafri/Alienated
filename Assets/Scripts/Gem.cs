using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour {

    [SerializeField] AudioClip coinPickupSound;
    [SerializeField] Switch connectedSwitch;

    Door door;

    private void Start()
    {
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
        countGemsToCollect();
    }

    private void countGemsToCollect()
    {
        door = FindObjectOfType<Door>();
        if (tag == "Gem")
        {
            door.CountGems();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Destroy(gameObject);
            connectedSwitch.Disable();
            door.GemCollected();
        }
    }

    public void Appear()
    {
        GetComponent<BoxCollider2D>().enabled = true;
        GetComponent<SpriteRenderer>().enabled = true;
    }

    public void Disappear()
    {
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
    }
}
