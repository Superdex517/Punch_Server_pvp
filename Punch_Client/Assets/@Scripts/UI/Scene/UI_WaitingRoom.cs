using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Define;
using Object = UnityEngine.Object;

public class UI_WaitingRoom : UI_Scene
{
    private enum GameObjects
    {
        UI_WaitingPopup,
    }

    private enum GameButtons
    {
        MakeRoomButton,
        PrevButton,
        NextButton,
    }

    public int MaxPage { get; set; } // (roomcount / 3) = maxpage
    
    private int currentPage;
    private Vector3 targetPos;
    private float dragThreshould;

    [SerializeField] private Vector3 pageStep;
    [SerializeField] private RectTransform levelPagesRect;
    [SerializeField] private float tweenTime;
    [SerializeField] private LeanTweenType tweenType;

    protected override void Awake()
    {
        base.Awake();

        BindObjects(typeof(GameObjects));
        BindButtons(typeof(GameButtons));

        currentPage = 1;
        targetPos = levelPagesRect.localPosition;
        dragThreshould = Screen.width / 15;
        UpdateArrowButton();

        GetObject((int)GameObjects.UI_WaitingPopup).gameObject.SetActive(false);

        GetButton((int)GameButtons.MakeRoomButton).onClick.AddListener(() =>
        {
            Debug.Log("MakeRoom");
            Managers.Room.IsRoomManager = true;
            MakeRoom();
        });

        GetButton((int)GameButtons.PrevButton).onClick.AddListener(() =>
        {
            Debug.Log("prev");
            Prev();
        });

        GetButton((int)GameButtons.NextButton).onClick.AddListener(() =>
        {
            Debug.Log("next");
            Next();
        });
    }
 
    protected override void Start()
    {
        base.Start();

    }

    public void Next()
    {
        if (currentPage < MaxPage)
        {
            currentPage++;
            targetPos += pageStep;
            MovePage();
        }
    }

    public void Prev()
    {
        if (currentPage > 1)
        {
            currentPage--;
            targetPos -= pageStep;
            MovePage();
        }
    }

    void MovePage()
    {
        levelPagesRect.LeanMoveLocal(targetPos, tweenTime).setEase(tweenType);
        UpdateArrowButton();
    }

    void UpdateArrowButton()
    {
        GetButton((int)GameButtons.PrevButton).interactable = true;
        GetButton((int)GameButtons.NextButton).interactable = true;
        if (currentPage == 1) GetButton((int)GameButtons.PrevButton).interactable = false;
        else if (currentPage == MaxPage) GetButton((int)GameButtons.NextButton).interactable = false;
    }

    void MakeRoom()
    {
        GetObject((int)GameObjects.UI_WaitingPopup).gameObject.SetActive(true);

        C_MakeRoom makeRoom = new C_MakeRoom();
        Managers.Network.Send(makeRoom);

        //GetObject((int)GameObjects.WaitingPopup).gameObject.SetActive(false);
    }
}
