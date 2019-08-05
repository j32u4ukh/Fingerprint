using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Father_obj : MonoBehaviour {

    Text nameObj = null;
    public int id = -1;

    public Button deleteBtn = null;

    public bool choose = false;

    //public GameObject children_gameObject;   //宣告一個GameObject(用來放取得的子物件)。

    void Start()
    {    //一開始就執行。

        //children_gameObject = gameObject.transform.GetChild(1).gameObject;

        //// 宣告的物件 = 取得本身的第一個子物件。



        //Debug.Log(children_gameObject.name);   //Debug子物件的名稱。

        

    }

    public void SetData(string name, int id)
    {
        transform.Find("Name").GetComponent<Text>().text = name;
        this.id = id;
    }

    public void Delete()
    {
        if (choose == true)
        {
            transform.Find("Go").gameObject.SetActive(false);

        }
        else if(choose == false)
        {
            transform.Find("Go").gameObject.SetActive(true);
        }
        choose = !choose;
       // deleteBtn.GetComponent<Image>()

    }
}
