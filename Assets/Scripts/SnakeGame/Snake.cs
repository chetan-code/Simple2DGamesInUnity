using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Snake : MonoBehaviour
{

    public enum PlayerDirection {
        LEFT,
        RIGHT,
        UP,
        DOWN,
    }

    public PlayerDirection playerDirection;

    public GameObject snakeHeadPrefab;
    public GameObject snakeTailPrefab;
    public GameObject food;

    public AudioClip eatSound;
    public AudioClip deathSound;
    public TextMeshProUGUI scoreText;
    

    private float screenWidth;
    private float screenHeight;
    private float startTime;
    private Transform foodOnScreen;
    private GameObject snakeHead;
    private AudioSource audioSource;

    Vector3 leftEnd;
    Vector3 topEnd;


    int xspeed;
    int yspeed;
    int total;
    public List<GameObject> snakeBody;
    Camera cam;

    int top;
    int bottom;
    int right;
    int left;


    private Vector2 lastPosition;

    public Snake() {
        xspeed = 1;
        yspeed = 0;
        total = 0;
        snakeBody = new List<GameObject>();
        playerDirection = PlayerDirection.LEFT;
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        cam = Camera.main;

        screenWidth = Screen.width;
        screenHeight = Screen.height;

        snakeBody = new List<GameObject>();
    

        Vector3 center = new Vector3();

        Debug.Log("screenWidth : " + screenWidth);
        Debug.Log("screenHeight : " + screenHeight);

        center = cam.ScreenToWorldPoint(new Vector3(screenWidth/2, screenHeight/2, cam.nearClipPlane));

        Debug.Log("Center is : " + center);

        leftEnd = new Vector3();

        leftEnd = cam.ScreenToWorldPoint(new Vector3(screenWidth , screenHeight / 2, cam.nearClipPlane));
        Debug.Log("LeftEnd is : " + leftEnd);

        topEnd = cam.ScreenToWorldPoint(new Vector3(screenWidth/2, screenHeight, cam.nearClipPlane));
        Debug.Log("TopEnd is : " + topEnd);

        snakeHead = Instantiate(snakeHeadPrefab, transform);
        startTime = Time.time;
        snakeBody.Add(snakeHead);
        CreateFood();
        scoreText.text = total.ToString();
    }


    void CreateFood() {

        top = Mathf.RoundToInt(topEnd.y);
        bottom = -top;
        left = Mathf.RoundToInt(leftEnd.x);
        right = -left;

        int RandomX = Random.Range(right + 1, left -2);
        int RandomY = Random.Range(bottom +1 , top -2);

        Vector3 positionOfFood = new Vector3(RandomX, RandomY);

        if (foodOnScreen == null)
        {
            GameObject foodGo = Instantiate(food, positionOfFood, Quaternion.identity);
            foodOnScreen = foodGo.transform;
            
        }
        else {
            foodOnScreen.transform.position = positionOfFood;
        }

    }

    private void Update()
    {
        startTime += Time.deltaTime;

        if (startTime > 0.2)
        {
            UpdatePosition();
            //add tail
            Debug.Log("Tail Count : " + snakeBody.Count);
            for (int i = 1; i < snakeBody.Count; i++)
            {
                UpdateSnakePosition(i);
                SnakeDeath(i);
            }




            startTime = 0;
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (playerDirection == PlayerDirection.DOWN) { return; }
            SetDirection(0, 1);
            snakeBody[0].transform.rotation = Quaternion.Euler(0, 0, 90);
            playerDirection = PlayerDirection.UP;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (playerDirection == PlayerDirection.UP) { return; }
            SetDirection(0, -1);
            snakeBody[0].transform.rotation = Quaternion.Euler(0, 0, -90);
            playerDirection = PlayerDirection.DOWN;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (playerDirection == PlayerDirection.LEFT) { return; }
            SetDirection(1, 0);
            snakeBody[0].transform.rotation = Quaternion.Euler(0, 0, 0);
            playerDirection = PlayerDirection.RIGHT;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (playerDirection == PlayerDirection.RIGHT) { return; }
            SetDirection(-1, 0);
            snakeBody[0].transform.rotation = Quaternion.Euler(0, 0, 180);
            playerDirection = PlayerDirection.LEFT;
        }



        //eat the food
        if (Vector3.Distance(snakeHead.transform.position, foodOnScreen.position) < 1) {
            CreateFood();
            AddTail();
            total++;
            scoreText.text = total.ToString();
        }

    }


    void UpdatePosition() {

         lastPosition = snakeBody[0].transform.position;
        

        float newX = snakeBody[0].transform.position.x + xspeed;
        float newY = snakeBody[0].transform.position.y + yspeed;

        //constrain to screensize
        newX = Mathf.Clamp(newX, -leftEnd.x , leftEnd.x);
        newX = Mathf.RoundToInt(newX);
        newY = Mathf.Clamp(newY, -topEnd.y, topEnd.y);
        newY = Mathf.RoundToInt(newY);

        Vector2 newPosition = new Vector2(newX, newY);

        snakeBody[0].transform.position = newPosition;
        
    }

    void UpdateSnakePosition(int index) {

        Vector3 newPosition = snakeBody[index].transform.position;
        snakeBody[index].transform.position = lastPosition;
        lastPosition = newPosition;

    }

    void SnakeDeath(int index) {
        float distance = Vector2.Distance(snakeBody[0].transform.position, snakeBody[index].transform.position);
        if (distance < 1) {
            Debug.Log("Game over");
            GameOver();
        }
    }

    void GameOver() {
        for (int i = 1; i < snakeBody.Count; i++) {
            Destroy(snakeBody[i]);
        }
        snakeBody.RemoveRange(1, snakeBody.Count - 1);
        audioSource.PlayOneShot(deathSound);
    }

    public void SetDirection(int x, int y) {
        xspeed = x;
        yspeed = y;
    }

    public void AddTail() {
        audioSource.PlayOneShot(eatSound);
        GameObject newBlock = Instantiate(snakeTailPrefab, lastPosition, Quaternion.identity);
        snakeBody.Add(newBlock);
    }

}

