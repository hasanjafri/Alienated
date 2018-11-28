using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour {

    [SerializeField] AudioClip switchSound;
    [SerializeField] Sprite switchOnSprite;
    [SerializeField] Sprite normalSprite;
    [SerializeField] float activeTimer;
    [SerializeField] Gem connectedGem;

    bool isSwitchedOn;
    float switchedOnTimer;

	// Use this for initialization
	void Start () {
        isSwitchedOn = false;
        switchedOnTimer = 0;
	}
	
	// Update is called once per frame
	void Update ()
    {
		if (isSwitchedOn)
        {
            switchedOnTimer += Time.deltaTime;
        } else
        {
            switchedOnTimer = 0;
        }

        if (switchedOnTimer > activeTimer)
        {
            GetComponent<SpriteRenderer>().sprite = normalSprite;
            isSwitchedOn = false;
            connectedGem.Disappear();
        }
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            turnSwitchOn();
        }
    }

    private void turnSwitchOn()
    {
        GetComponent<SpriteRenderer>().sprite = switchOnSprite;
        isSwitchedOn = true;
        connectedGem.Appear();
    }

    public void Disable()
    {
        if (isSwitchedOn)
        {
            isSwitchedOn = false;
        }
    }
}
