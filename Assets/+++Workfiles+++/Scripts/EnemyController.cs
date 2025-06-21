using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    #region definitions
    [Header("Input")]
    [SerializeField] private float speed;
    [SerializeField] private float speedChase;
    [SerializeField] private float patrolEnd;
    [SerializeField] private float patrolPause;
    [SerializeField] private UIManager uiManager;
    
    [Header("Debug")]
    [SerializeField] private int patrolIndex;
    [SerializeField] private bool chase;
    [SerializeField] private Vector3 patrolStartPos;
    [SerializeField] private Vector3 patrolEndPos;
    [SerializeField] private GameObject patrolTarget;
    
    private GameObject player;
    #endregion

    void Start()
    {
        //some baseline values so the drone remembers where it came from
        patrolStartPos.x = transform.position.x;
        patrolStartPos.y = transform.position.y;
        //and doesn't deviate in height
        patrolEndPos.y = transform.position.y;
        
        //defines player for later tracking
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        //calculates end position in live so the value can be edited in play mode
        patrolEndPos.x = patrolStartPos.x + patrolEnd;
        //moves patrolTarget to end position for visualization
        patrolTarget.transform.position = patrolEndPos;
        
        //patrolReturn 0 means go to end position
        if (patrolIndex == 1)
        {
            moveToEnd();
            faceTarget();
        }

        //patrolReturn 2 means go to start position
        if (patrolIndex == 0)
        {
            moveToStart();
        }
        
        //patrol position 0 means hold position at start or end point and initiate switch
        if (transform.position.x == patrolStartPos.x || transform.position.x == patrolEndPos.x)
        {
            patrolIndex = 2;
            Invoke("switchReturn", patrolPause);
        }

        //on chase, go outside patrol index to avoid other states
        if (chase)
        {
            patrolIndex = 4;
            moveToPlayer();
        }
        
        //correct drone View directon by inverting x axis
        faceTarget();
        
        //reset drone position upon world reset
        if (uiManager.resetWorld)
        {
            transform.position = patrolStartPos;
            patrolIndex = 0;
            uiManager.resetWorld = false;
        }
    }

    #region Movement
    //these move the drone from point a to point b and inverse
    
    private void moveToPlayer()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speedChase * Time.deltaTime);
    }
    private void moveToStart()
    {
        transform.position = Vector3.MoveTowards(transform.position, patrolStartPos, speed * Time.deltaTime);
    }
    private void moveToEnd()
    {
        transform.position = Vector3.MoveTowards(transform.position, patrolEndPos, speed * Time.deltaTime);
    }
    
    //I don't know why this methode works and the other 10+ ideas I tried didn't
    //but it finally works now... only took me too many hours
    //the hubris of thinking you can write a function without a tutorial
    //so much time was lost here attempting different methods.
    //anyways. depending on where this void is invoked, it changes the drones destination
    private void switchReturn()
    {
        if (transform.position.x == patrolStartPos.x)
        {
            patrolIndex = 1;
        }

        if (transform.position.x == patrolEndPos.x)
        {
            patrolIndex = 0;
        }
    }
    #endregion
    
    #region trigger
    //entering trigger leads to chase behavior
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            chase = true;
        }

        if (other.CompareTag("SafeZone"))
        {
            chase = false;
            patrolIndex = 0;
        }
    }
    //exiting trigger stops chase
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            chase = false;
            patrolIndex = 0;
        }
    }
    #endregion
    
    #region drone cosmetics
    //flips drone to face player
    private void faceTarget()
    {
        //look to player if to the left
        if (chase && transform.position.x > player.transform.position.x)
            transform.localScale = new Vector3(-1, 1, 1);
        //look to player if to the right
        if (chase && transform.position.x < player.transform.position.x)
            transform.localScale = new Vector3(1, 1, 1);
        
        //look left when moving to start position
        if (patrolIndex == 0)
            transform.localScale = new Vector3(-1, 1, 1);;
        
        //look right when moving to end position
        if (patrolIndex == 1)
            transform.localScale = new Vector3(1, 1, 1);

    }
    #endregion
}
