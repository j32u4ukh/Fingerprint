using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Button_Totxh_Event : MonoBehaviour
{
    FingerprintIdentification_to_Uniy.Class1 fingerController = null;


    public GameObject Menu_Close;

    public GameObject add_UI_Open;

    public GameObject FingIn_UI_Open;

    public GameObject remind_Open;

    public GameObject IndividuaDelete;

    public bool Delete_Fing;

    public bool Event_CloseUpdata;

     


    public void Awake()
    {
       // fingerController = new FingerprintIdentification_to_Uniy.Class1();

    }

    //進入註冊介面
    public void Add_Event_button()
    {
        //GameObject.Find("GameManager_Fingprint").GetComponent<FingPrintcCode>().load();
        //GameObject.Find("GameManager_Fingprint").GetComponent<PagesController>().fingerController.StartLoad();
        //GameObject.Find("GameManager_Fingprint").GetComponent<PagesController>().fingerController.ReStart(GameObject.Find("GameManager_Fingprint").GetComponent<PagesController>().Test_FingerPrint_Delegate);
        //GameObject.Find("GameManager_Fingprint").GetComponent<PagesController>().fingerController.CancelCaptureAndCloseReader(fingerController.OnCaptured);


        GameObject.Find("GameManager_Fingprint").GetComponent<PagesController>().Add_bool = true;

        add_UI_Open.SetActive(true);
        Menu_Close.SetActive(false);
    }
    //進入登入介面
    public void GIn_Event_button()
    {
        //GameObject.Find("GameManager_Fingprint").GetComponent<PagesController>().fingerController.ReStart(GameObject.Find("GameManager_Fingprint").GetComponent<PagesController>().Test_FingerPrint_Delegate);
        if (GameObject.Find("GameManager_Fingprint").GetComponent<PagesController>().fingerPointData.Count >=1)
        {
            GameObject.Find("GameManager_Fingprint").GetComponent<PagesController>().LoadFingerData();

        }

        GameObject.Find("GameManager_Fingprint").
            GetComponent<PagesController>().
            fingerController.
            ReStart(GameObject.Find("GameManager_Fingprint").
            GetComponent<PagesController>().
            Test_FingerPrint_Delegate);


        GameObject.Find("GameManager_Fingprint").GetComponent<PagesController>().InSignIn_bool = true;



        //GameObject.Find("GameManager_Fingprint").GetComponent<PagesController>().fingerController.ReStart(GameObject.Find("GameManager_Fingprint").GetComponent<PagesController>().Test_FingerPrint_Delegate);

        FingIn_UI_Open.SetActive(true);
        Menu_Close.SetActive(false);
        //fingerController.ReStart();

    }
    //返回Menu
    public void Back_Menu()
    {
        //GameObject.Find("GameManager_Fingprint").GetComponent<PagesController>().fingerController.StartLoad();

        GameObject.Find("GameManager_Fingprint").GetComponent<PagesController>().fingerController.CancelCaptureAndCloseReader(GameObject.Find("GameManager_Fingprint").GetComponent<PagesController>().fingerController.OnCaptured);

        GameObject.Find("GameManager_Fingprint").GetComponent<PagesController>().Fing_Tip_Close.SetActive(true);
        GameObject.Find("GameManager_Fingprint").GetComponent<PagesController>().FingError_SingIn.SetActive(false);
        //fingerController.CancelCaptureAndCloseReader(fingerController.OnCaptured);
        //GameObject.Find("GameManager_Fingprint").GetComponent<PagesController>().fingerController.StartLoad();
        //GameObject.Find("GameManager_Fingprint").GetComponent<PagesController>().fingerController.ReStart(GameObject.Find("GameManager_Fingprint").GetComponent<PagesController>().Test_FingerPrint_Delegate);

        GameObject.Find("GameManager_Fingprint").GetComponent<PagesController>().InSignIn_bool = false;


        //SceneManager.LoadScene("Menu_20190125");

        // SceneManager.LoadScene("Menu");
        FingIn_UI_Open.SetActive(false);
        Menu_Close.SetActive(true);
    }
    //確定全部刪除
    public void remind_Menu()
    {
        remind_Open.SetActive(true);
    }
    //取消全部刪除
    public void remindBack_Menu()
    {

        remind_Open.SetActive(false);
    }
    //登入返回Menu
    public void Add_Event_Menu()
    {
        Debug.Log("add_Event");
        GameObject.Find("GameManager_Fingprint").GetComponent<PagesController>().Input_Go.GetComponent<InputField>().text = "";
        GameObject.Find("GameManager_Fingprint").GetComponent<PagesController>().Input_Go.SetActive(true);
        GameObject.Find("GameManager_Fingprint").GetComponent<PagesController>().Image_Go.SetActive(false);
        GameObject.Find("GameManager_Fingprint").GetComponent<PagesController>().Error_Fing_obj.SetActive(false);

        for (int i = 0; i < GameObject.Find("GameManager_Fingprint").GetComponent<PagesController>().Text_Tip.Length; i++)
        {
            GameObject.Find("GameManager_Fingprint").GetComponent<PagesController>().Text_Tip[i].SetActive(false);
        }
        //GameObject.Find("GameManager_Fingprint").GetComponent<PagesController>().Text_Tip[0].SetActive(true);
        //fingerController.CancelCaptureAndCloseReader(fingerController.OnCaptured);
        //GameObject.Find("GameManager_Fingprint").GetComponent<PagesController>().fingerController.ReStart(GameObject.Find("GameManager_Fingprint").GetComponent<PagesController>().Test_FingerPrint_Delegate);
        GameObject.Find("GameManager_Fingprint").GetComponent<PagesController>().fingerController.CancelCaptureAndCloseReader(GameObject.Find("GameManager_Fingprint").GetComponent<PagesController>().fingerController.OnCaptured);

        //GameObject.Find("GameManager_Fingprint").GetComponent<PagesController>().fingerController.StartLoad();

        //GameObject.Find("GameManager_Fingprint").GetComponent<PagesController>().fingerController.CancelCaptureAndCloseReader(fingerController.OnCaptured);
        GameObject.Find("GameManager_Fingprint").GetComponent<PagesController>().Add_bool = false;

        // SceneManager.LoadScene("Menu_20190125");

        //GameObject.Find("GameManager_Fingprint").GetComponent<FingPrintcCode>().load();

        add_UI_Open.SetActive(false);
        Menu_Close.SetActive(true);

    }
    //登入返回Menu
    public void Delete_one_Event_Menu()
    {
        //fingerController.CancelCaptureAndCloseReader(fingerController.OnCaptured);

        GameObject.Find("GameManager_Fingprint").GetComponent<PagesController>().Lock_Ins = false;
        //SceneManager.LoadScene("Menu_20190125");

        Event_CloseUpdata = false;
        // SceneManager.LoadScene("Menu");

        //GameObject.Find("GameManager_Fingprint").GetComponent<FingPrintcCode>().load();

        IndividuaDelete.SetActive(false);
        Menu_Close.SetActive(true);

    }
    //Menu to Delete_one
    public void Delete_one_Event_Go()
    {
        GameObject.Find("GameManager_Fingprint").GetComponent<PagesController>().Lock_Ins = true;

        Event_CloseUpdata = true;

        IndividuaDelete.SetActive(true);
        Menu_Close.SetActive(false);
        GameObject.Find("GameManager_Fingprint").GetComponent<PagesController>().ChangePage(1) ;
        GameObject.Find("GameManager_Fingprint").GetComponent<PagesController>().LoadFingerData();

    }
    public void Delete_All()
    {
 
        //File.Delete(@"D:\ID\datafingprint.txt");

        //string QQ;
        


        // GameObject.Find("GameManager_Fingprint").GetComponent<FingPrintcCode>().fingerPointData.Clear();
    }
    public void Start_add()
    {
        add_UI_Open.SetActive(true);
        FingIn_UI_Open.SetActive(false);
    }
    //刪除案件1

}
