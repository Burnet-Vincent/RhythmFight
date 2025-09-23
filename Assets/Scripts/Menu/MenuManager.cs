using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class MenuManager : MonoBehaviour
{
    public GameObject[] playerPanels;
    
    private enum States {
        Disconnected,
        Connected,
        Ready
    }

    private States[] _playersState;
    private Image[] _playersBackgroundImage;

    private void Start()
    {
        _playersState = new States[playerPanels.Length];
        
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
        SetDisconnected(0);
    }
    
    public void Player2ReadyClicked()
    {
        SetDisconnected(1);
    }
    
    public void Player3ReadyClicked()
    {
        SetDisconnected(2);
    }
    
    public void Player4ReadyClicked()
    {
        SetDisconnected(3);
    }

    private void SetDisconnected(int index)
    {
        _playersState[index] = States.Disconnected;
        _playersBackgroundImage[index].color = new Color(1f, 1f, 1f, 0f);
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
}
