using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    public Chapter CurrentChapter { get; private set; }
    
    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}