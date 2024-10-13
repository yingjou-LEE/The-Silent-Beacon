using UnityEngine;
using System;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    public event Action<Vector2> OnWASD;
    public event Action<Vector2> OnArrowKeys;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        HandleWASDInput();
        HandleArrowKeysInput();
    }

    private void HandleWASDInput()
    {
        float moveX = Input.GetAxis("HorizontalWASD");
        float moveY = Input.GetAxis("VerticalWASD");
        
        var input = new Vector2(moveX, moveY);
        
        OnWASD?.Invoke(input);
    }
    
    private void HandleArrowKeysInput()
    {
        float moveX = Input.GetAxis("HorizontalArrow");
        float moveY = Input.GetAxis("VerticalArrow");
        
        var input = new Vector2(moveX, moveY);
        
        OnArrowKeys?.Invoke(input);
    }
}