using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour {

    [SerializeField] Sprite normalDoorSprite;
    [SerializeField] Sprite doorOpenSprite;
    [SerializeField] float LevelLoadDelay = 2f;
    [SerializeField] float LevelExitSloMoRatio = 0.2f;

    BoxCollider2D myBoxCollider;
    bool isOpen;

	// Use this for initialization
	void Start () {
        myBoxCollider = GetComponent<BoxCollider2D>();
        isOpen = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            StartCoroutine(LoadNextLevel());
        }
    }

    IEnumerator LoadNextLevel()
    {
        Time.timeScale = LevelExitSloMoRatio;
        yield return new WaitForSecondsRealtime(LevelLoadDelay);
        Time.timeScale = 1f;

        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }
}
