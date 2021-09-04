using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{    
    private Renderer _renderer;
    private Collider _collider;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _collider = GetComponent<Collider>();
    }

    private void OnCollisionEnter(Collision other)
    {        
        if (GameManager.IsGameOver) return;
        
        _renderer.enabled = false;
        _collider.enabled = false;
        StartCoroutine(BackToGame());
    }

    private IEnumerator BackToGame()
    {                
        yield return new WaitForSeconds(1.0f);
        
        if (!GameManager.IsGameOver)
        {
            _renderer.enabled = true;
            _collider.enabled = true;
        }
    }
}
