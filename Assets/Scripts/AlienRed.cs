using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class AlienRed : MonoBehaviour
{

  // Config params
  [SerializeField] float runSpeed = 5f;
  [SerializeField] float jumpSpeed = 5f;
  [SerializeField] float climbSpeed = 5f;
  [SerializeField] Vector2 deathKick = new Vector2(25f, 25f);
  [SerializeField] GameObject fireballRight, fireballLeft;
  [SerializeField] float fireRate = 0.5f;
  [SerializeField] float jumpBoost = 10f;
    

  // State
  bool isAlive = true;

  // Cached references
  Rigidbody2D myRigidBody;
  Animator myAnimator;
  CapsuleCollider2D myBodyCollider;
  BoxCollider2D myFeetCollider;
  float gravityScale;
  float inputTimer;
  Vector2 fireballPos;
  float nextFire = 0.0f;

  // Use this for initialization
  void Start()
  {
    myRigidBody = GetComponent<Rigidbody2D>();
    myAnimator = GetComponent<Animator>();
    myBodyCollider = GetComponent<CapsuleCollider2D>();
    myFeetCollider = GetComponent<BoxCollider2D>();
    gravityScale = myRigidBody.gravityScale;
    inputTimer = 0;
  }

  // Update is called once per frame
  void Update()
  {
    if (!isAlive)
    {
      return;
    }
    else
    {
      Run();
      fireBullet();
      ClimbLadder();
      Jump();
      FlipSprite();
      Idling();
    }
  }

  private void Idling()
  {
    if (myAnimator.GetCurrentAnimatorStateInfo(0).fullPathHash != 1991636315)
    {
      //print(myAnimator.GetCurrentAnimatorStateInfo(0).fullPathHash);
      return;
    }

    if (!Input.anyKey)
    {
      inputTimer += Time.deltaTime;
    }
    else
    {
      inputTimer = 0;
    }

    if (inputTimer >= 5f)
    {
      myAnimator.SetBool("Standing", false);
      inputTimer = 0;
    }
  }

  private void OnCollisionEnter2D(Collision2D collision)
  {
    if (collision.gameObject.tag == "Foreground")
    {
      myAnimator.SetBool("Jumping", false);
    }

    if (collision.gameObject.tag == "Springboard")
        {
            myRigidBody.AddForce(Vector2.up * jumpBoost);
            myAnimator.SetBool("Jumping", true);
        }

    Die();
  }

  private void OnTriggerEnter2D(Collider2D collision)
  {
    if (collision.gameObject.tag == "Climbing")
    {
      myAnimator.SetBool("Jumping", false);
      myAnimator.SetBool("Climbing", true);
    }
  }

  private void OnTriggerExit2D(Collider2D collision)
  {
    if (collision.gameObject.tag == "Climbing" && !myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Foreground")))
    {
      myAnimator.SetBool("Climbing", false);
      myAnimator.SetBool("Jumping", true);
    }
  }

  private void Run()
    {
        float controlThrow = CrossPlatformInputManager.GetAxis("Horizontal"); // value is between -1 to +1
        Vector2 playerVelocity = new Vector2((controlThrow * runSpeed), myRigidBody.velocity.y);
        myRigidBody.velocity = playerVelocity;

        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
        if (playerHasHorizontalSpeed)
        {
            myAnimator.SetBool("Running", true);
            myAnimator.SetBool("Standing", false);
        } else
        {
            myAnimator.SetBool("Running", false);
            myAnimator.SetBool("Standing", true);
        }
    }

    private void fireBullet()
    {
        if (CrossPlatformInputManager.GetButtonDown("Fire1") && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            fire();
        }
    }

    void fire()
    {
        if (myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            return;
        }

        fireballPos = transform.position;
        if (isFacingRight())
        {
            fireballPos += new Vector2(+1f, 0f);
            Instantiate(fireballRight, fireballPos, Quaternion.identity);
        } else
        {
            fireballPos += new Vector2(-1f, 0f);
            Instantiate(fireballLeft, fireballPos, Quaternion.identity);
        }
    }

    bool isFacingRight()
    {
        return transform.localScale.x > 0;
    }

    private void ClimbLadder()
    {
        if (!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            myAnimator.SetBool("Climbing", false);
            myRigidBody.gravityScale = gravityScale;
            return;
        }

        float controlThrow = CrossPlatformInputManager.GetAxis("Vertical");
        Vector2 climbVelocity = new Vector2(myRigidBody.velocity.x, (controlThrow * climbSpeed));
        myRigidBody.velocity = climbVelocity;
        myRigidBody.gravityScale = 0f;

        bool playerHasVerticalSpeed = Mathf.Abs(myRigidBody.velocity.y) > Mathf.Epsilon;
        myAnimator.SetBool("Climbing", playerHasVerticalSpeed);
    }

    private void Jump()
    {
        if (myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
          return;
        }

        if (!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Foreground", "Interactables")))
        {
            return;
        }

        if (CrossPlatformInputManager.GetButtonDown("Jump"))
        {
            Vector2 jumpVelocity = new Vector2(0f, jumpSpeed);
            myRigidBody.velocity += jumpVelocity;
            myAnimator.SetBool("Jumping", true);
        }
    }

    private void Die()
    {
        if (myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemy", "Obstacles", "Spikes")))
        {
            isAlive = false;
            //myBodyCollider.enabled = false;
            //myFeetCollider.enabled = false;
            myAnimator.SetTrigger("Dying");
            myRigidBody.velocity = deathKick;
            //FindObjectOfType<GameSession>().ProcessPlayerDeath();
        }
    }

    private void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
        if (playerHasHorizontalSpeed) 
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidBody.velocity.x), 1f);
        }
    }
}
