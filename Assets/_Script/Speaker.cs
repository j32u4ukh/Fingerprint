using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Speaker : MonoBehaviour {
    [HideInInspector]public UnityEvent event1, event2;

    // Use this for initialization
    void Start () {
        event1 = new UnityEvent();
        event2 = new UnityEvent();
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {            
            event1.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {            
            event2.Invoke();
        }
    }
}
