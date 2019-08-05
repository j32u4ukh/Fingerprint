using UnityEngine;
using UnityEngine.Events;
using DPUruNet;
using System;
using UnityEngine.UI;
using System.Collections.Generic;

public class Test : MonoBehaviour {
    FingerprintCapture capture;
    Fmd finger1, finger2;

    public Button close_reader;
    public Text text;
    public InputField input;
    bool is_new_user;
    string user_name;
    Dictionary<string, string> user;
    Fmd user_finger;

    bool start_compare;    

    // Use this for initialization
    void Start () {
        capture = new FingerprintCapture();
        finger1 = null;
        finger2 = null;
        user = new Dictionary<string, string>();
        is_new_user = true;
        start_compare = false;

        capture.error.AddListener(getErrorEvent);
        capture.finger.AddListener(getFingerprint);

        close_reader.onClick.AddListener(() =>
        {
            capture.closeReader();
            print("CancelCaptureAndCloseReader");
        });
    }

    // Update is called once per frame
    void Update () {
        if ((finger1 != null) && (finger2 != null) && Input.GetKeyDown(KeyCode.C))
        {
            compareTwoFinger();
        }

        if (start_compare)
        {
            signInOrUp();
        }
    }

    void compareTwoFinger()
    {
        CompareResult compareResult = Comparison.Compare(finger1, 0, finger2, 0);
        if (compareResult.ResultCode != Constants.ResultCode.DP_SUCCESS)
        {
            throw new Exception(compareResult.ResultCode.ToString());
        }

        print("Comparison resulted in a dissimilarity score of " + compareResult.Score.ToString() + (compareResult.Score < (FingerprintCapture.PROBABILITY_ONE / 100000) ? " (fingerprints matched)" : " (fingerprints did not match)"));
    }

    void getErrorEvent(string error_message)
    {
        print(error_message);
    }

    void getFingerprint(Fmd fmd)
    {
        print("getFingerprint");
        finger1 = fmd;
        start_compare = true;
    }

    void signInOrUp()
    {
        print("signInOrUp");

        CompareResult compareResult;
        foreach (KeyValuePair<string, string> item in user)
        {
            finger2 = Fmd.DeserializeXml(item.Value);
            compareResult = Comparison.Compare(finger1, 0, finger2, 0);
            if (compareResult.ResultCode != Constants.ResultCode.DP_SUCCESS)
            {
                throw new Exception(compareResult.ResultCode.ToString());
            }

            if (compareResult.Score < (FingerprintCapture.PROBABILITY_ONE / 100000)){
                is_new_user = false;
                text.text = string.Format("Welcome {0}", item.Key);
                break;
            }
        }

        if (is_new_user)
        {
            string name = input.text;
            string fingerprint = Fmd.SerializeXml(finger1);
            user.Add(name, fingerprint);
            text.text = string.Format("New user {0}", name);
        }

        input.text = "";
        start_compare = false;
        is_new_user = true;
    }
}
