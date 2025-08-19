using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;

public static class HoldToSee
{
    [SerializeField] public static Image[] images;
    [SerializeField] public static TextMeshProUGUI[] texts;

    public static void Initialize()
    {
        images = ScreenManager.Instance().gameObject.GetComponentsInChildren<Image>(includeInactive:true);
        texts = ScreenManager.Instance().gameObject.GetComponentsInChildren<TextMeshProUGUI>(includeInactive:true);
    }
    

    public static void FadeOut()
    {
        if (IsPointerOverUIObject())
        {
            return;
        }
        
        Sequence seq = DOTween.Sequence();
        
        foreach (var image in images)
        {
            seq.Join(image.DOFade(0f, 0.5f).SetEase(Ease.InOutSine));
        }
        
        foreach (var text in texts)
        {
            seq.Join(text.DOFade(0f, 0.5f).SetEase(Ease.InOutSine));
        }
    }
    
    public static void FadeIn()
    {
        if (IsPointerOverUIObject())
        {
            return;
        }
        Sequence seq = DOTween.Sequence();
        
        foreach (var image in images)
        {
            seq.Join(image.DOFade(1f, 0.5f).SetEase(Ease.InOutSine));
        }
        
        foreach (var text in texts)
        {
            seq.Join(text.DOFade(1f, 0.5f).SetEase(Ease.InOutSine));
        }
    }

    private static bool IsPointerOverUIObject() {
        if (EventSystem.current == null)
        {
            Debug.Log("Event system does not exist!");
            return false;
        }
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 1;
    }
}
