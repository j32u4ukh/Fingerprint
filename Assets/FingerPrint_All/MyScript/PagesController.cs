using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using DPUruNet;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



[System.Serializable]
public class Data_FingprintAndName_ID
{
    public string ID_Name_Copy;

    public string FingPrint_Data_Copy;

    public int ID_User = 0;

}



public class PagesController : MonoBehaviour {


    //inner class


    string jsonStr = "";
    public FingerprintIdentification_to_Uniy.Class1 fingerController = null;

    Data_FingprintAndName_ID data1 = new Data_FingprintAndName_ID();

    public List<Data_FingprintAndName_ID> fingerPointData = null;

    public Transform container = null;

    public GameObject dataPrefab = null;

    int nowPage = 1;

    int totalPage = 0;

    Father_obj[] buttons = new Father_obj[6];

    public readonly string Path = @"D:\ID\datafingprint.txt";

    public readonly string Path_To_D = @"D:\ID";

    private Fmd[] Load_Fing_Fmd;

    public CompareResult[] FingSignIn;

    public Image Head_Photo;

    public Sprite[] Photo_Image;

    public bool[] compareResult_Copy;

    public int EventScore;

    public GameObject[] Text_Tip = new GameObject[6];

    public string ID_Name;

    public Text _ID;

    public GameObject Photo_Go;

    public Text SingInName;

    public bool InSignIn_bool;

    public GameObject Image_Game;

    public GameObject Image_Go;

    public GameObject SingInImage_Game;

    public GameObject SingInImage_Go;

    public GameObject Error_Fing_obj;

    public GameObject Input_Go;

    public GameObject Ok_Button_Save;

    public GameObject Fing_Tip_Close;

    public GameObject FingError_SingIn;

    public GameObject inst_Delete_go;

    public GameObject Delete_go_menu;

    public GameObject Back_obj;

    public GameObject Error_add;

    public GameObject OK_good;

    public GameObject add_Close;

    public bool Lock_Ins;

    public bool Lock_singIn;

    public bool Add_bool;

    public bool Lock_Error_Image;

    // public GameObject Image_Error;
    private AsyncOperation Load_Scene;

    public GameObject Menu_obj;
    //AsyncOperation async_Scene;

    private void Awake()
    {
        //PlayerPrefs.DeleteAll();

        if (Directory.Exists(Path_To_D))
        {
            //資料夾存在
            Debug.Log("-------------> Hi");
        }
        else
        {
            //新增資料夾
            Debug.Log("-------------> NOOOOOOOOOOO");
            Directory.CreateDirectory(@"D:\ID\");
        }
        fingerController = new FingerprintIdentification_to_Uniy.Class1();
    }

    void Start()
    {
        Load_Scene = SceneManager.LoadSceneAsync(1);
        Load_Scene.allowSceneActivation = false;
        fingerController = new FingerprintIdentification_to_Uniy.Class1();

        Load_Data_Event();
        ChangePage(1);

    }
    void Update()
    {


        for (int i = 0; i < compareResult_Copy.Length; i++)
        {
            compareResult_Copy[i] = fingerController.CompareResult_bool[i];
        }

        //Debug.Log(" Path = " + Path);
        if (File.Exists(Path) == false)
        {
            List<Data_FingprintAndName_ID> fingerPointData = new List<Data_FingprintAndName_ID>();

            string stringJson = JSonConverter.Instance.ToJson(fingerPointData, true);
            StreamWriter file = new StreamWriter(Path);
            file.Write(stringJson);
            file.Close();

        }
        //第一個
        if (fingerController.Lose_bool[0] == true && EventScore == 0)
        {
            EventScore = 1;
            Text_Tip[5].SetActive(false);
            Text_Tip[0].SetActive(false);
            Text_Tip[1].SetActive(true);
            Error_add.SetActive(false);

        }

        //第二個
        if (fingerController.Lose_bool[1] == true)
        {
            Lock_Error_Image = false;
            // Event_SignIn();
            if (compareResult_Copy[0] == true && EventScore == 1)
            {
                //Error_add.SetActive(false);

                Text_Tip[1].SetActive(false);
                Text_Tip[2].SetActive(true);
                //Event_SignIn();
                EventScore = 2;

            }
            else if (compareResult_Copy[0] == false && EventScore == 1)
            {
                for (int i = 0; i < Text_Tip.Length; i++)
                {
                    Text_Tip[i].SetActive(false);
                }
                Text_Tip[5].SetActive(true);
                Error_add.SetActive(true);
                EventScore = 0;
                fingerController.ReStart(Test_FingerPrint_Delegate);

                //UI顯示指紋失敗   ->   重新開始
            }
        }

        //第三個
        if (fingerController.Lose_bool[2] == true)
        {
            // Event_SignIn();
            //Event_SignIn_Add();
            if (compareResult_Copy[0] == true && compareResult_Copy[1] == true && compareResult_Copy[2] == true && EventScore == 2)
            {
                //Error_add.SetActive(false);

                Text_Tip[2].SetActive(false);
                Text_Tip[3].SetActive(true);
                EventScore = 3;

            }
            else if ((compareResult_Copy[0] == false || (compareResult_Copy[1] == false || compareResult_Copy[2] == false) && EventScore == 2))
            {
                Error_add.SetActive(true);

                for (int i = 0; i < Text_Tip.Length; i++)
                {
                    Text_Tip[i].SetActive(false);
                }
                EventScore = 0;
                //Error_add.SetActive(true);

                Text_Tip[5].SetActive(true);
                fingerController.ReStart(Test_FingerPrint_Delegate);

                //UI顯示指紋失敗   ->   重新開始
            }
        }

        Lock_Error_Image = true;


        //第四個
        if (fingerController.Lose_bool[3] == true)
        {
            // Event_SignIn();
            //Event_SignIn_Add();
            if (compareResult_Copy[0] == true && compareResult_Copy[1] == true && compareResult_Copy[2] == true && compareResult_Copy[3] == true && compareResult_Copy[4] == true && compareResult_Copy[5] == true && EventScore == 3)
            {
             //   Error_add.SetActive(false);

                Ok_Button_Save.SetActive(true);
                save();

                Text_Tip[3].SetActive(false);
                Text_Tip[4].SetActive(true);
                for (int i = 0; i < compareResult_Copy.Length; i++)
                {
                    fingerController.CompareResult_bool[i] = false;
                }

            }
            else if ((compareResult_Copy[0] == false || compareResult_Copy[1] == false || compareResult_Copy[2] == false || compareResult_Copy[3] == false || compareResult_Copy[4] == false || compareResult_Copy[5] == false) && EventScore == 3)
            {

                for (int i = 0; i < Text_Tip.Length; i++)
                {
                    Text_Tip[i].SetActive(false);
                }
                Text_Tip[5].SetActive(true);

                EventScore = 0;

                fingerController.ReStart(Test_FingerPrint_Delegate);

            }
        }

        if (Add_bool == true)
        {
            for (int i = 0; i < fingerPointData.Count; i++)
            {
                if (fingerPointData[i].ID_User >= 1)
                {
                    Event_SignIn_Add();
                }
            }

            //Event_SignIn_Add();
        }
        if (InSignIn_bool == true)
        {
            for (int i = 0; i < fingerPointData.Count; i++)
            {
                if (fingerPointData[i].ID_User >= 1)
                {
                    Debug.Log("HIIIIIIIIIIII");
                    Event_SignIn();
                  
                }

            }
            //Debug.Log("AA");
        }
        //Event_SignIn_Add();
        //Debug.Log(" fingerPointData.Count = " + fingerPointData.Count);
        if (fingerPointData.Count == 0)
        {
            //Debug.Log("QQ");
            Fing_Tip_Close.SetActive(false);
            FingError_SingIn.SetActive(true);
            Lock_singIn = true;
        }
        else
        {
            
            Lock_singIn = false;
        }
        ID_Name = _ID.text;
    }

    public void ChangePage(int pageN)
    {
        ClearUIData();
        int start = (pageN - 1) * 6;
        int end = start + 6;

        int n = 0;
        for (int i = start; i < end; i++)
        {
            if (fingerPointData.Count <= i)
                break;

            GameObject go = Instantiate(dataPrefab, container);

            string name = fingerPointData[i].ID_Name_Copy;
            int id = fingerPointData[i].ID_User;
            go.GetComponent<Father_obj>().SetData(name, id);

            buttons[n] = go.GetComponent<Father_obj>();
            n++;
        }
    }

    void ChooseButton(int index)
    {

    }

    void ClearUIData()
    {
        int count = container.transform.childCount;

        Debug.Log("count = " + count);

        for (int i = 0; i < count; i++)
        {
            Debug.Log("Destroy ");
            Destroy(container.transform.GetChild(count - 1 - i).gameObject);
        }
    }


    public void LoadFingerData()
    {
        StreamReader file = new StreamReader(@"D:\ID\datafingprint.txt");

        jsonStr = file.ReadToEnd();
        file.Close();
        Debug.Log("1. jsonStr =" + jsonStr);
        Debug.Log("2. JSonConverter.Instance =" + JSonConverter.Instance);

        //fingerController.CancelCaptureAndCloseReader(fingerController.OnCaptured);
        fingerPointData = JSonConverter.Instance.GetListFormJsonString<Data_FingprintAndName_ID>(jsonStr);

        Debug.Log(" fingerPointData = " + fingerPointData.Count + " jsonStr =" + jsonStr);

        for (int i = 0; i < fingerPointData.Count; i++)
            {
                //  FingPrint_Copy_2 = new string[fingerPointData[i].ID_User];
                //Photo_Image = new Sprite[fingerPointData[i].ID_User];
                //Debug.Log(fingerPointData.Count);
                FingSignIn = new CompareResult[fingerPointData.Count];
                Load_Fing_Fmd = new Fmd[fingerPointData.Count];

            //Photo_Image = new Sprite[fingerPointData[i].ID_User];
            // Debug.Log(" FingSignIn =" + FingSignIn[i]);
        }
        if (FingSignIn != null)
            {
                for (int i = 0; i < Load_Fing_Fmd.Length; i++)
                {
                    Debug.Log("FingSignIn");
                    //FingPrint_Copy_2[i] = fingerPointData[i].FingPrint_Data_Copy;
                    Load_Fing_Fmd[i] = Fmd.DeserializeXml(fingerPointData[i].FingPrint_Data_Copy);
                    //Debug.Log(Load_Fing_Fmd[i]);
                    //Event_SignIn();

                }
            }
            fingerController.ReStart(Test_FingerPrint_Delegate);

            fingerController.CancelCaptureAndCloseReader(fingerController.OnCaptured);

            foreach (Data_FingprintAndName_ID id in fingerPointData)
            {
                Debug.Log("id = " + id.ID_User + "  name = " + id.ID_Name_Copy + " Fingprint =" + id.FingPrint_Data_Copy);
            }

            totalPage = fingerPointData.Count / 6;
            if (fingerPointData.Count % 6 != 0)
            {
                totalPage += 1;
            }
        
        
    }

    public void NextPage()
    {
        if (nowPage < totalPage)
        {
            nowPage++;
            ChangePage(nowPage);
        }

    }

    public void BackPage()
    {
        if (nowPage > 1)
        {
            nowPage--;
            ChangePage(nowPage);

        }

    }
    //****
    public void Start_GO()
    {
        Image_Go.SetActive(false);
        Image_Game.SetActive(true);

        fingerController.ReStart(Test_FingerPrint_Delegate);
    }
    public void Start_GO_gien()
    {
        //SingInImage_Game.SetActive(true);
        Lock_singIn = false;
        SingInImage_Go.SetActive(false);
        GameObject.Find("GameManager_Fingprint").GetComponent<PagesController>().InSignIn_bool = false;


        SceneManager.LoadScene("Menu_20190125");

        // SceneManager.LoadScene("Menu");
        LoadFingerData();
        GameObject.Find("GameManager_ButtonEvent").GetComponent<Button_Totxh_Event>().FingIn_UI_Open.SetActive(false);
        GameObject.Find("GameManager_ButtonEvent").GetComponent<Button_Totxh_Event>().Menu_Close.SetActive(true);

        fingerController.ReStart(Test_FingerPrint_Delegate);
    }
    public void Start_GO_SingOut()
    {

        InSignIn_bool = true;
        Fing_Tip_Close.SetActive(true);
        SingInImage_Game.SetActive(false);
        Photo_Go.SetActive(false);
        SingInImage_Go.SetActive(true);
        SceneManager.LoadScene("Menu_20190125");
    }
    public void Delete()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            if (buttons[i] != null)
            {
                if (buttons[i].choose)
                {
                    //刪除

                    for (int n = 0; n < fingerPointData.Count; n++)
                    {
                        if (fingerPointData[n].ID_User == buttons[i].id)
                        {
                            fingerPointData.RemoveAt(n);
                            break;
                        }
                    }
                }
            }
        }
        Event_SignIn();
        ChangePage(nowPage);
        //save();

        StreamWriter file = new StreamWriter(Path);
        string stringJson = JSonConverter.Instance.ToJson(fingerPointData, true);
        file.Write(stringJson);
        file.Close();
        //fingerController.CancelCaptureAndCloseReader(fingerController.OnCaptured);
        inst_Delete_go.SetActive(false);

        totalPage = fingerPointData.Count / 6;
        if (fingerPointData.Count % 6 != 0)
        {
            totalPage += 1;
        }

        Debug.Log("totalPage = " + totalPage);
    }
    public void Delete_ins()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            if (fingerPointData.Count >= 1)
            {
                inst_Delete_go.SetActive(true);
            }
        }

    }
    public void Delete_ins_NO()
    {
        inst_Delete_go.SetActive(false);
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
            LoadFingerData();
        }
    }
    public void save()
    {

        Debug.Log("???");
        add_Close.SetActive(false);
        OK_good.SetActive(true);
        data1 = new Data_FingprintAndName_ID();

        int usedId = 0;
        if (PlayerPrefs.HasKey("UsedUserId"))
        {
            usedId = PlayerPrefs.GetInt("UsedUserId");
        }

        usedId++;
        //for (int i = 0; i < fingerPointData.Count + 1; i++)
        //{
        //    data1.ID_User += 1;
        //}

        data1.ID_User = usedId;

        PlayerPrefs.SetInt("UsedUserId", usedId);

        Debug.Log(" data1.ID_User = " + data1.ID_User);
        data1.FingPrint_Data_Copy = fingerController.base64Fmd;
        data1.ID_Name_Copy = _ID.text;
        fingerPointData.Add(data1);
        string stringJson = JSonConverter.Instance.ToJson(fingerPointData, true);
        StreamWriter file = new StreamWriter(Path);
        file.Write(stringJson);
        file.Close();
        //fingerController.CancelCaptureAndCloseReader(fingerController.OnCaptured);
        Input_Go.SetActive(true);
        Image_Game.SetActive(false);
        //Lock_Error_Image = true;
        //fingerController.ReStart();

    }
    //已註冊完後，登入
    public void Event_SignIn()
    {
        //LoadFingerData();
        if (Lock_singIn == false && Lock_Ins == false)
        {
            if (Load_Fing_Fmd.Length >= 1)
            {
                for (int i = 0; i < Load_Fing_Fmd.Length; i++)
                {
                    Debug.Log(i);

                    FingSignIn[i] = Comparison.Compare(fingerController.firstFinger, 0, Load_Fing_Fmd[i], 0);
                    Debug.Log("AAA");
                    Debug.Log(FingSignIn[i] = Comparison.Compare(fingerController.firstFinger, 0, Load_Fing_Fmd[i], 0));
                    if (fingerController.Lose_bool[0] == true)
                    {
                        if (FingSignIn[i].Score < (fingerController.PROBABILITY_ONE / 100000))
                        {

                            //PlayerPrefs.SetString("LogIn_ID_Name", fingerPointData[i].ID_Name_Copy);
                            //PlayerPrefs.SetInt("LogIn_ID_User", fingerPointData[i].ID_User);
                            //Load_Scene.allowSceneActivation = true;
                            Debug.Log("HI");
                            Fing_Tip_Close.SetActive(false);
                            InSignIn_bool = false;
                            SingInImage_Game.SetActive(true);
                            SingInName.text = fingerPointData[i].ID_Name_Copy;
                            Head_Photo.sprite = Photo_Image[i];
                            Photo_Go.SetActive(true);
                            fingerController.ReStart(Test_FingerPrint_Delegate);
                            Back_obj.SetActive(false);
                            fingerController.CancelCaptureAndCloseReader(fingerController.OnCaptured);
                        }

                    }
                }
                for (int j = 0; j < Load_Fing_Fmd.Length; j++)
                {
                    if (fingerController.Lose_bool[0] == true)
                    {
                        if (FingSignIn[j].Score > (fingerController.PROBABILITY_ONE / 100000))
                        {
                            Debug.Log("Q");
                            Fing_Tip_Close.SetActive(false);
                            FingError_SingIn.SetActive(true);
                            Lock_singIn = true;

                            //fingerController.ReStart(Test_FingerPrint_Delegate);
                        }
                    }
                }

            }
        }



    }

    //已註冊完後，登入
    public void Event_SignIn_Add()
    {
        if (fingerController.Lose_bool[0] == true && Lock_singIn == false)
        {
            if (FingSignIn.Length >= 1)
            {
                for (int i = 0; i < Load_Fing_Fmd.Length; i++)
                {
                    FingSignIn[i] = Comparison.Compare(fingerController.firstFinger, 0, Load_Fing_Fmd[i], 0);

                    if (FingSignIn[i].Score < (fingerController.PROBABILITY_ONE / 100000))
                    {
                        for (int j = 0; j < Text_Tip.Length; j++)
                        {
                            Text_Tip[j].SetActive(false);
                        }
                        Debug.Log("QQQ");
                        EventScore = 0;
                        Image_Game.SetActive(false);

                        Error_Fing_obj.SetActive(true);
                        Add_bool = false;

                        fingerController.ReStart(Test_FingerPrint_Delegate);

                        fingerController.CancelCaptureAndCloseReader(fingerController.OnCaptured);

                    }
                }
            }

        }
    }
    public void Delete_All()
    {
        Debug.Log("Q");
        Delete_go_menu.SetActive(false);
        fingerPointData.Clear();
        StreamWriter file = new StreamWriter(Path);
        string stringJson = JSonConverter.Instance.ToJson(fingerPointData, true);
        file.Write(stringJson);
        file.Close();
        Delete();

    }
    //以認證過
    public void Error_Onclick()
    {

        Input_Go.SetActive(true);
        Error_Fing_obj.SetActive(false);
    
        Text_Tip[1].SetActive(true);
        fingerController.CancelCaptureAndCloseReader(fingerController.OnCaptured);
    }
    public void Error_RePlay()
    {
        InSignIn_bool = true;
        Fing_Tip_Close.SetActive(true);
        SingInImage_Game.SetActive(false);
        Photo_Go.SetActive(false);
        SingInImage_Go.SetActive(true);
        Start_GO_gien();
        FingError_SingIn.SetActive(false);

    }
    public void Event_Back_Menu()
    {
        //LoadFingerData();
        //fingerController.ReStart(Test_FingerPrint_Delegate);
        Add_bool = false;
        LoadFingerData();

        fingerController.CancelCaptureAndCloseReader(fingerController.OnCaptured);
        Input_Go.GetComponent<InputField>().text = "";
        Input_Go.SetActive(true);
        Image_Go.SetActive(false);
        Error_Fing_obj.SetActive(false);
        Ok_Button_Save.SetActive(false);
        Error_add.SetActive(false);
        for (int i = 0; i < Text_Tip.Length; i++)
        {
            Text_Tip[i].SetActive(false);
        }
        OK_good.SetActive(false);
        Menu_obj.SetActive(true);
        //SceneManager.LoadSceneAsync("Menu_20190125");
    }

    public void Start_Game()
    {
        SceneManager.LoadScene(0);

    }
    public void Test_FingerPrint_Delegate()
    {
        ////第一個
        //if (fingerController.Lose_bool[0] == true && EventScore == 0)
        //{
        //    EventScore = 1;
        //    Text_Tip[0].SetActive(false);
        //    Text_Tip[1].SetActive(true);
        //    Error_add.SetActive(false);

        //}

        ////第二個
        //if (fingerController.Lose_bool[1] == true)
        //{
        //    Lock_Error_Image = false;
        //    // Event_SignIn();
        //    if (compareResult_Copy[0] == true && EventScore == 1)
        //    {
        //        Error_add.SetActive(false);

        //        Text_Tip[1].SetActive(false);
        //        Text_Tip[2].SetActive(true);
        //        //Event_SignIn();
        //        EventScore = 2;

        //    }
        //    else if (compareResult_Copy[0] == false && EventScore == 1)
        //    {
        //        for (int i = 0; i < Text_Tip.Length; i++)
        //        {
        //            Text_Tip[i].SetActive(false);
        //        }
        //        Text_Tip[0].SetActive(true);
        //        Error_add.SetActive(true);
        //        EventScore = 0;
        //        fingerController.ReStart(Test_FingerPrint_Delegate);

        //        //UI顯示指紋失敗   ->   重新開始
        //    }
        //}

        ////第三個
        //if (fingerController.Lose_bool[2] == true)
        //{
        //    // Event_SignIn();
        //    //Event_SignIn_Add();
        //    if (compareResult_Copy[0] == true && compareResult_Copy[1] == true && compareResult_Copy[2] == true && EventScore == 2)
        //    {
        //        Error_add.SetActive(false);

        //        Text_Tip[2].SetActive(false);
        //        Text_Tip[3].SetActive(true);
        //        EventScore = 3;

        //    }
        //    else if ((compareResult_Copy[0] == false || (compareResult_Copy[1] == false || compareResult_Copy[2] == false) && EventScore == 2))
        //    {
        //        for (int i = 0; i < Text_Tip.Length; i++)
        //        {
        //            Text_Tip[i].SetActive(false);
        //        }
        //        EventScore = 0;
        //        Error_add.SetActive(true);

        //        Text_Tip[0].SetActive(true);
        //        fingerController.ReStart(Test_FingerPrint_Delegate);

        //        //UI顯示指紋失敗   ->   重新開始
        //    }
        //}

        //Lock_Error_Image = true;


        ////第四個
        //if (fingerController.Lose_bool[3] == true)
        //{
        //    // Event_SignIn();
        //    //Event_SignIn_Add();
        //    if (compareResult_Copy[0] == true && compareResult_Copy[1] == true && compareResult_Copy[2] == true && compareResult_Copy[3] == true && compareResult_Copy[4] == true && compareResult_Copy[5] == true && EventScore == 3)
        //    {
        //        Error_add.SetActive(false);

        //        Ok_Button_Save.SetActive(true);
        //        save();

        //        Text_Tip[3].SetActive(false);
        //        Text_Tip[4].SetActive(true);
        //        for (int i = 0; i < compareResult_Copy.Length; i++)
        //        {
        //            fingerController.CompareResult_bool[i] = false;
        //        }

        //    }
        //    else if ((compareResult_Copy[0] == false || compareResult_Copy[1] == false || compareResult_Copy[2] == false || compareResult_Copy[3] == false || compareResult_Copy[4] == false || compareResult_Copy[5] == false) && EventScore == 3)
        //    {
        //        Text_Tip[0].SetActive(true);

        //        for (int i = 0; i < Text_Tip.Length; i++)
        //        {
        //            Text_Tip[i].SetActive(false);
        //        }
        //        EventScore = 0;
        //        //Error_add.SetActive(true);

        //        fingerController.ReStart(Test_FingerPrint_Delegate);

        //    }
        //}
        //Debug.Log("Test_Raset");
    }



}
