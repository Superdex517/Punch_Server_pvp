using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Define;
using Object = UnityEngine.Object;

public class UI_WaitingPopup : UI_Popup
{
    private enum GameObjects
    {
    }

    private enum Texts
    {
        RoomTitle,
        HostId,
        QuestId,
    }

    private enum Buttons
    {
        LeaveButton,
        StartButton,
        ReadyButton,
    }

    private void OnEnable()
    {
        if (Managers.Room.IsHost)
        {
            GetButton((int)Buttons.ReadyButton).gameObject.SetActive(false);
            GetButton((int)Buttons.StartButton).gameObject.SetActive(true);

            //GetButton((int)Buttons.StartButton).interactable = false;
        }
        else
        {
            GetButton((int)Buttons.ReadyButton).gameObject.SetActive(true);
            GetButton((int)Buttons.StartButton).gameObject.SetActive(false);
        }

        if (Managers.Room.IsHost)
        {
            GetButton((int)Buttons.StartButton).onClick.AddListener(() =>
            {
                Debug.Log("startbtn");

                GameStart();
            });
        }
        else
        {
            GetButton((int)Buttons.ReadyButton).onClick.AddListener(() =>
            {
                Debug.Log("readybtn");
                Managers.Object.readyPlayer(true);
            });
        }

        GetButton((int)Buttons.LeaveButton).onClick.AddListener(() =>
        {
            LeaveRoom();
        });
    }

    private void OnDisable()
    {
        
    }

    protected override void Awake()
    {
        BindObjects(typeof(GameObjects));
        BindTexts(typeof(Texts));
        BindButtons(typeof(Buttons));
    }

    private void LeaveRoom()
    {
        Debug.Log("Leave");

        S_LeaveWaitingRoom leaveWaitingRoom = new S_LeaveWaitingRoom();
        
        Managers.Network.Send(leaveWaitingRoom);

        ClosePopupUI();

        Managers.Room.IsHost = false;
    }

    public void RoomIsReady()
    {
        if (Managers.Room.IsHost)
        {
            GetButton((int)Buttons.StartButton).interactable = true;
        }
    }

    public void GameStart()
    {
        {
            C_GameStart startGame = new C_GameStart();
            startGame.RoomInfo = Managers.MyPlayer.RoomInfo;
            Managers.Network.Send(startGame);
        }
    }
}
