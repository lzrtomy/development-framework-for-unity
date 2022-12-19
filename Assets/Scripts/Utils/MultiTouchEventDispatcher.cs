using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
//using System;

public class MultiTouchEventData
{
    public Vector2 pStart;
    public Vector2 pCurrent;
    public Vector2 pDelta;
    public float dStart;
    public float dCurrent;
    public float dDelta;
    public GameObject sender;
    public RaycastResult currentRaycast;
}

public class MultiTouchEventDispatcher : MonoBehaviour, IDragHandler, IBeginDragHandler, IPointerDownHandler, IPointerUpHandler
{
    private static MultiTouchEventDispatcher m_Instance = null;

    public System.Action<MultiTouchEventData> OnClickEvent;
    public System.Action<MultiTouchEventData> OnDragEvent;
    public System.Action<MultiTouchEventData> OnBeginDragEvent;
    public System.Action<MultiTouchEventData> OnEndDragEvent;
    public System.Action<MultiTouchEventData> OnPinchEvent;
    public System.Action<MultiTouchEventData> OnBeginPinchEvent;
    public System.Action<MultiTouchEventData> OnEndPinchEvent;
    public System.Action<MultiTouchEventData> OnSweepEvent;
    public System.Action<MultiTouchEventData> OnHoldEvent;
    public System.Action<MultiTouchEventData> OnReleaseEvent;

    private class TouchInfo
    {
        public int touchId;
        public Vector2 startPos;
        public Vector2 lastPos;
    }

    private enum TouchState
    {
        Idle,
        Hold,
        Drag,
        Pinch
    }

    private List<TouchInfo> touchInfoList;

    private Vector2 dragDelta;
    private float dragDeltaTime;
    private float dragAcc;
    private float lastDragTime;
    private MultiTouchEventData _cachedEventData;

    private float swipeDeltaThreshold;
    private float swipeReleaseTimeThreshold;
    private TouchState state;
    private Coroutine holdCheck;

    private Vector2 pinchPosCursor;
    private float pinchDistCursor;

    public static MultiTouchEventDispatcher Instance { get { return m_Instance; } }

    protected void Awake()
    {
        if (m_Instance != null) 
        {
            Destroy(gameObject);
            return;
        }
        m_Instance = this;


        swipeDeltaThreshold = 5;
        swipeReleaseTimeThreshold = 0.1f;

        _cachedEventData = new MultiTouchEventData();
        _cachedEventData.sender = gameObject;
        touchInfoList = new List<TouchInfo>();

        enabled = true;
    }

    IEnumerator holdTrigger()
    {
        yield return new WaitForSeconds(0.033f);

        if(state == TouchState.Idle)
        {
            state = TouchState.Hold;
            OnHoldEvent?.Invoke(_cachedEventData);
            //eventDelegate.onHold(_cachedEventData);
        }
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        _cachedEventData.currentRaycast = eventData.pointerPressRaycast;

        if (!enabled) return;

        int pointerCount = touchInfoList.Count;

        switch (pointerCount)
        {
            case 0:
                {
                    addTouchInfo(eventData);

                    dragDelta = new Vector2(0, 0);
                    dragDeltaTime = 0;
                    dragAcc = 0;
                    lastDragTime = 0;

                    _cachedEventData.pStart = eventData.position;

                    if(holdCheck != null)
                    {
                        StopCoroutine(holdCheck);
                    }

                    holdCheck = StartCoroutine(holdTrigger());

                    break;
                }
            case 1:
                {
                    if(getTouchInfo(eventData.pointerId) == null)
                    {
                        addTouchInfo(eventData);

                        dragDelta = new Vector2(0, 0);
                        dragDeltaTime = 0;
                        dragAcc = 0;
                        lastDragTime = 0;

                        TouchInfo otherInfo = getTheOther(eventData.pointerId);
                        otherInfo.startPos = otherInfo.lastPos;
                    }

                    break;
                }
            default:
                break;
        }
    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {
        _cachedEventData.currentRaycast = eventData.pointerPressRaycast;

        if (!enabled) return;

        TouchInfo info = getTouchInfo(eventData.pointerId);

        if (info != null)
        {
            int pointerCount = touchInfoList.Count;
            switch (pointerCount)
            {
                case 1:
                    {
                        if(holdCheck != null)
                        {
                            StopCoroutine(holdCheck);
                        }

                        if (state == TouchState.Idle)
                        {
                            _cachedEventData.pStart = eventData.position;
                            OnHoldEvent?.Invoke(_cachedEventData);
                            //eventDelegate.onHold(_cachedEventData);
                        }

                        if (state == TouchState.Drag)
                        {
                            if (dragAcc > 0 && dragDelta.magnitude > swipeDeltaThreshold && (Time.time - lastDragTime) < swipeReleaseTimeThreshold)
                            {
                                _cachedEventData.pStart = eventData.position;
                                _cachedEventData.pDelta = dragDelta / dragDeltaTime;
                                OnSweepEvent?.Invoke(_cachedEventData);
                                //eventDelegate.onSwipe(_cachedEventData);
                            }
                            else
                            {
                                _cachedEventData.pStart = eventData.position;
                                OnEndDragEvent?.Invoke(_cachedEventData);
                                //eventDelegate.onRelease(_cachedEventData);
                            }
                        }
                        else
                        {
                            _cachedEventData.pStart = eventData.position;
                            OnReleaseEvent?.Invoke(_cachedEventData);
                            //eventDelegate.onRelease(_cachedEventData);
                        }

                        if (state == TouchState.Hold || state == TouchState.Idle)
                        {
                            _cachedEventData.pStart = eventData.position;
                            OnClickEvent?.Invoke(_cachedEventData);
                            //eventDelegate.onClick(_cachedEventData);
                        }

                        state = TouchState.Idle;

                        break;
                    }
                case 2:
                    {
                        TouchInfo otherInfo = getTheOther(eventData.pointerId);
                        otherInfo.startPos = otherInfo.lastPos;

                        if (state == TouchState.Pinch)
                        {
                            _cachedEventData.pStart = otherInfo.startPos;
                            _cachedEventData.pDelta = new Vector2(0, 0);
                            OnEndPinchEvent?.Invoke(_cachedEventData);
                            //eventDelegate.onPinchEnd(_cachedEventData);
                            state = TouchState.Drag;
                        }

                        break;
                    }
                default:
                    break;
            }

            touchInfoList.Remove(info);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Drag");
        TouchInfo info = getTouchInfo(eventData.pointerId);

        if (info != null)
        {
            info.startPos = info.lastPos = eventData.position;
        }

        OnDrag(eventData);
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        _cachedEventData.currentRaycast = eventData.pointerPressRaycast;

        if (!enabled) return;

        TouchInfo info = getTouchInfo(eventData.pointerId);

        if (info != null)
        {
            if (touchInfoList.Count == 2)
            {
                // send pinch event
                TouchInfo theOther = getTheOther(eventData.pointerId);

                Vector2 startCenter = (theOther.startPos + info.startPos) / 2;
                Vector2 lastCenter = (theOther.lastPos + info.lastPos) / 2;
                Vector2 currentCenter = (theOther.lastPos + eventData.position) / 2;
                Vector2 centerDelta = currentCenter - lastCenter;
                float startDist = (theOther.startPos - info.startPos).magnitude;
                float lastDist = (theOther.lastPos - info.lastPos).magnitude;
                float currentDist = (theOther.lastPos - eventData.position).magnitude;
                float distDelta = (theOther.lastPos - eventData.position).magnitude;

                _cachedEventData.pStart = startCenter;
                _cachedEventData.pCurrent = currentCenter;
                _cachedEventData.pDelta = (currentCenter - lastCenter);
                _cachedEventData.dStart = startDist;
                _cachedEventData.dCurrent = currentDist;
                _cachedEventData.dDelta = currentDist - lastDist;

                if (state != TouchState.Pinch)
                {
                    state = TouchState.Pinch;

                    pinchDistCursor = startDist;
                    pinchPosCursor = startCenter;

                    //float filteredDist = Mathf.Lerp(pinchDistCursor, currentDist, Mathf.Abs(pinchDistCursor - currentDist) / 30.0f);
                    //_cachedEventData.dCurrent = filteredDist;
                    //_cachedEventData.dDelta = filteredDist - pinchDistCursor;
                    //pinchDistCursor = filteredDist;
                    OnBeginPinchEvent?.Invoke(_cachedEventData);
                    //eventDelegate.onPinchStart(_cachedEventData);
                }
                else
                {
                    //float filteredDist = Mathf.Lerp(pinchDistCursor, currentDist, Mathf.Abs(pinchDistCursor - currentDist) / 30.0f);
                    //_cachedEventData.dCurrent = filteredDist;
                    //_cachedEventData.dDelta = filteredDist - pinchDistCursor;
                    //pinchDistCursor = filteredDist;
                    OnPinchEvent?.Invoke(_cachedEventData);
                    //eventDelegate.onPinch(_cachedEventData);
                }

                dragAcc += _cachedEventData.pDelta.magnitude - dragDelta.magnitude;
                dragDelta = _cachedEventData.pDelta;
                dragDeltaTime = Time.deltaTime;
                lastDragTime = Time.time;

            }
            else if(touchInfoList.Count == 1)
            {
                _cachedEventData.pStart = info.startPos;
                _cachedEventData.pCurrent = eventData.position;
                _cachedEventData.pDelta = (eventData.position - info.lastPos);

                if (state != TouchState.Drag)
                {
                    state = TouchState.Drag;
                    OnBeginDragEvent?.Invoke(_cachedEventData);
                    //eventDelegate.onDragBegin(_cachedEventData);
                }
                else
                {
                    OnDragEvent?.Invoke(_cachedEventData);
                    //eventDelegate.onDrag(_cachedEventData);
                }

                dragAcc += eventData.delta.magnitude - dragDelta.magnitude;
                dragDelta = eventData.delta;
                dragDeltaTime = Time.deltaTime;
                lastDragTime = Time.time;
            }

            info.lastPos = eventData.position;
        }
    }

    private void addTouchInfo(PointerEventData eventData)
    {
        TouchInfo touchInfo = new TouchInfo();
        touchInfo.touchId = eventData.pointerId;
        touchInfo.startPos = eventData.position;
        touchInfo.lastPos = eventData.position;
        touchInfoList.Add(touchInfo);
    }

    private TouchInfo getTouchInfo(int touchId)
    {
        foreach (TouchInfo info in touchInfoList)
        {
            if (info.touchId == touchId)
            {
                return info;
            }
        }

        return null;
    }

    private TouchInfo getTheOther(int touchId)
    {
        foreach (TouchInfo info in touchInfoList)
        {
            if (info.touchId != touchId)
            {
                return info;
            }
        }

        return null;
    }
}