using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkerScript : MonoBehaviour
{
    public bool m_isCollideWithPlayer = false;
    public joyButton m_getInfo; // need to check how
    public bool youCanClick = true;
    public GameObject myPlayer;
    public Transform myFood;
    Animator m_AnimatorFirst, m_AnimatorSecond, m_AnimatorThird;
    Color m_Greencolor, m_RedColor;

    SpriteRenderer m_SpriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        m_AnimatorFirst = this.transform.GetChild(2).GetComponent<Animator>(); 
        m_AnimatorSecond = this.transform.GetChild(3).GetComponent<Animator>();
        m_AnimatorThird = this.transform.GetChild(4).GetComponent<Animator>();

        m_Greencolor = this.transform.GetChild(4).GetComponent<SpriteRenderer>().color;
        m_RedColor = new Color(1, 75/255, 40/255);
    }

    // Update is called once per frame
    void Update()
    {
        if (!youCanClick)
        {
            if (m_getInfo.ReturnIfPressed())
                return;
            youCanClick = true;
        }


        if (m_isCollideWithPlayer && m_getInfo.ReturnIfPressed()) // if theres a collision and the pickup button is pressed
        {
            youCanClick = false;
            if (myPlayer.transform.childCount > 2 && myPlayer.transform.GetChild(2).tag == "Plate") //player wants to put down a plate
            {
                Transform dish = myPlayer.transform.GetChild(2);
                if(dish.transform.childCount > 0) // the plate contains something
                {
                    
                    dish.transform.SetParent(this.transform);
                    dish.transform.localPosition = new Vector3(0.5f, 0.5f, 0f);
                    myFood = dish;


                    setAnimators(true);
                }    

            }
        }
    }

    void setAnimators(bool state)
    {
        m_AnimatorFirst.SetBool("DishOn", state);
        m_AnimatorSecond.SetBool("DishOn", state);
        m_AnimatorThird.SetBool("DishOn", state);
    }

    public void setGreenColor()
    {
        m_SpriteRenderer = this.transform.GetChild(2).GetComponent<SpriteRenderer>();
        m_SpriteRenderer.color = m_Greencolor;
        m_SpriteRenderer = this.transform.GetChild(3).GetComponent<SpriteRenderer>();
        m_SpriteRenderer.color = m_Greencolor;
        m_SpriteRenderer = this.transform.GetChild(4).GetComponent<SpriteRenderer>();
        m_SpriteRenderer.color = m_Greencolor;
    //    this.transform.GetChild(2).GetComponent<SpriteRenderer>().color = m_Greencolor;
    //    this.transform.GetChild(3).GetComponent<SpriteRenderer>().color = m_Greencolor;
     //   this.transform.GetChild(4).GetComponent<SpriteRenderer>().color = m_Greencolor;
    }

    public void setRedColor()
    {
        m_SpriteRenderer = this.transform.GetChild(2).GetComponent<SpriteRenderer>();
        m_SpriteRenderer.color = m_RedColor;
        m_SpriteRenderer = this.transform.GetChild(3).GetComponent<SpriteRenderer>();
        m_SpriteRenderer.color = m_RedColor;
        m_SpriteRenderer = this.transform.GetChild(4).GetComponent<SpriteRenderer>();
        m_SpriteRenderer.color = m_RedColor;
    }


    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            m_isCollideWithPlayer = true;
        }
    }
    void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            m_isCollideWithPlayer = false;
            setAnimators(false);
        }
    }
}
