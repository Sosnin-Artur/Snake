using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodPlane : Plane
{       
    [SerializeField] private int colorPerPlane = 2;      

    [SerializeField] private int minFoodPlaces = 2; 
    [SerializeField] private int maxFoodPlaces = 10; 
    
    [SerializeField] private float placementHalfWidth = 4.0f; 
    [SerializeField] private float placementHalfHeight = 13.0f; 

    [SerializeField] private int foodAmountPerGroup = 4; 
    [SerializeField] private float foodPlacementRadius = 2.0f; 
    
    [SerializeField] private GameObject foodPrefab;
    [SerializeField] private PlayerController player;

    private LevelController _levelController;
    
    public override void Place()
    {
        for (int i = 3; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        int count = Random.Range(minFoodPlaces, maxFoodPlaces);        
        
        List<Color> colors = new List<Color>();
        colors.Add(player.Color);
        
        for (int i = 1; i < colorPerPlane; i++)
        {
            Color color = _levelController.GetRandomColor();        
            while(colors.Contains(color))
            {
                color = _levelController.GetRandomColor();     
            }       
            colors.Add(color);
        }

        for (int i = 0; i < count; i++)
        {
            float posX = Random.Range(-placementHalfWidth, placementHalfWidth);
            float posZ = Random.Range(-placementHalfHeight, placementHalfHeight);            
            CreateGroup(transform.position.x + posX, 1, transform.position.z + posZ,
                colors[Random.Range(0, colors.Count)]);
        }
    }

    private void Awake()
    {
        _levelController = GameObject.Find("Controllers").GetComponent<LevelController>();        
    }    
    
    private void CreateGroup(float x, float y, float z, Color color)
    {        
        for (int i = 0; i < foodAmountPerGroup; i++)
        {            
            Vector2 posInCircle = Random.insideUnitCircle * foodPlacementRadius;
            Food food = 
                Instantiate(foodPrefab, new Vector3(posInCircle.x + x, y, 
                z + posInCircle.y),Quaternion.identity, transform).GetComponent<Food>();

            food.Color = color;
        }
    }
}
