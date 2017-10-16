using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {

    public float moveSpeed;

    public bool canClimb;
    public bool canClimbUp;
    public bool canDropDown;

    Rigidbody2D rigidBody;

    public bool immuneSnake;
    public bool immuneVine;
    public bool immuneSmoke;

    GameObject gameManagerObject;
    GameManagerScript gameManager;

    Animator animator;
    public string animationState;
    public int directionFacing;

	// Use this for initialization
	void Start ()
    {
        gameManagerObject = GameObject.FindWithTag("GameController");
        gameManager = gameManagerObject.GetComponent<GameManagerScript>();

        moveSpeed = 0.75f;//0.02f

        canClimb = false;

        rigidBody = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();

        immuneSnake = false;
        immuneVine = false;
        

        //Physics.IgnoreLayerCollision(6, 0, true);
        //Physics.IgnoreCollision(this.GetComponent<Collider>(), Snake.GetComponent<Collider>());
        //Debug.Log(Physics.GetIgnoreLayerCollision(6, 0));

    }
	
	// Update is called once per frame
	void Update ()
    {
        if (gameManager.gameState == "Playing")
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                if (canClimb || canClimbUp)
                {
                    SetAnimationState("ClimbLeft");
                    directionFacing = -1;
                    transform.position += Vector3.left * moveSpeed * Time.deltaTime;
                }
                else if (Input.GetKey(KeyCode.LeftShift))
                {
                    SetAnimationState("RunLeft");
                    directionFacing = -1;
                    transform.position += Vector3.left * moveSpeed * 2 * Time.deltaTime;
                }
                else
                {
                    SetAnimationState("WalkLeft");
                    directionFacing = -1;
                    transform.position += Vector3.left * moveSpeed * Time.deltaTime;
                }
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                if (canClimb || canClimbUp)
                {
                    SetAnimationState("ClimbRight");
                    directionFacing = -1;
                    transform.position += Vector3.right * moveSpeed * Time.deltaTime;
                }
                else if (Input.GetKey(KeyCode.LeftShift))
                {
                    SetAnimationState("RunRight");
                    directionFacing = -1;
                    transform.position += Vector3.right * moveSpeed * 2 * Time.deltaTime;
                }
                else
                {
                    SetAnimationState("WalkRight");
                    directionFacing = -1;
                    transform.position += Vector3.right * moveSpeed * Time.deltaTime;
                }
            }
            else if (canClimb || canClimbUp)
            {
                rigidBody.gravityScale = 0;
                SetAnimationState("ClimbIdle");
                if (Input.GetKey(KeyCode.DownArrow))
                {
                    SetAnimationState("Climb");
                    transform.position += Vector3.down * moveSpeed * Time.deltaTime;
                }
                if (canClimbUp)
                {
                    if (Input.GetKey(KeyCode.UpArrow))
                    {
                        SetAnimationState("Climb");
                        transform.position += Vector3.up * moveSpeed * Time.deltaTime;
                    }
                }
            }
            else
            {
                //rigidBody.gravityScale = 1;
                SetAnimationState("Idle");
            }
            if (!canClimb && !canClimbUp)
            {
                rigidBody.gravityScale = 1;
            }

            if (Input.GetKey(KeyCode.DownArrow))
            {
                //Physics.IgnoreLayerCollision(8, 0, true);
                //Debug.Log(Physics.GetIgnoreLayerCollision(8, 9));
                //turn off one way collider

                //canDropDown = true;
                /*RaycastHit hit;
                if (Physics.Raycast(new Vector2(transform.position.x, transform.position.y + 10.0f), Vector2.down, out hit, 10))
                {
                    Debug.DrawLine(transform.position, hit.transform.position, Color.magenta);
                    if (hit.transform.tag == "OneWayPlatform")
                    {
                        hit.transform.gameObject.SetActive(false);
                    }
                }*/
                if (canDropDown)
                {
                    this.GetComponent<BoxCollider2D>().isTrigger = true;
                }

            }
            else if (Input.GetKeyUp(KeyCode.DownArrow))
            {
                //Physics.IgnoreLayerCollision(8, 0, false);
                //Debug.Log(Physics.GetIgnoreLayerCollision(8, 9));
                canDropDown = false;
                this.GetComponent<BoxCollider2D>().isTrigger = false;
            }
        }

        if (transform.position.x <= -1.55)
        {
            transform.position = new Vector2(-1.55f, transform.position.y);
        }
        
        if (transform.position.x >= 1.55)
        {
            transform.position = new Vector2(1.55f, transform.position.y);
        }
        if (transform.position.y <= -0.8)
        {
            transform.position = new Vector2(transform.position.x, -0.8f);
        }

        /*RaycastHit hit;
        if (Physics.Raycast(new Vector2(transform.position.x, transform.position.y + 1.0f), Vector2.left, out hit, 10))
        {
            Debug.DrawLine(transform.position, hit.transform.position, Color.magenta);
            if (hit.transform.tag == "OneWayPlatform")
            {
                hit.transform.gameObject.SetActive(false);
            }
        }*/

    }

    public void ChangeAntidote(string antidoteType)
    {
        switch (antidoteType)
        {
            case "Snake":
                immuneSnake = true;
                immuneVine = false;
                immuneSmoke = false;
                gameManager.snakeImmuneImage.gameObject.SetActive(true);
                gameManager.vineImmuneImage.gameObject.SetActive(false);
                gameManager.smokeImmuneImage.gameObject.SetActive(false);
                break;
            case "Vine":
                immuneSnake = false;
                immuneVine = true;
                immuneSmoke = false;
                gameManager.snakeImmuneImage.gameObject.SetActive(false);
                gameManager.vineImmuneImage.gameObject.SetActive(true);
                gameManager.smokeImmuneImage.gameObject.SetActive(false);
                break;
            case "Smoke":
                immuneSnake = false;
                immuneVine = false;
                immuneSmoke = true;
                gameManager.snakeImmuneImage.gameObject.SetActive(false);
                gameManager.vineImmuneImage.gameObject.SetActive(false);
                gameManager.smokeImmuneImage.gameObject.SetActive(true);
                break;
        }
    }

    public void SetAnimationState(string currentState)
    {
        switch (currentState)
        {
            case "WalkLeft":
                animationState = currentState;
                animator.SetBool("Walking", true);
                animator.SetBool("OnLadder", false);
                animator.SetBool("Climbing", false);
                animator.SetBool("Running", false);
                gameObject.transform.localScale = new Vector3(-1, 1);
                break;
            case "WalkRight":
                animationState = currentState;
                animator.SetBool("Walking", true);
                animator.SetBool("OnLadder", false);
                animator.SetBool("Climbing", false);
                animator.SetBool("Running", false);
                gameObject.transform.localScale = new Vector3(1, 1);
                break;
            case "RunLeft":
                animationState = currentState;
                animator.SetBool("Walking", true);
                animator.SetBool("OnLadder", false);
                animator.SetBool("Climbing", false);
                animator.SetBool("Running", true);
                gameObject.transform.localScale = new Vector3(-1, 1);
                break;
            case "RunRight":
                animationState = currentState;
                animator.SetBool("Walking", true);
                animator.SetBool("OnLadder", false);
                animator.SetBool("Climbing", false);
                animator.SetBool("Running", true);
                gameObject.transform.localScale = new Vector3(1, 1);
                break;
            case "Climb":
                animationState = currentState;
                animator.SetBool("Walking", false);
                animator.SetBool("OnLadder", true);
                animator.SetBool("Climbing", true);
                gameObject.transform.localScale = new Vector3(1, 1);
                break;
            case "Idle":
                animationState = currentState;
                animator.SetBool("Walking", false);
                animator.SetBool("OnLadder", false);
                animator.SetBool("Climbing", false);
                //gameObject.transform.localScale = new Vector3(1, 1);
                break;
            case "ClimbIdle":
                animationState = currentState;
                animator.SetBool("Walking", false);
                animator.SetBool("OnLadder", true);
                animator.SetBool("Climbing", false);
                gameObject.transform.localScale = new Vector3(1, 1);
                break;
            case "ClimbLeft":
                animationState = currentState;
                animator.SetBool("Walking", true);
                animator.SetBool("OnLadder", true);
                animator.SetBool("Climbing", true);
                gameObject.transform.localScale = new Vector3(-1, 1);
                break;
            case "ClimbRight":
                animationState = currentState;
                animator.SetBool("Walking", true);
                animator.SetBool("OnLadder", true);
                animator.SetBool("Climbing", true);
                gameObject.transform.localScale = new Vector3(1, 1);
                break;
        }
    }

    void OnCollisionEnter2D(Collision2D whatIHit)
    {
        if (whatIHit.transform.tag == "OneWayPlatform")
        {

            canDropDown = true;
            if (canDropDown)
            {
                //whatIHit.transform.gameObject.SetActive(false);
                //StartCoroutine(turnColliderOn(whatIHit.gameObject));
            }
        }
    }

    IEnumerator turnColliderOn(GameObject collider)
    {
        yield return new WaitForSeconds(0.5f);
        collider.SetActive(true);
    }

    void OnTriggerStay2D(Collider2D whatIHit)
    {
        if (whatIHit.transform.tag == "Ladder")
        {
            canClimb = true;
            canClimbUp = true;
        }
    }

    void OnTriggerEnter2D(Collider2D whatIHit)
    {
        this.GetComponent<BoxCollider2D>().isTrigger = false;
        switch (whatIHit.transform.tag)
        {
            case "Ladder":
                canClimb = true;
                canClimbUp = true;
                //Debug.Log("Ladder");
                break;
            case "Antidote":
                string antidoteType;
                AntidoteScript antidoteScript = whatIHit.gameObject.GetComponent<AntidoteScript>();
                antidoteType = antidoteScript.antidoteType;
                ChangeAntidote(antidoteType);

                Destroy(whatIHit.gameObject);
                break;
            case "Hazard":
                string hazardType;
                HazardScript hazardScript = whatIHit.gameObject.GetComponent<HazardScript>();
                hazardType = hazardScript.hazardType;
                switch (hazardType)
                {
                    case "Snake":
                        if (!immuneSnake)
                        {
                            print("Player Killed By " + hazardType);
                            gameManager.RestartGame();
                        }
                        break;
                    case "Vine":
                        if (!immuneVine)
                        {
                            print("Player Killed By " + hazardType);
                            gameManager.RestartGame();
                        }
                        break;
                    case "Smoke":
                        if (!immuneSmoke)
                        {
                            print("Player Killed By " + hazardType);
                            gameManager.RestartGame();
                        }
                        break;
                }
                break;
            case "Portal":
                gameManager.GameWon();
                break;
            case "LadderTop":
                canClimb = true;
                canClimbUp = false;
                break;
            case "OneWayPlatform":
                canDropDown = true;
                break;
        }

        

        /*if (whatIHit.transform.tag == "Ladder")
        {
            canClimb = true;
        }
        if (whatIHit.transform.tag == "Antidote")
        {
            string antidoteType;
            AntidoteScript antidoteScript = whatIHit.gameObject.GetComponent<AntidoteScript>();
            antidoteType = antidoteScript.antidoteType;
            switch (antidoteType)
            {
                case "Snake":
                    immuneSnake = true;
                    break;
                case "Vine":
                    immuneVine = true;
                    break;
            }

            Destroy(whatIHit.gameObject);
        }
        if (whatIHit.transform.tag == "Snake")
        {
            if (immuneSnake)
            {

            }
            else
            {
                Debug.Log("dead");
            }
        }*/
        
    }

    void OnTriggerExit2D(Collider2D whatIHit)
    {
        this.GetComponent<BoxCollider2D>().isTrigger = false;
        if (whatIHit.transform.tag == "Ladder")
        {
            canClimb = false;
            canClimbUp = false;
        }
        if (whatIHit.transform.tag == "LadderTop")
        {
            canClimb = false;
            //canClimbDown = false;
        }
        if (whatIHit.transform.tag == "OneWayCollider")
        {
            canDropDown = false;
        }
    }
}
