using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class Nf_QuickEvent : MonoBehaviour
{
    public enum CollisionType
    {
        Collision_2D,
        Collision_3D,
        Trigger_2D,
        Trigger_3D
    }
    
    

    public EventList[] MyQuickEvents;

    [Header("*Turn on if you are using collider callback*")]
    public bool UseColliderFunctions;
    public CollisionType _CollisionType;
    
    
    
    private EventList OnAwake, OnStart,_OnEnable,_OnDisable;

    private void Awake()
    {
        
        
        EventSort(MyQuickEvents);
        OnAwake.Response?.Invoke();
    }

    private void Start()
    {
        OnStart.Response?.Invoke();
    }

    private void OnEnable()
    {
        _OnEnable.Response?.Invoke();
    }

    private void OnDisable()
    {
        _OnDisable.Response?.Invoke();
    }


    private void EventSort(EventList[] eventLists)
    {
        
        //add collider type
        switch (_CollisionType)
        {
            case CollisionType.Collision_2D:
                this.AddComponent<QE_Collision_2d>();
                break;
            case CollisionType.Collision_3D:
                this.AddComponent<QE_Collision_3d>();
                break;
            case CollisionType.Trigger_2D:
                this.AddComponent<QE_Trigger_2d>();
                break;
            case CollisionType.Trigger_3D:
                this.AddComponent<QE_Trigger_3d>();
                break;
        }

        

        //sorting call back type
        foreach (var item in eventLists)
        {
            switch (item.CallBackFunction)
            {
                //initiate
                case QE_CallbackEnum.QE_CallBack.OnAwake:
                    OnAwake = item;
                    break;
                case QE_CallbackEnum.QE_CallBack.OnStart:
                    OnStart = item;
                    break;
                //enable,disable
                case QE_CallbackEnum.QE_CallBack.OnEnable:
                    _OnEnable = item;
                    break;
                case QE_CallbackEnum.QE_CallBack.OnDisable:
                    _OnDisable = item;
                    break;
                //update
                case QE_CallbackEnum.QE_CallBack.OnUpdate:
                    if (QE_Update.Instance != null)
                    {
                        QE_Update.Instance.OnUpdate.Add(item);
                    }
                    else
                    {
                        Debug.Log("Please add QE_Update to your scene");
                    }
                    break;
                case QE_CallbackEnum.QE_CallBack.OnFixedUpdate:
                    QE_Update.Instance.OnFixedUpdate.Add(item);
                    break;
                //collision
                case QE_CallbackEnum.QE_CallBack.OnCollisionEnter:
                    GetComponent<QE_Collision_3d>().OnColEnter = item;
                    break;
                case QE_CallbackEnum.QE_CallBack.OnCollisionExit:
                    GetComponent<QE_Collision_3d>().OnColExit = item;
                    break;
                case QE_CallbackEnum.QE_CallBack.OnCollisionStay:
                    GetComponent<QE_Collision_3d>().OnColStay = item;
                    break;
                case QE_CallbackEnum.QE_CallBack.OnCollisionEnter2D:
                    GetComponent<QE_Collision_2d>().OnColEnter = item;
                    break;
                case QE_CallbackEnum.QE_CallBack.OnCollisionExit2D:
                    GetComponent<QE_Collision_2d>().OnColExit = item;
                    break;
                case QE_CallbackEnum.QE_CallBack.OnCollisionStay2D:
                    GetComponent<QE_Collision_2d>().OnColStay = item;
                    break;
                //trigger
                case QE_CallbackEnum.QE_CallBack.OnTriggerEnter:
                    GetComponent<QE_Trigger_3d>()._OnTriggerEnter = item;
                    break;
                case QE_CallbackEnum.QE_CallBack.OnTriggerExit:
                    GetComponent<QE_Trigger_3d>()._OnTriggerExit = item;
                    break;
                case QE_CallbackEnum.QE_CallBack.OnTriggerStay:
                    GetComponent<QE_Trigger_3d>()._OnTriggerStay = item;
                    break;
                case  QE_CallbackEnum.QE_CallBack.OnTriggerEnter2D:
                    GetComponent<QE_Trigger_2d>()._OnTriggerEnter = item;
                    break;
                case  QE_CallbackEnum.QE_CallBack.OnTriggerExit2D:
                    GetComponent<QE_Trigger_2d>()._OnTriggerExit = item;
                    break;
                case  QE_CallbackEnum.QE_CallBack.OnTriggerStay2D:
                    GetComponent<QE_Trigger_2d>()._OnTriggerStay = item;
                    break;
            }
        }
    }
 
}



[Serializable]
public struct EventList
{
    public string EventHeaderName;
    public QE_CallbackEnum.QE_CallBack CallBackFunction;
    public UnityEvent Response;
}
