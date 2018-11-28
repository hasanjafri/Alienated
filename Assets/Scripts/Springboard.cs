using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Springboard : MonoBehaviour {

    [SerializeField] Sprite normalSprite;
    [SerializeField] Sprite springboardActivated;
    [SerializeField] AudioClip springboardSound;

    bool isActivated;
    float activeTimer;

	// Use this for initialization
	void Start () {
        isActivated = false;
        activeTimer = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (isActivated)
        {
            activeTimer += Time.deltaTime;
        } else
        {
            activeTimer = 0;
        }

        if (activeTimer > 2f)
        {
            GetComponent<SpriteRenderer>().sprite = normalSprite;
            isActivated = false;
        }
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            activateSwitch();
        }
    }

    private void activateSwitch()
    {
        GetComponent<SpriteRenderer>().sprite = springboardActivated;
        isActivated = true;
    }
}
