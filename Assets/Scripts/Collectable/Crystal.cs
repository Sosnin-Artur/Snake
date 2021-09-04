using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal : MonoBehaviour
{
    [SerializeField] private float rotationSpeed;

    private Renderer _renderer;
    private Collider _collider;

    private void Awake()
    {
        _renderer = transform.GetChild(0).GetComponent<Renderer>();
        _collider = GetComponent<Collider>();
    }

    private void Update()
    {
        transform.Rotate(0, rotationSpeed, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().Collect();
            _renderer.enabled = false;
            _collider.enabled = false;
            StartCoroutine(BackToGame());
        }
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
