using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    [SerializeField] private float oscilationForce = 1.0f;
    [SerializeField] private float oscilationRadius = 1.0f;
    
    private Rigidbody _rb;    
    private Vector3 _startPos;    
    private LevelController _levelController;
    private Color _color;
    public Color Color 
    {
        get
        {
            return _color;
        } 
        set
        {            
            ChangeColor(value);
        }
    }    

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();                
        _levelController = GameObject.Find("Controllers").GetComponent<LevelController>();                
        _startPos = transform.position;          
    }
    
    public void ChangeColor(Color color)
    {
        gameObject.GetComponent<Renderer>().material.color = color;
        _color = color;
    }

    private void FixedUpdate()
    {
        Vector2 direction = Random.insideUnitCircle;
        _rb.AddForce(new Vector3(direction.x, 0, direction.y)* oscilationForce);
        
        Vector3 curPos = transform.position;

        if (Vector3.Distance(curPos, _startPos) > oscilationRadius)
        {
           _rb.velocity = new Vector3(0, 0, 0);
        }
    }
}
