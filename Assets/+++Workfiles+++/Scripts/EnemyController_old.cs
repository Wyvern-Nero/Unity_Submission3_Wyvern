using System.Collections;
using UnityEngine;

public class EnemyController_old : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private float speed;
    [SerializeField] private bool chase;
    [SerializeField] private bool retreat;
    private GameObject player;

    [SerializeField] private Vector2 origin;
    
    void Start()
    {
        origin = new Vector2(transform.position.x, transform.position.y);
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (chase)
        {
            Chase();
            Flip();
        }
        
        if (retreat)
        {
            DroneReturn();
        }
    }
    
    
    
    #region drone movements
    //drone goes towards player
    private void Chase()
    {
        retreat = false;
        
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
    }
    
    private void DroneReturn()
    {
        chase = false;
        transform.position = Vector2.MoveTowards(transform.position, origin, speed * Time.deltaTime);
        if (new Vector2(transform.position.x, transform.position.y) == origin)
        {
            retreat = false;
        }

    }
    #endregion
    
    #region drone cosmetics
    //flips drone to face player
    private void Flip()
    {
        if (transform.position.x > player.transform.position.x)
            transform.localScale = new Vector3(-1, 1, 1);
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
        }
    }
    //exiting trigger stops chase
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            chase = false;
        }
    }
    #endregion
}
