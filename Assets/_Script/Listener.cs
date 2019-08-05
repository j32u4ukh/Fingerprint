using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Listener : MonoBehaviour {
    Speaker speaker;
    UnityEvent m_MyEvent;

    void Start()
    {
        if (m_MyEvent == null)
            m_MyEvent = new UnityEvent();

        m_MyEvent.AddListener(Ping);

        speaker = GetComponent<Speaker>();
        speaker.event1.AddListener(event1Happend);
        speaker.event2.AddListener(event2Happend);
    }

    void Update()
    {
        //if (Input.anyKeyDown && m_MyEvent != null)
        //{
        //    m_MyEvent.Invoke();
        //}
    }

    void Ping()
    {
        Debug.Log("Ping");
    }

    void event1Happend()
    {
        print("event1 happend");
    }

    void event2Happend()
    {
        print("event2 happend");
    }
}
