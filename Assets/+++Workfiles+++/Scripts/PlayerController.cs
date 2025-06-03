using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private float direction;
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    public bool canMove = true;
    
    private LayerMask layerMask;
    private Rigidbody2D rb;
    
    [Header("Groundcheck")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    
    [Header("Manager")]
    [SerializeField] private UIManager uiManager;

    private bool faceR = true;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (canMove)
        {
            direction = 0;

            if (Keyboard.current.aKey.isPressed)
            {
                direction = -1;
                faceR = false;
            }

            if (Keyboard.current.dKey.isPressed)
            {
                direction = 1;
                faceR = true;
            }

            if (Keyboard.current.spaceKey.isPressed)
            {
                Jump();
            }
            
            rb.linearVelocity = new Vector2(direction * speed, rb.linearVelocity.y);
            
            if (faceR)
            {
                transform.localScale = new Vector3(-1f, 2, 1);
            }
            else
            {
                transform.localScale = new Vector3(1f, 2, 1);
            }
        }
    }

    void Jump()
    {
        if (Physics2D.OverlapCircle(groundCheck.position, 0.3f, groundLayer))
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(message: "Trigger" + other.name);

        if (other.CompareTag("Target"))
        {
            Destroy(other.gameObject);
            uiManager.AddTarget();
        } else if (other.CompareTag("Secret"))
        {
            Destroy(other.gameObject);
            uiManager.AddSecret();
        } else if (other.CompareTag("Killzone"))
        {
            uiManager.ShowLosePanel();
            canMove = false;
        } else if (other.CompareTag("TriggerStart"))
        {
            uiManager.StartTimer();
        } else if (other.CompareTag("TriggerEnd"))
        {
            uiManager.StopTimer();
        }
    }
}
