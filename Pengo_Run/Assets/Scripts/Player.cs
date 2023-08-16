using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public float speed;
    public float laneSpeed;
    public float jumpLength;
    public float jumpHeigth;

    private Animator anim;
    private float currentLane = 1;
    private Rigidbody rb;
    private BoxCollider boxCollider;
    private Vector3 verticalTargetPosition;
    private bool jumping = false;
    private float jumpStart;
    private bool isSwipping;
    private Vector2 startingTouch;
    private UIManager uiManager;
    [HideInInspector]
    public float score;
    private float maxSpeed = 30;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        uiManager = FindObjectOfType<UIManager>();
        anim.Play("runStart");
        GameManager.gm.StartMissions();
    }

    // Update is called once per frame
    void Update()
    {
        score += Time.deltaTime * speed;
        uiManager.UpdateScore((int)score);
        if (Input.GetKeyDown(KeyCode.LeftArrow)){
            ChangeLane(-1);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            ChangeLane(1);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Jump();
        }

        if(Input.touchCount == 1)
        {
            if (isSwipping)
            {
                Vector2 diff = Input.GetTouch(0).position - startingTouch;
                diff = new Vector2(diff.x / Screen.width, diff.y / Screen.width);
                if(diff.magnitude > 0.01f)
                {
                    if (Mathf.Abs(diff.y) > Mathf.Abs(diff.x))
                    {
                        if(diff.y > 0)
                        {
                            Jump();
                        }
                    }
                    else
                    {
                        if(diff.x<0)
                        {
                            ChangeLane(-1);

                        }
                        else
                        {
                            ChangeLane(1);
                        }
                    }

                    isSwipping = false;

                }

            }

            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                startingTouch = Input.GetTouch(0).position;
                isSwipping = true;
            }
            else if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                isSwipping = false;
            }
        }

        if (jumping)
        {
            float ratio = (transform.position.z - jumpStart) / jumpLength;
            if(ratio >= 1)
            {
                jumping = false;
                anim.SetBool("Jumping", false);

            }
            else
            {
                verticalTargetPosition.y = Mathf.Sin(ratio * Mathf.PI) * jumpHeigth;
            }
        }
        else
        {
            verticalTargetPosition.y = Mathf.MoveTowards(verticalTargetPosition.y, 0, 5*Time.deltaTime); 
        }
            Vector3 targetPosition = new Vector3(verticalTargetPosition.x, verticalTargetPosition.y, transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, laneSpeed * Time.deltaTime);

    }

    void ChangeLane(float direction)
    {
        float targetLane = currentLane + direction;
        if(targetLane < 0 || targetLane > 2)
        {
            return;
        }
        currentLane = targetLane;
        verticalTargetPosition = new Vector3((currentLane - 1), 0, 0);
    }

    void Jump()
    {
        if (!jumping)
        {
            jumpStart = transform.position.z;
            anim.SetFloat("JumpSpeed", speed / jumpLength);
            anim.SetBool("Jumping", true);
            jumping = true;
        }
    }
    private void FixedUpdate()
    {
        rb.velocity = Vector3.forward * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
       
        if (other.CompareTag("Obstacle"))
        {
            speed = 0;
            uiManager.gameOverPanel.SetActive(true);
            Invoke("CallMenu", 2f);
            anim.SetTrigger("Hit");
        }
    }

    void CallMenu()
    {
        GameManager.gm.EndRun();
    }

    public void IncreaseSpeed()
    {
        speed *= 1.02f;
        if (speed >= maxSpeed)
            speed = maxSpeed;
    }
}

