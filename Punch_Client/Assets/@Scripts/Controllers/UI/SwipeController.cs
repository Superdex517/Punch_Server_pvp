using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SwipeController : MonoBehaviour/*, IEndDragHandler*/
{
    ////room count
    //public int maxPage; // (roomcount / 3) = maxpage
    //int currentPage;
    //Vector3 targetPos;
    //[SerializeField] Vector3 pageStep;
    //[SerializeField] RectTransform levelPagesRect;

    //[SerializeField] float tweenTime;
    //[SerializeField] LeanTweenType tweenType;
    //float dragThreshould;

    //[SerializeField] Button prevBtn, nextBtn;

    //private void Awake()
    //{
    //    currentPage = 1;
    //    targetPos = levelPagesRect.localPosition;
    //    dragThreshould = Screen.width / 15;
    //    UpdateArrowButton();
    //}

    //public void Next()
    //{
    //    if(currentPage < maxPage)
    //    {
    //        currentPage++;
    //        targetPos += pageStep;
    //        MovePage();
    //    }
    //}

    //public void Prev()
    //{
    //    if (currentPage > 1)
    //    {
    //        currentPage--;
    //        targetPos -= pageStep;
    //        MovePage();
    //    }
    //}

    //void MovePage()
    //{
        //levelPagesRect.LeanMoveLocal(targetPos, tweenTime).setEase(tweenType);
        //UpdateArrowButton();
    //}

    //public void OnEndDrag(PointerEventData eventData)
    //{
    //    if(Mathf.Abs(eventData.position.x - eventData.pressPosition.x) > dragThreshould)
    //    {
    //        if (eventData.position.x > eventData.pressPosition.x) Prev();
    //        else Next();
    //    }
    //    else
    //    {
    //        MovePage();
    //    }
    //}

    //void UpdateArrowButton()
    //{
    //    nextBtn.interactable = true;
    //    prevBtn.interactable = true;
    //    if (currentPage == 1) prevBtn.interactable = false;
    //    else if (currentPage == maxPage) nextBtn.interactable = false;
    //}
}
