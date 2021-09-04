using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float forwardSpeed = 5.0f;
    [SerializeField] private float speedBoostModifier = 3.0f;
    [SerializeField] private float speedBoostDuration = 5.0f;
    [SerializeField] private float sensitivity = 10.0f;
    [SerializeField] private float maxSidewaysSpeed = 5.0f;
    [SerializeField] private int length = 1;
    [SerializeField] private int crystalsForFever = 3;

    [SerializeField] private Camera mainCamera;
    [SerializeField] private Transform head;
    [SerializeField] private LevelController levelController;
    
    private int _curCrystalCount = 0;
    private bool _isBoosted; 
    
    private Rigidbody _rb;
    private SnakeBody _snakeBody;    
    private GameManager _gameManager;

    private Vector2 touchLastPos;
    private float _sidewaysSpeed;
        
    public Color Color {get; private set;}

    public void ChangeColor(Color color)
    {        
        Color = color;
        head.GetChild(0).GetComponent<Renderer>().material.color = Color;   
        _snakeBody.ChangeColor(Color);
    }    

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _snakeBody = GetComponent<SnakeBody>();        
        _gameManager = GameObject.Find("Managers").GetComponent<GameManager>();
    }

    private void Start()
    {                        
        for (int i = 0; i < length; i++) 
        {
            _snakeBody.AddBody();
        }        
        ChangeColor(levelController.GetRandomColor());
    }

    private void Update()
    {        
        if (!_isBoosted && !GameManager.IsGameOver)
        {
            if (Input.GetMouseButtonDown(0))
            {
                touchLastPos = mainCamera.ScreenToViewportPoint(Input.mousePosition);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                _sidewaysSpeed = 0;                                   
                touchLastPos = new Vector2(0, 0);
            }
            else if (Input.GetMouseButton(0))
            {
                Vector2 delta = (Vector2)mainCamera.ScreenToViewportPoint(Input.mousePosition) - touchLastPos;            
                _sidewaysSpeed += delta.x * sensitivity;
                
                touchLastPos = mainCamera.ScreenToViewportPoint(Input.mousePosition);                                       
            }                
            float angleY = Mathf.Atan2(_sidewaysSpeed, forwardSpeed) * Mathf.Rad2Deg;            
            head.rotation = Quaternion.Euler(0, angleY, 0);
        }        
        else
        {
            head.rotation = Quaternion.identity;
        }
    }

    private void FixedUpdate()
    {
        if (GameManager.IsGameOver)
        {
            _rb.velocity = new Vector3(0, 0, 0);
            return;
        }
        if (_isBoosted)
        {
            _rb.velocity = new Vector3(0, 0, forwardSpeed * speedBoostModifier);
            
            _sidewaysSpeed = 0;
        }
        else
        {            
            if (Mathf.Abs(_sidewaysSpeed) > maxSidewaysSpeed)
            {
                _sidewaysSpeed = maxSidewaysSpeed * Mathf.Sign(_sidewaysSpeed);
            }         

            _rb.velocity = new Vector3(_sidewaysSpeed, 0, forwardSpeed);
            
            _sidewaysSpeed = 0;
        }
    }    

    private void OnCollisionEnter(Collision other) 
    {
        if (_isBoosted)
        {
            _snakeBody.AddBody();
            if (other.gameObject.CompareTag("Food"))
            {                
                Destroy(other.gameObject);
            }        
        }
        else
        {
            if (other.gameObject.CompareTag("Food"))
            {
                Eat(other.gameObject.GetComponent<Food>());
                Destroy(other.gameObject);
            }        

            if (other.gameObject.CompareTag("Obstacle"))
            {
                _gameManager.GameOver();
            }    
        }        
    }    

    private void OnTriggerEnter(Collider other) 
    {                
        if (other.gameObject.CompareTag("Finish"))
        {
            _gameManager.CompliteGame();
        }                 
    }    
    
    public void Collect()
    {
        _curCrystalCount++;
        GameManager.AddScore(1);
        
        if (_curCrystalCount == crystalsForFever)
        {
           _curCrystalCount = 0;
           SpeedBoost();           
        }
    }

    private void Eat(Food food)
    {
        if (Color == food.Color)
        {
            _curCrystalCount = 0;
            _snakeBody.AddBody();
        }
        else
        {
            GameObject.Find("Managers").GetComponent<GameManager>().GameOver();
        }
    }
       
    private void SpeedBoost()
    {
        if (!_isBoosted)
        {
            _isBoosted = true;        
            StartCoroutine(MoveToPosition(new Vector3(4.7f, transform.position.y, transform.position.z)));   
            StartCoroutine(FinishBoost());
        }        
    }       
    
    private IEnumerator MoveToPosition(Vector3 pos)
    {    
        _rb.velocity = new Vector3(0, 0, 0);
        while (Mathf.Abs(transform.position.x - pos.x) > 0.1f)
        {            
            transform.position = Vector3.MoveTowards(transform.position, pos, Time.time / (Time.deltaTime * forwardSpeed));
            yield return new WaitForFixedUpdate();
        }        
    }
    
    private IEnumerator FinishBoost()
    {        
        yield return new WaitForSeconds(speedBoostDuration);        
        _isBoosted = false;        
        _curCrystalCount = 0;
    }
}