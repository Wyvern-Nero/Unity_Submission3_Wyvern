using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //____Definitions
    #region Definitions
    [Header("Player modifiers")]
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    
    [Header("Player parts")]
    private Rigidbody2D rb;
    [SerializeField] private GameObject cameraTarget;
    [SerializeField] private GameObject faceR;
    [SerializeField] private GameObject faceL;
    [SerializeField] private GameObject spriteIdleL;
    [SerializeField] private GameObject spriteIdleR;
    [SerializeField] private GameObject spriteRunL;
    [SerializeField] private GameObject spriteRunR;
    [SerializeField] private GameObject spriteJumpL;
    [SerializeField] private GameObject spriteJumpR;
    
    
    [Header("Groundcheck")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    
    [Header("Manager")]
    [SerializeField] private UIManager uiManager;
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private AudioManager AudioManager;
    
    [Header("Debug - Don't touch")]
    [SerializeField] private float direction;
    [SerializeField] private bool canDoubleJump;
    [SerializeField] private bool canJumpInFall;
    public bool canMove = true;
    #endregion
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        faceR.SetActive(true);
        spriteIdleR.SetActive(true);
    }

    void Update()
    {
        #region Movement
        //player cannot move when false - used primarily for menus and countdown
        if (canMove)
        { 
            //Left = - | Right = +
            direction = 0;
            
            //takes input
            direction = Input.GetAxisRaw("Horizontal");
           
            //input times speed = motion
            rb.linearVelocity = new Vector2(direction * speed, rb.linearVelocity.y);
           
            //jumping function. requires player to touch the ground or
            //when on the ground and jump isn't being pressed, disable double jump
            //necessary to prevent double jump being used while still on ground
            if (IsGrounded() && !Input.GetButton("Jump"))
            {
                canDoubleJump = false;
                canJumpInFall = true;
            }
            //Enables second jump without performing first jump when falling
            //this made sense at the time
            //im not "fixing" what works
            if (!IsGrounded() && !Input.GetButton("Jump") && canJumpInFall)
            {
                canJumpInFall = false;
                canDoubleJump = true;
            }

            // if jump then jump
            if (Input.GetButtonDown("Jump"))
            {
                //jump requires to be either on ground or have a double jump available
                if (IsGrounded() || canDoubleJump)
                {
                    rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);

                    canDoubleJump = !canDoubleJump;
                }
            }

            //different jump strength dependent on length of input
            if (Input.GetButtonUp("Jump") && rb.linearVelocity.y > 0f)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
                AudioManager.PlayJumpSound();
                Debug.Log(message: "PlayJumpSound");
            }
        }
        //if canMove = false, no more horizontal movement, stopping the player
        else
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }
        #endregion
        
        #region Sprites
        // if direction below 0 face left | if direction above 0 face right
        if (direction < 0)
        {
            //this will enable the gameobject with the left facing sprites and disable the other
            faceL.gameObject.SetActive(true);
            faceR.gameObject.SetActive(false);
            //change camera target position
            cameraTarget.transform.localScale = new Vector2(2f, 1f);
        }
        if (direction > 0)
        {
            //this will enable the gameobject with the right facing sprites and disable the other
            faceL.gameObject.SetActive(false);
            faceR.gameObject.SetActive(true);
            //change camera target position
            cameraTarget.transform.localScale = new Vector2(-2f, 1f);
        }
        
        //enable idle sprite when not moving
        if (direction == 0)
        {
            //enable idle sprites
            spriteIdleL.SetActive(true);
            spriteIdleR.SetActive(true);
            
            //disable running sprites
            spriteRunL.SetActive(false);
            spriteRunR.SetActive(false);
            
            //disable jumping sprites
            spriteJumpL.SetActive(false);
            spriteJumpR.SetActive(false);
        }
        //enable running sprite when moving
        if (IsGrounded() && direction != 0)
        {
            //enable running sprites
            spriteRunL.SetActive(true);
            spriteRunR.SetActive(true);
            
            //disable idle sprites
            spriteIdleL.SetActive(false);
            spriteIdleR.SetActive(false);
            
            //disable jumping sprites
            spriteJumpL.SetActive(false);
            spriteJumpR.SetActive(false);
        }
        //enable jumoing sprite when in air
        if (!IsGrounded())
        {
            //enable jumping sprites
            spriteJumpL.SetActive(true);
            spriteJumpR.SetActive(true);
            
            //disable idle sprites
            spriteIdleL.SetActive(false);
            spriteIdleR.SetActive(false);
            
            //disable running sprites
            spriteRunL.SetActive(false);
            spriteRunR.SetActive(false);
        }
        
        #endregion
        
    }
    
    #region collisions
    //isGrounded - checks if GroundCheck object intersects with ground layer
    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }
    
    //collisions with tagged objects
    private void OnTriggerEnter2D(Collider2D other)
    {
        //defeat targets by running into them
        if (other.CompareTag("Target"))
        {
            Destroy(other.gameObject);
            uiManager.AddTarget();
            Debug.Log(message: "Trigger" + other.name);
            AudioManager.PlayTargetSound();

        }
        //find secrets
        if (other.CompareTag("Secret"))
        {
            Destroy(other.gameObject);
            uiManager.AddSecret();
            Debug.Log(message: "Trigger" + other.name);
            AudioManager.PlaySecretSound();

        }
        //trigger that will cause a game over state upon contact. May also be used in moving enemies.
        if (other.CompareTag("Killzone"))
        {
            uiManager.ShowLosePanel();
            Debug.Log(message: "Trigger" + other.name);
            AudioManager.PlayDeathSound();
        }
        //trigger to start the countdown and moves player to starting line
        if (other.CompareTag("TriggerStart"))
        {
            uiManager.StartCountdown();
            Debug.Log(message: "Trigger" + other.name);

        }
        //trigger to end stopwatch and calculate score
        if (other.CompareTag("TriggerEnd"))
        {
            uiManager.StopTimer();
            Debug.Log(message: "Trigger" + other.name);
        }

        if (other.CompareTag("SafeZone"))
        {
            uiManager.LockHub();
        }
    }
    #endregion
}
