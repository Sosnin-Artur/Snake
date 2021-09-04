using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI crystalCountText;    
    
    private static TextMeshProUGUI _crystalCountText;        

    public static bool IsGameOver {get; private set;}
    public static int CrystalCount {get; private set;}    

    public static void AddScore(int value)
    {
        CrystalCount += value;
        _crystalCountText.text = CrystalCount.ToString();        
    }
    
    public void GameOver()
    {
        StopAllCoroutines();
        StartCoroutine(StartNewGame());
    }

    public void CompliteGame()
    {
        StopAllCoroutines();
        StartCoroutine(StartNewGame());
    }

    private void Awake()
    {                
        _crystalCountText = crystalCountText;
        AddScore(0);
    }

    private IEnumerator StartNewGame()
    {        
        IsGameOver = true;
        yield return new WaitForSeconds(2);        
        IsGameOver = false;
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
    }
}
