using System.Collections.Generic;
using UnityEngine;
using System;
using DPUruNet;
using System.IO;
using UnityEngine.UI;


public class FingPrintcCode : MonoBehaviour {

    FingerprintIdentification_to_Uniy.Class1 fingerController = null;

    public bool[] Lock_Test_bool;

    public bool Name_bool;

    public bool determine_bool;

    public Text _ID;

    public bool[] compareResult_Copy = new bool[6];

    public GameObject determine_Ok;

    public GameObject Next_Op;

    public string ID_Name;

    public string FingPrint_Data;

    public string[] FingPrint_Copy_2 = new string[10];

    public readonly string Path = @"D:\ID\datafingprint.txt";

    public Data_FingprintAndName_ID Main_Data;

    public Fmd[] Load_Fing_Fmd = new Fmd[10];

    public Text[] ID_Name_Text;

    private int Scoer;

    List<Data_FingprintAndName_ID> fingerPointData = null;

    public CompareResult[] FingSignIn = new CompareResult[10];

    void Start()
    {

        fingerController = new FingerprintIdentification_to_Uniy.Class1();
        Load_Data_Event();
        //TestArray();

    }
    private void Awake()
    {

    }

    void TestArray()
    {
        //從資料轉換到JsonString
        List<Data_FingprintAndName_ID> test = new List<Data_FingprintAndName_ID>();

        Data_FingprintAndName_ID data1 = new Data_FingprintAndName_ID();
        data1 = new Data_FingprintAndName_ID();
        data1.FingPrint_Data_Copy = "FingPrint_Data_Copy1";
        data1.ID_Name_Copy = "ID_Name_Copy1";

        Data_FingprintAndName_ID data2 = new Data_FingprintAndName_ID();
        data2 = new Data_FingprintAndName_ID();
        data2.FingPrint_Data_Copy = "FingPrint_Data_Copy2";
        data2.ID_Name_Copy = "ID_Name_Copy2";

        test.Add(data1);
        test.Add(data2);

        string stringJson = JSonConverter.Instance.ToJson(test, true);
        Debug.Log(">>>>> 1.  " + stringJson);


        /////////////////////////////////////////////////////////


        //從JSonString轉換資料到List
        StreamReader file = new StreamReader(@"D:\ID\datafingprint.txt");


        string jsonStr = file.ReadToEnd();
        file.Close();

        fingerPointData = JSonConverter.Instance.GetListFormJsonString<Data_FingprintAndName_ID>(jsonStr);

        foreach (Data_FingprintAndName_ID id in fingerPointData)
        {
            Debug.Log("id = " + id.ID_int + "  name = " + id.ID_Name_Copy);

        }

        ////新增一個物件類型為playerState的變數 loadData
        //Data_FingprintAndName_ID loadData = new Data_FingprintAndName_ID();

        ////使用JsonUtillty的FromJson方法將存文字轉成Json
        //loadData = JsonUtility.FromJson<Data_FingprintAndName_ID>(loadJson);

        ////驗證用，將sammaru的位置變更為json內紀錄的位置

        //FingPrint_Copy_2[Main_Data.ID_int] = loadData.FingPrint_Data_Copy;
        ////GameObject.Find("sammaru").transform.position = loadData.pos;
    }

    void Update()
    {
        //按鈕觸發A
        //if (Input.GetKeyDown(KeyCode.A))
        //{
        //    fingerController.StartLoad();
        //}
        Event_SignIn();

        Event_registered();

        //讓名子 資料更新
        ID_Name = _ID.text;
        //6次判定是否認證成功
        for (int i = 0; i < compareResult_Copy.Length; i++)
        {
            compareResult_Copy[i] = fingerController.CompareResult_bool[i];
        }

        if (compareResult_Copy[0] == true && compareResult_Copy[1] == true && compareResult_Copy[2] == true && compareResult_Copy[3] == true && compareResult_Copy[4] == true && compareResult_Copy[5] == true)
        {
            save();

            //Save_Text_Database(FingPrint_Data);
            determine_Ok.SetActive(false);
            for (int i = 0; i < compareResult_Copy.Length; i++)
            {
                fingerController.CompareResult_bool[i] = false;
            }

        }
        ////判斷指紋是登入狀態或註冊狀態


    }
    public void Start_GO()
    {
        fingerController.StartLoad();
    }


    [Serializable]
    public class Data_FingprintAndName_ID
    {
        public string ID_Name_Copy;

        public string FingPrint_Data_Copy;

        public int ID_int = 0;
    }

    //Json_Save
    public void save()
    {

        Debug.Log("11");
        //一個玩家指紋數據
        //save_string.Add(data2);
        Data_FingprintAndName_ID data1 = new Data_FingprintAndName_ID();
        data1 = new Data_FingprintAndName_ID();
        data1.FingPrint_Data_Copy = fingerController.base64Fmd;
        data1.ID_Name_Copy = _ID.text;
        //data1.ID_int += 1;
        fingerPointData.Add(data1);

        string stringJson = JSonConverter.Instance.ToJson(fingerPointData, true);

        StreamWriter file = new StreamWriter(Path);
        file.Write(stringJson);
        file.Close();
        Debug.Log(">>>>> 1.  " + stringJson);

        Debug.Log(" data1.ID_Name_Copy = " + data1.ID_Name_Copy + " data1.FingPrint_Data_Copy = " + data1.FingPrint_Data_Copy);
        fingerController.CancelCaptureAndCloseReader(fingerController.OnCaptured);

        //Debug.Log(" data2.ID_Name_Copy = " + data2.ID_Name_Copy + " data2.FingPrint_Data_Copy = " + data2.FingPrint_Data_Copy);
    }
    public void load()
    {
        StreamReader file = new StreamReader(@"D:\ID\datafingprint.txt");


        string jsonStr = file.ReadToEnd();
        file.Close();

        fingerPointData = JSonConverter.Instance.GetListFormJsonString<Data_FingprintAndName_ID>(jsonStr);

        for (int i = 0; i < fingerPointData.Count; i++)
        {
            FingPrint_Copy_2[i] = fingerPointData[i].FingPrint_Data_Copy;
            Load_Fing_Fmd[i] = Fmd.DeserializeXml(FingPrint_Copy_2[i]);
            Debug.Log(" Load_Fing_Fmd[i] = " + FingPrint_Copy_2[i]);

        }



        foreach (Data_FingprintAndName_ID id in fingerPointData)
        {
            Debug.Log("id = " + id.ID_int + "  name = " + id.ID_Name_Copy + " Fingprint =" + id.FingPrint_Data_Copy);
      
        }



        //if(File.Exists(Path) == false)
        //{
        //    Debug.LogError("File Is Not Exists!!");

        //    StreamReader file = new StreamReader(@"D:\ID\datafingprint.txt");

        //    string jsonStr = file.ReadToEnd();
        //    file.Close();

        //    fingerPointData = JSonConverter.Instance.GetListFormJsonString<Data_FingprintAndName_ID>(jsonStr);

        //    foreach (Data_FingprintAndName_ID id in fingerPointData)
        //    {
        //        Debug.Log("id = " + id.ID_int + "  name = " + id.ID_Name_Copy);
        //    }

        //    //fingerPointData = new Data_FingprintAndName_ID[0];
        //    fingerPointData = new List<Data_FingprintAndName_ID>();
        //}
        //else
        //{
        //    //讀取json檔案並轉存成文字格式
        //    StreamReader file = new StreamReader(System.IO.Path.Combine(Application.persistentDataPath, Path));
        //    string loadJson = file.ReadToEnd();
        //    file.Close();
        //    //新增一個物件類型為playerState的變數 loadData
        //    Data_FingprintAndName_ID loadData = new Data_FingprintAndName_ID();

        //    //使用JsonUtillty的FromJson方法將存文字轉成Json
        //    loadData = JsonUtility.FromJson<Data_FingprintAndName_ID>(loadJson);

        //    //驗證用，將sammaru的位置變更為json內紀錄的位置

        //    FingPrint_Copy_2[Main_Data.ID_int] = loadData.FingPrint_Data_Copy;
        //    Load_Fing_Fmd = Fmd.DeserializeXml(FingPrint_Copy_2[Main_Data.ID_int]);
        //    //GameObject.Find("sammaru").transform.position = loadData.pos;
        //}
    }

    //已註冊完後，登入
    public void Event_SignIn()
    {
        //FingPrint_Data = fingerController.base64Fmd;

        //fingerController.compareResult = Comparison.Compare(fingerController.firstFinger, 0, Load_Fing_Fmd[0], 0);
        for (int i = 0; i < FingSignIn.Length; i++)
        {
            FingSignIn[i] = Comparison.Compare(fingerController.firstFinger, 0, Load_Fing_Fmd[i], 0);
            if (FingSignIn[i].Score < (fingerController.PROBABILITY_ONE / 100000))
            {
                fingerController.CancelCaptureAndCloseReader(fingerController.OnCaptured);

                determine_Ok.SetActive(false);
                Next_Op.SetActive(true);
                //fingerController.Reset = true;
                // fingerController.resultConversion.ResultCode = Constants.ResultCode.DP_NO_DATA;
            }
        }

       

    }

    public void Load_Data_Event()
    {
        //Load 資料
        if (File.Exists(Path) == false)
        {
            Debug.Log("AAA");

        }
        else
        {
            load();
        }
    }

    //註冊Event
    public void Event_registered()
    {
        //3次認證指紋
        if (determine_bool == false && fingerController.count == 0)
        {
            Name_bool = false;
        }
        if (fingerController.count == 1 && Lock_Test_bool[0] == false)
        {
            Lock_Test_bool[0] = true;
        }
        else if (fingerController.count == 2 && Lock_Test_bool[1] == false)
        {
            Lock_Test_bool[1] = true;
        }
        else if (fingerController.count == 3 && Lock_Test_bool[2] == false)
        {
            Lock_Test_bool[2] = true;
        }
        else if (fingerController.count == 0)
        {
            for (int i = 0; i < Lock_Test_bool.Length; i++)
            {

                Lock_Test_bool[i] = false;
            }
        }
    }

}
