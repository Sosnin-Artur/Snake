using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeBody : MonoBehaviour
{
    [SerializeField] private Transform head;
    [SerializeField] private Transform tail;
    
    [SerializeField] private Transform bodyPrefab;    
    [SerializeField] private float circleDiameter;

    private List<Transform> units = new List<Transform>();
    private List<Vector3> positions = new List<Vector3>();
    
    public Color color;

    public void ChangeColor(Color color)
    {
        this.color = color;
        for (int i = 0, lengthI = units.Count; i < lengthI; ++i)
        {             
            units[i].GetChild(0).GetComponent<Renderer>().material.color = color;               
        }        
    }

    public void AddBody()
    {
        Transform unit = Instantiate(bodyPrefab, positions[positions.Count - 1], bodyPrefab.transform.rotation, head);        
        unit.GetChild(0).GetComponent<Renderer>().material.color = color;      
        units.Remove(tail);
        units.Add(unit);
        units.Add(tail);
        positions.Add(unit.position);
    }

    private void Awake()
    {
        positions.Add(head.position);
        positions.Add(tail.position);
        units.Add(tail);
    }

    private void FixedUpdate()
    {
        Move();
        Rotate();
    }

    private void Move()
    {
        float distance = (head.position - positions[0]).magnitude;

        if (distance > circleDiameter)
        {            
            Vector3 direction = (head.position - positions[0]).normalized;

            positions.Insert(0, positions[0] + direction * circleDiameter);
            positions.RemoveAt(positions.Count - 1);

            distance -= circleDiameter;
        }

        for (int i = 0; i < units.Count; i++)
        {            
            Vector3 pos = Vector3.Lerp(positions[i + 1], positions[i], distance / circleDiameter);            
            units[i].position = pos;
        }
    }

    private void Rotate()
    {
        units[0].LookAt(head);
        for (int i = 0; i < units.Count - 1; i++)
        {
            units[i + 1].LookAt(units[i]);            
        }
    }
}