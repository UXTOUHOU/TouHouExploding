using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class UIEventListener : UnityEngine.EventSystems.EventTrigger
{
    public delegate void UIEventHandler(GameObject go);
    public UIEventHandler onClick;
    public UIEventHandler onDown;
    public UIEventHandler onEnter;
    public UIEventHandler onExit;
    public UIEventHandler onUp;
    public UIEventHandler onSelect;
    public UIEventHandler onUpdateSelect;

    public static UIEventListener Get(GameObject go)
    {
        UIEventListener listener = go.GetComponent<UIEventListener>();
        if (listener == null) listener = go.AddComponent<UIEventListener>();
        return listener;
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        if (onClick != null) onClick(gameObject);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (onDown != null) onDown(gameObject);
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (onEnter != null) onEnter(gameObject);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        if (onExit != null) onExit(gameObject);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        if (onUp != null) onUp(gameObject);
    }

    public override void OnSelect(BaseEventData eventData)
    {
        if (onSelect != null) onSelect(gameObject);
    }
}
