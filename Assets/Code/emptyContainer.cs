using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class emptyContainer : MonoBehaviour
{
    public bool m_takeFrom = false;
    public bool isCollideWithPlayer = false;
    public Transform myFood;
    public GameObject myPlayer;
    public joyButton m_getInfo; // need to check how
    Color m_Color;
    public float m_timeBetweenTrades;

    void Start()
    {
        m_Color = this.gameObject.GetComponent<Renderer>().material.color;
    }


    // Update is called once per frame
    void Update()
    {
        if (isCollideWithPlayer && m_getInfo.ReturnIfPressed()) // if theres a collision and the pickup button is pressed
        {
            if(m_takeFrom)
            {
                if (myPlayer.transform.childCount <= 2 && Time.realtimeSinceStartup >= m_timeBetweenTrades + 1)
                {
                    Debug.Log("takeee");
                        //יש מירוץ בין לקחת את האובייקט מהקופסא ולהחזיר אותו לקופסא
                        //רק בכאלו שהם בלי אוכל, לא בקופסאות שמביאות אוכל
                        //לחשוב על משהו יפה
                       myFood.transform.SetParent(myPlayer.transform);
                       m_takeFrom = false;
                       this.myFood = null;
                }

                //צריכים לחשב יותר טוב איפה שמים כל אובייקט מבחינת המרכיבים בהמבורגר
                else if(myPlayer.transform.childCount  == 3 && myFood.tag == "Plate") // object on plate
                {
                    Transform child = myPlayer.transform.GetChild(2);
                    if(child.tag == "Food") // כי אני לא רוצה לשים צלחת על צלחת, אין היגיון
                    {
                        for(int i = 0; i <myFood.childCount; i++)
                        {
                            Transform myBread = myFood.transform.GetChild(i);

                            //אם יש לחמניה בצלחת, והאובייקט שהשחקן רוצה להניח הוא לא לחמניה(אין היגיון בלשים לחמניה על לחמניה)
                            if(myBread.name == "Bread(Clone)" && child.name != "Bread(Clone)") 
                            {
                                for (int j = 0; j < myBread.childCount; j++)
                                {
                                    Transform myTopBun = myBread.transform.GetChild(j);
                                    if (myTopBun.tag == "TopBun") // אני רוצה להעלות את הלחמניה העליונה ולשים את האובייקט באמצע
                                    {
                                        myTopBun.transform.localPosition = new Vector3(
                                            myTopBun.transform.localPosition.x,
                                            myTopBun.transform.localPosition.y + 0.2f, // move up a little
                                            myTopBun.transform.localPosition.z);
                                    }
                                }
                            }
                        }
                        child.transform.SetParent(myFood.transform);
                        if(child.name == "Burger(Clone)")
                            child.transform.localPosition = new Vector3(0f, 0.161f, 0f);
                        else // if it's a plate(need to think about something better ofcourse
                            child.transform.localPosition = new Vector3(0f, 0f, 0f);
                        m_timeBetweenTrades = Time.realtimeSinceStartup;
                    }
                }
            }
            else
            {
                for (int i = 0; i < myPlayer.transform.childCount; i++)// שחקן מניח אובייקט
                {
                    Transform child = myPlayer.transform.GetChild(i);
                    if (child.tag == "Food" || child.tag == "Plate") // if theres a Food/plate child
                    {
                        child.transform.SetParent(this.transform);
                        this.myFood = child;
                        // myFood.transform.localPosition = Vector3.zero;
                        if(child.tag == "Food")
                           myFood.transform.localPosition = new Vector3(0f, 0f, 1.1f);
                        else // זו צלחת
                            myFood.transform.localPosition = new Vector3(0f, 0f, 1.2f);
                        m_takeFrom = true;

                        m_timeBetweenTrades = Time.realtimeSinceStartup; // object moved from player to container
                        break;
                    }

                }
                
            }
        }
        else
        {
            //maybe need maybe not. probabely need to put back the item
            // forDebug = true;
        }
    }

    
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isCollideWithPlayer = true;
            this.gameObject.GetComponent<Renderer>().material.color = Color.HSVToRGB(0.07f, 0.584f, 0.71f);
        }
    }
    void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isCollideWithPlayer = false;
            this.gameObject.GetComponent<Renderer>().material.color = m_Color;
        }
    }


}
