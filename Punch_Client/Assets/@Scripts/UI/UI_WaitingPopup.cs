using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Define;
using Object = UnityEngine.Object;

public class UI_WaitingPopup : UI_Base
{
    
    private enum GameObjects
    {
    }

    private enum Texts
    {
    }

    private enum Buttons
    {
        LeaveButton,
        StartButton,
        ReadyButton,
    }

    protected override void Awake()
    {
        BindObjects(typeof(GameObjects));
        BindTexts(typeof(Texts));
        BindButtons(typeof(Buttons));

        GetButton((int)Buttons.LeaveButton).onClick.AddListener(() =>
        {
            Debug.Log("Leave");
        });

        if (Managers.Room.IsRoomManager)
        {
            GetButton((int)Buttons.ReadyButton).gameObject.SetActive(false);
            if (!Managers.Room.IsPlayerReady)
                GetButton((int)Buttons.StartButton).interactable = false;
            else
            {
                GetButton((int)Buttons.StartButton).interactable = true;
                GetButton((int)Buttons.StartButton).onClick.AddListener(() =>
                {
                    Debug.Log("next");
                });
            }
 

        }
        else
        {
            GetButton((int)Buttons.StartButton).gameObject.SetActive(false);
            GetButton((int)Buttons.ReadyButton).onClick.AddListener(() =>
            {
                Debug.Log("next");
            });
        }

        GetButton((int)Buttons.LeaveButton).onClick.AddListener(() =>
        {
            Managers.Room.IsRoomManager = false;

            //TODO : LeaveRoomPacket
            this.gameObject.SetActive(false);
            
        });
    }
}
