using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Delegate_Code : MonoBehaviour {

    public FingerprintIdentification_to_Uniy.Class1 fingerController = null;


    void Awake()
    {

        fingerController = new FingerprintIdentification_to_Uniy.Class1();


    }

    void Start()
    {
        fingerController.OnFingerPrintPush = Test_FingerPrint_Delegate;
    }
 
    public void Test_FingerPrint_Delegate()
    {
        Debug.Log("Test_Raset");
    }
    
}
