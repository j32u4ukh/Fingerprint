using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class TestDir : MonoBehaviour {

    public GameObject sender;

    public readonly string Path = @"D:\ID";
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Directory.Exists(Path))
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

    }
   
}

