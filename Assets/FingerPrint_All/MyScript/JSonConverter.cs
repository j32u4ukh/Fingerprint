using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class JSonConverter : MonoBehaviour
{
	#region Instance
	static JSonConverter instance = null;

	public static JSonConverter Instance { get { return instance; } }
    #endregion

    void Awake()
    {
        Debug.Log("JSonConverter Awake ");
        instance = this;
    }

    // Use this for initialization
    void Start ()
	{
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public T GetJsonData<T>(string str)
	{
		return JsonUtility.FromJson<T>(str);
	}

	public List<T> GetListFormJsonString<T>(string str)
	{
		Debug.Log("str = " + str);
		Wrapper<T> wrapper = UnityEngine.JsonUtility.FromJson<Wrapper<T>>(str);
		Debug.Log("wrapper = " + wrapper.results);
		return wrapper.results;
	}

	[Serializable]
	private class Wrapper<T>
	{
        public List<T> results;
	}

	public string ToJson<T>(List<T> array, bool prettyPrint = false)
	{
		Wrapper<T> wrapper = new Wrapper<T>();
		wrapper.results = array;
		return JsonUtility.ToJson(wrapper, prettyPrint);
	}

}


