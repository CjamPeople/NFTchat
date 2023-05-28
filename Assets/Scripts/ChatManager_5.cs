using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class ChatManager_5 : MonoBehaviour
{
    public GameObject YellowArea, WhiteArea, DateArea;
    public RectTransform ContentRect;
    public Scrollbar scrollBar;
    AreaScript LastArea;

    public void Chat(bool isSend, string text, string user, Texture picture)
    {
        if(text.Trim() == "") return;

        bool isBottom = scrollBar.value <= 0.00001f;

        AreaScript Area = Instantiate(isSend ? YellowArea : WhiteArea).GetComponent<AreaScript>();
        Area.transform.SetParent(ContentRect.transform, false);
        Area.BoxRect.sizeDelta = new Vector2(600, Area.BoxRect.sizeDelta.y);
        Area.TextRect.GetComponent<Text>().text = text;
        Fit(Area.BoxRect);

        float X = Area.TextRect.sizeDelta.x + 42;
        float Y = Area.TextRect.sizeDelta.y;
        if(Y>49)
        {
            for(int i=0;i<200;i++)
            {
                Area.BoxRect.sizeDelta = new Vector2(X-i*2,Area.BoxRect.sizeDelta.y);
                Fit(Area.BoxRect);

                if(Y!=Area.TextRect.sizeDelta.y)
                {
                    Area.BoxRect.sizeDelta = new Vector2(X-(i*2) +2, Y);
                    break;
                }
            }
        }
        else Area.BoxRect.sizeDelta = new Vector2(X,Y);

        DateTime t = DateTime.Now;
        Area.Time = t.ToString("yyyy-MM-dd-HH-mm");
        Area.User = user;

        int hour = t.Hour;
        if(t.Hour == 0) hour =12;
        else if(t.Hour > 12) hour -= 12;
        Area.TimeText.text = hour+ ":" + t.Minute.ToString("D2") +(t.Hour > 12 ? "PM" : "AM");

        bool isSame = LastArea != null && LastArea.Time == Area.Time && LastArea.User == Area.User;
        if(isSame) LastArea.TimeText.text = "";
        
        if(!isSend)
        {
            Area.UserImage.gameObject.SetActive(!isSame);
            Area.UserText.gameObject.SetActive(!isSame);
            Area.UserText.text = Area.User;
        }

        if(LastArea != null && LastArea.Time.Substring(0,10) != Area.Time.Substring(0,10))
        {
            Transform CurDateArea = Instantiate(DateArea).transform;
            CurDateArea.SetParent(ContentRect.transform,false);
            CurDateArea.SetSiblingIndex(CurDateArea.GetSiblingIndex() - 1 );

            string week = "";
            switch (t.DayOfWeek)
            {
                case DayOfWeek.Sunday : week = "Sun"; break;
                case DayOfWeek.Monday : week = "Mon"; break;
                case DayOfWeek.Tuesday : week = "Tue"; break;
                case DayOfWeek.Wednesday : week = "Wed"; break;
                case DayOfWeek.Thursday : week = "Thu"; break;
                case DayOfWeek.Friday : week = "Fri"; break;
                case DayOfWeek.Saturday : week = "Sat"; break;
            }
            CurDateArea.GetComponent<AreaScript>().DateText.text = t.Year + "년" + t.Month + "월" + t.Day + "일" + week + "요일";
        }

        Fit(Area.BoxRect);
        Fit(Area.AreaRect);
        Fit(ContentRect);
        LastArea = Area;
    }

    void Fit(RectTransform Rect) => LayoutRebuilder.ForceRebuildLayoutImmediate(Rect);
}
