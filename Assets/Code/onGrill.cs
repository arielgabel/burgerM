using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class onGrill : MonoBehaviour
{
    public bool isCollideWithPlayer = false;
    public GameObject myPlayer;
    public joyButton m_getInfo;
    public Transform myFood;
    public bool m_onGrill = false;
    public float m_timeBetweenTrades;

    Animator burgerAnimator;
    void Start()
    {

        
    }
   
    // Update is called once per frame
    void Update()
    {
 
        if (isCollideWithPlayer && m_getInfo.ReturnIfPressed()) // if theres a collision and the pickup button is pressed
        {
            if (m_onGrill)
            {
                if (myPlayer.transform.childCount <= 2 && Time.realtimeSinceStartup >= m_timeBetweenTrades + 1)
                {
                    //יש מירוץ בין לקחת את האובייקט מהקופסא ולהחזיר אותו לקופסא
                    //רק בכאלו שהם בלי אוכל, לא בקופסאות שמביאות אוכל
                    //לחשוב על משהו יפה
                    myFood.transform.SetParent(myPlayer.transform);
                    this.myFood = null;
                }
                return;
            }
               
            for (int i = 0; i < myPlayer.transform.childCount; i++)
            {
                Transform child = myPlayer.transform.GetChild(i);
                if (child.name == "Burger(Clone)") // grill accepts only Burgers!!!
                {
                    
                    child.transform.SetParent(this.transform); //parent is grill
                    this.myFood = child;

                    m_onGrill = true;

                    myFood.transform.localPosition = new Vector3(0f, 0.44f, 0.9f);
                    m_timeBetweenTrades = Time.realtimeSinceStartup;
                    break;
                }

            }
        }
    }
    void handleBurger()
    {


    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isCollideWithPlayer = true;
        }
    }
    void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isCollideWithPlayer = false;
        }
    }
}
