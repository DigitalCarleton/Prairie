using UnityEngine;
using System.Collections;

public class BroadcastListener : MonoBehaviour
{
    public string eventName;

    public void OnEventFires (GameObject rootInvoker)
    {
        this.gameObject.InteractAll (rootInvoker);
    }
}
