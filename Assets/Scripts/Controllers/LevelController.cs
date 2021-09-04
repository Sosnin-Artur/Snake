using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    [SerializeField] private int controlledPlanesCount = 3;
    [SerializeField] private float startPosZ = 5.0f;    
    [SerializeField] private float levelDuration = 40.0f;    

    [SerializeField] private PlayerController player;

    [SerializeField] private Color[] colors;
    [SerializeField] private Plane[] planes;
    [SerializeField] private float[] lengthes;
    
    [SerializeField] private GameObject finishPlane;
    [SerializeField] private float finishLength;

    private float _curLength;
    private float _prevLength;    
    private bool _canPlace;
    private List<Plane> _placed;

    public Color GetRandomColor()
    {
        return colors[Random.Range(0, colors.Length)];
    }
    
    private void Start()
    {                               
        _curLength = startPosZ;
        _prevLength = 0;
        _canPlace = true;
        _placed = new List<Plane>();
        
        for (int i = 0; i < controlledPlanesCount; i++)
        {
            PlacePlane();
        }    
        StartCoroutine(FinishGame());    
    }    
    
    private void Update()
    {
        if (player.transform.position.z >= _placed[1].transform.position.z && _canPlace)
        {
            _placed.Remove(_placed[0]);
            PlacePlane();
        }
    }

    private void PlacePlane()
    {                
        if (_canPlace)
        {
            int index = Random.Range(0, planes.Length);
            Plane plane = planes[index];            
            while (_placed.Contains(plane))
            {
                index = Random.Range(0, planes.Length);
                plane = planes[index];            
            }
            _placed.Add(plane);
            _curLength += lengthes[index] / 2 + _prevLength / 2;
            plane.gameObject.transform.position = new Vector3(4.7f, 0, _curLength);                    
            _prevLength = lengthes[index];
            plane.Place();                    
        }                
    }

    private IEnumerator FinishGame()
    {
        yield return new WaitForSeconds(levelDuration);
        _canPlace = false;

        _curLength += finishLength / 2 + _prevLength / 2;
        finishPlane.transform.position = new Vector3(4.7f, 0, _curLength);                    
        _prevLength = finishLength;
    }
}
