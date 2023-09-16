using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QE_CallbackEnum : MonoBehaviour
{
   public enum QE_CallBack
   {
      OnAwake,
      OnStart,
      OnEnable,
      OnDisable,
      OnUpdate,
      OnFixedUpdate,
      OnCollisionEnter,
      OnCollisionExit,
      OnCollisionStay,
      OnCollisionEnter2D,
      OnCollisionExit2D,
      OnCollisionStay2D,
      OnTriggerEnter,
      OnTriggerExit,
      OnTriggerStay,
      OnTriggerEnter2D,
      OnTriggerExit2D,
      OnTriggerStay2D
   }
}
