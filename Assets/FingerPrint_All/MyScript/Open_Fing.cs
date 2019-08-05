using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Open_Fing : MonoBehaviour
{
    public GameObject UI_Fing;

    public GameObject UI_1;

    public void Open_Fing_Button()
    {
        this.gameObject.GetComponent<FingPrintcCode>().determine_bool = true;
        // GameObject.Find("GameManager_Fingprint").GetComponent<PagesController>().fingerController.StartLoad();
        //GameObject.Find("GameManager_Fingprint").GetComponent<PagesController>().fingerController.ReStart();
        for (int j = 0; j < GameObject.Find("GameManager_Fingprint").GetComponent<PagesController>().Text_Tip.Length; j++)
        {
            GameObject.Find("GameManager_Fingprint").GetComponent<PagesController>().Text_Tip[j].SetActive(false);
        }
        GameObject.Find("GameManager_Fingprint").GetComponent<PagesController>().Text_Tip[0].SetActive(true);
        GameObject.Find("GameManager_Fingprint").GetComponent<PagesController>().Add_bool = true;
        GameObject.Find("GameManager_Fingprint").GetComponent<PagesController>().Start_GO();
        GameObject.Find("GameManager_Fingprint").GetComponent<PagesController>().Ok_Button_Save.SetActive(false);

        //UI_Fing.SetActive(true);
        UI_1.SetActive(false);
    }

}
