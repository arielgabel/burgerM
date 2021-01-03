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

    public bool youCanClick = true;
    Animator TimeUpAnimator;
    burgerStates myBurger;

    void Start()
    {
        TimeUpAnimator = this.transform.GetChild(2).GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(myFood == null)
        {
            TimeUpAnimator.SetBool("TimeIsUp", false);
        }
        else
        {
            if (!FindObjectOfType<AudioManager>().IsPlaying("BurgerOnGrill"))
            {
                Debug.Log("complete firsti audio");
                if (!FindObjectOfType<AudioManager>().IsPlaying("OnGrill"))
                {
                    //FindObjectOfType<AudioManager>().PlayS("OnGrill");
                    FindObjectOfType<AudioManager>().Play("OnGrill");

                }
                    
            } 
        }

        if (!youCanClick) 
        {
            if(m_getInfo.ReturnIfPressed())
                return;
            youCanClick = true;
        }
        if (isCollideWithPlayer && m_getInfo.ReturnIfPressed()) // if theres a collision and the pickup button is pressed
        {
            youCanClick = false;
            if (m_onGrill)
            {
                if (myPlayer.transform.childCount <= 2)
                {
                    //יש מירוץ בין לקחת את האובייקט מהקופסא ולהחזיר אותו לקופסא
                    //רק בכאלו שהם בלי אוכל, לא בקופסאות שמביאות אוכל
                    //לחשוב על משהו יפה
                    myFood.transform.SetParent(myPlayer.transform);
                    FindObjectOfType<AudioManager>().Stop("OnGrill");
                    this.myFood = null;
                    m_onGrill = false;
                }
                return;
            }

            Transform myChild = myPlayer.transform.Find("Burger(Clone)").transform;
            if (myChild != null) // grill accepts only Burgers!!!
            {
                FindObjectOfType<AudioManager>().Play("BurgerOnGrill");

                myChild.transform.SetParent(this.transform); //parent is grill
                this.myFood = myChild;

                m_onGrill = true;
                myFood.transform.localPosition = new Vector3(0f, 0.44f, 0.9f);
                myBurger = this.myFood.GetComponent<burgerStates>();
                TimeUpAnimator.SetBool("TimeIsUp", true);
            }
        }
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
