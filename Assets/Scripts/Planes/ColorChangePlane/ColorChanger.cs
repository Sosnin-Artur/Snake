using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChanger : MonoBehaviour
{    
    [SerializeField] private PlayerController player;
    
    private LevelController _levelController;
    
    public Color Color {get; private set;}
        
    public void ChangeColor()
    {        
        Color = _levelController.GetRandomColor();        
        while(Color == player.Color)
        {
            Color = _levelController.GetRandomColor();     
        }       
        gameObject.GetComponent<Renderer>().material.color = Color;
        
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.GetComponent<ParticleSystem>().GetComponent<Renderer>().material.color = Color;
        }
    }

    private void Awake()
    {
        _levelController = GameObject.Find("Controllers").GetComponent<LevelController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<PlayerController>().ChangeColor(Color);
    }
}
