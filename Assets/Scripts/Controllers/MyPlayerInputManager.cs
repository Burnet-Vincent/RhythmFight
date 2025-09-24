using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MyPlayerInputManager : MonoBehaviour
{
    public static MyPlayerInputManager Instance;
    
    [SerializeField] private int maxPlayerCount = 4;
    
    private PlayerInputManager _playerInputManager;

    private PlayerInput[] _activePlayers;

    private List<PlayerInput> _otherPlayers;
    

    private enum States
    {
        Menu,
        Game
    }
    
    private States _state = States.Menu;
    private InputDevice[] _inputDevices;
    [SerializeField] private List<string> _inputDeviceNames;

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

    private void Update()
    {
        _inputDevices = InputSystem.devices.ToArray();
        _inputDeviceNames = new List<string>();
        foreach (InputDevice device in _inputDevices)
        {
            _inputDeviceNames.Add(device.name);
        }
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
                
                if (_state == States.Menu)
                {
                    MenuManager.Instance.AddPlayer(i, playerInput);
                }

                break;
            }
        }

        if (!check)
        {
            _otherPlayers.Add(playerInput);
        }
    }
    
    public void OnLeft(PlayerInput playerInput)
    {
        print("Oui");
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
