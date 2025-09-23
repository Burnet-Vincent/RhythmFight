using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MyPlayerInputManager : MonoBehaviour
{
    public static MyPlayerInputManager Instance;
    
    [SerializeField] private int maxPlayerCount = 2;
    
    private PlayerInputManager _playerInputManager;

    private PlayerInput[] _activePlayers;

    private List<PlayerInput> _otherPlayers;
    
    [SerializeField] private int otherCount = 0, activeCount = 0;

    private enum States
    {
        Menu,
        Game
    }
    
    private States _state = States.Menu;

    private void Awake()
    {
        if (Instance)
        {
            Destroy(Instance.gameObject);
        }
        
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        _playerInputManager = GetComponent<PlayerInputManager>();
        
        _activePlayers = new PlayerInput[maxPlayerCount];
        _otherPlayers = new List<PlayerInput>();
        
    }

    public void OnJoined(PlayerInput playerInput)
    {
        
        playerInput.gameObject.SetActive(false);
        
        bool check = false;
        
        for (int i = 0; i < _activePlayers.Length; i++)
        {
            if (!_activePlayers[i])
            {
                _activePlayers[i] = playerInput;
                check = true;
                activeCount++;

                break;
            }
        }

        if (!check)
        {
            _otherPlayers.Add(playerInput);
            otherCount++;
        }
    }
    
    public void OnLeft(PlayerInput playerInput)
    {
        bool check = false;

        if (_state == States.Menu)
        {
            Destroy(playerInput);
            return;
        }

        for (int i = 0; i < _activePlayers.Length; i++)
        {
            if (_activePlayers[i] == playerInput)
            {
                _activePlayers[i] = null;
                check = true;
                activeCount--;

                break;
            }
        }
        
        if (!check)
        {
            for (int i = 0; i < _otherPlayers.Count; i++)
            {
                if (_otherPlayers[i] == playerInput)
                {
                    _otherPlayers[i] = null;
                    otherCount--;
                    break;
                }
            }
        }
    }

    public void OnMenuJoined()
    {
        
    }

    public void OnGameJoined()
    {
        
    }
}
