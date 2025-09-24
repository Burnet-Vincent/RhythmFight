using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;
    
    public GameObject[] playerPanels;
    
    private enum States {
        Disconnected,
        Connected,
        Ready
    }

    private States[] _playersState;
    private Image[] _playersBackgroundImage;
    private PlayerInput[] _playersInput;

    private void Awake()
    {
        if (Instance)
        {
            Destroy(Instance.gameObject);
        }
        
        Instance = this;
    }

    private void Start()
    {
        _playersState = new States[playerPanels.Length];
        _playersInput = new PlayerInput[playerPanels.Length];
        
        _playersBackgroundImage = new Image[playerPanels.Length];
        for (int i = 0; i < _playersBackgroundImage.Length; i++)
        {
            _playersBackgroundImage[i] = playerPanels[i].GetComponent<Image>();
        }

        for (int i = 0; i < playerPanels.Length; i++)
        {
            SetDisconnected(i);
        }
    }
    
    public void Player1ReadyClicked()
    {
        SetReady(0);
    }
    
    public void Player2ReadyClicked()
    {
        SetReady(1);
    }
    
    public void Player3ReadyClicked()
    {
        SetReady(2);
    }
    
    public void Player4ReadyClicked()
    {
        SetReady(3);
    }

    private void SetDisconnected(int index)
    {
        _playersState[index] = States.Disconnected;
        _playersBackgroundImage[index].color = new Color(1f, 1f, 1f, 0f);
        _playersInput[index] = null;
        SetActiveItem(index, 0);
    }
    
    private void SetButton(int index)
    {
        _playersState[index] = States.Connected;
        _playersBackgroundImage[index].color = new Color(1f, 1f, 1f, 1f);
        SetActiveItem(index, 1);
    }
    
    private void SetReady(int index)
    {
        _playersState[index] = States.Ready;
        _playersBackgroundImage[index].color = new Color(1f, 1f, 1f, 1f);
        SetActiveItem(index, 2);
    }

    private void SetActiveItem(int indexPlayer, int indexItem)
    {
        playerPanels[indexPlayer].transform.GetChild(0).gameObject.SetActive(indexItem == 0);
        playerPanels[indexPlayer].transform.GetChild(1).gameObject.SetActive(indexItem == 1);
        playerPanels[indexPlayer].transform.GetChild(2).gameObject.SetActive(indexItem == 2);
    }

    public void AddPlayer(int i, PlayerInput playerInput)
    {
        SetButton(i);
        _playersInput[i] = playerInput;
    }
}
