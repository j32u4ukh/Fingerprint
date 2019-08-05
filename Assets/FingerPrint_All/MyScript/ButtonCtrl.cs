using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

public class ButtonCtrl : MonoBehaviour
{

    public Sprite img;

    public Transform[] t;

    public GameObject[] g;

    public GameObject[] Button_pos;

    public Text[] Text_button;

    void Start()
    {
     
        for (int i = 0; i < t.LongLength; i++)
        {
            g[i] = new GameObject("My Button", typeof(RectTransform), typeof(Image), typeof(Button));
            t[i] = g[i].transform;
            t[i].SetParent(transform);
            t[i].localPosition = Button_pos[i].transform.localPosition ;

            Image img2 = g[i].GetComponent<Image>();
            img2.sprite = img;
            img2.SetNativeSize();

            Button.ButtonClickedEvent e = new Button.ButtonClickedEvent();
            e.AddListener(Kiss);

            Button b = g[i].GetComponent<Button>();
            b.onClick = e;
        }
        

      

    }

    public void Kiss()
    {
        print("Kiss");
    }

}