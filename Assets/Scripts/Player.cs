using UnityEngine;

public class Player : MonoBehaviour
{
    //This script is for the player. It controls the movement & shooting of the player. A separate manager script will control lives
    // When the player is hit, we will have an event listener in the manager that tells us to remove a life then react accordingly (end the game, etc)

    public GameObject bulletSpawn; //This is where we will fire bullets from.
    public GameObject[] bullets; //These are our bullets
    private int bulletIndex = 0;
    private bool _canMove;
    private float playerSpeed = 3.0f;
    private float playerHalfSize;
    private Vector3 screenSpace;
    private bool _isInvincible;

    public bool CanMove
    {
        get{ return _canMove;}

        set{ _canMove = value;}
    }

    public bool IsInvincible
    {
        get { return _isInvincible; }

        set { _isInvincible = value; }
    }

    void Start ()
    {
        //Need these so we can determine when the player is about to go off screen
        screenSpace = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0.0f, 0.0f));
        playerHalfSize = gameObject.GetComponent<Collider>().bounds.size.x / 2;
    }

    void Update ()
    {
		if (_canMove)
        {
            Movement();
        }   
	}

    private void LateUpdate()
    {
        if (_canMove)
        {
            Shooting();
        }
    }

    private void Shooting()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //Alright, now we need to have projectiles shoot forward from bulletSpawn's transform.position
            bullets[bulletIndex].transform.position = bulletSpawn.transform.position;
            Rigidbody bulletBody = bullets[bulletIndex].GetComponent<Rigidbody>();
            bulletBody.velocity = new Vector3(0, 0, 5);
            bulletIndex++;
            if (bulletIndex > bullets.Length-1)
            {
                bulletIndex = 0;
            }
        }
    }

    private void Movement()
    {
        transform.Translate(Vector3.right * Time.deltaTime * Input.GetAxis("Horizontal") * playerSpeed);
        if (transform.position.x > (screenSpace.x - playerHalfSize))
        {
            transform.position = new Vector3(screenSpace.x - playerHalfSize, transform.position.y, transform.position.z);
        }
        if (transform.position.x < (playerHalfSize - screenSpace.x))
        {
            transform.position = new Vector3(playerHalfSize - screenSpace.x, transform.position.y, transform.position.z);
        }
    }
}
