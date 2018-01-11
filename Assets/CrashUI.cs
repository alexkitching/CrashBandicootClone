using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrashUI : MonoBehaviour
{
    [SerializeField]
    private RectTransform wumpaCountHolder;
    [SerializeField]
    private Text wumpaCountText;

    private bool WumpaCountShown;
    private float WumpaCountTime;
    [SerializeField]
    private Vector2 WumpaCountHiddenPos;
    [SerializeField]
    private Vector2 WumpaCountShownPos;

    [SerializeField]
    private RectTransform lifeCountHolder;
    [SerializeField]
    private Text lifeCountText;

    private bool LifeCountShown;
    private float LifeCountTime;
    [SerializeField]
    private Vector2 LifeCountHiddenPos;
    [SerializeField]
    private Vector2 LifeCountShownPos;

    [SerializeField]
    private float transitionSpeed;

    [SerializeField]
    private float defaultCountShowTime;
    public float DefaultCountShowTime
    {
        get { return defaultCountShowTime; }
    }


    void Start ()
    {
        WumpaCountShown = false;
        WumpaCountTime = 0f;
        LifeCountShown = false;
        LifeCountTime = 0f;

        ShowWumpaCountTimed(defaultCountShowTime);
        ShowLifeCountTimed(defaultCountShowTime);
	}
	
	void Update ()
    {
        if (WumpaCountShown)
        {
            if(wumpaCountHolder.anchoredPosition != WumpaCountShownPos)
            {
                wumpaCountHolder.anchoredPosition = Vector2.Lerp(wumpaCountHolder.anchoredPosition, WumpaCountShownPos, transitionSpeed * Time.deltaTime);
            }

            WumpaCountTime -= Time.deltaTime;
            if(WumpaCountTime <= 0)
            {
                WumpaCountShown = false;
            }
        }
        else
        {
            if(wumpaCountHolder.anchoredPosition != WumpaCountHiddenPos)
            {
                wumpaCountHolder.anchoredPosition = Vector3.Lerp(wumpaCountHolder.anchoredPosition, WumpaCountHiddenPos, transitionSpeed * Time.deltaTime);
            }
        }

        if(LifeCountShown)
        {
            if (lifeCountHolder.anchoredPosition != LifeCountShownPos)
            {
                lifeCountHolder.anchoredPosition = Vector2.Lerp(lifeCountHolder.anchoredPosition, LifeCountShownPos, transitionSpeed * Time.deltaTime);
            }

            LifeCountTime -= Time.deltaTime;
            if (LifeCountTime <= 0)
            {
                LifeCountShown = false;
            }
        }
        else
        {
            if (lifeCountHolder.anchoredPosition != LifeCountHiddenPos)
            {
                lifeCountHolder.anchoredPosition = Vector3.Lerp(lifeCountHolder.anchoredPosition, LifeCountHiddenPos, transitionSpeed * Time.deltaTime);
            }
        }
	}

    public void SetWumpaCountText(int a_wumpaCount)
    {
        wumpaCountText.text = a_wumpaCount.ToString();
    }



    public void ShowWumpaCountTimed(float a_time)
    {
        if (!WumpaCountShown)
        {
            WumpaCountShown = true;
            WumpaCountTime = a_time;
        }
        else if(WumpaCountShown)
        {
            WumpaCountTime = a_time;
        }
    }

    public void ShowWumpaCount()
    {
        if(!WumpaCountShown)
        {
            WumpaCountShown = true;
        }
    }

    public void HideWumpaCount()
    {
        if(WumpaCountShown)
        {
            WumpaCountShown = false;
        }
    }

    public void ToggleWumpaCount()
    {
        WumpaCountShown = !WumpaCountShown;
    }

    public void SetLifeCountText(int a_lifeCount)
    {
        lifeCountText.text = a_lifeCount.ToString();
    }

    public void ShowLifeCountTimed(float a_time)
    {
        if (!LifeCountShown)
        {
            LifeCountShown = true;
            LifeCountTime = a_time;
        }
        else if (LifeCountShown)
        {
            LifeCountTime = a_time;
        }
    }

    public void ShowLifeCount()
    {
        if (!LifeCountShown)
        {
            LifeCountShown = true;
        }
    }

    public void HideLifeCount()
    {
        if (LifeCountShown)
        {
            LifeCountShown = false;
        }
    }

    public void ToggleLifeCount()
    {
        LifeCountShown = !LifeCountShown;
    }
}
