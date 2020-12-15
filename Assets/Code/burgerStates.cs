using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class burgerStates : MonoBehaviour
{
    public float m_BurgerTime = 0;
    Animator m_animator;
    Color m_Burned, m_Medium, m_Ready, m_Rare;
    void Start()
    {
        m_animator = GetComponent<Animator>();
        m_Rare = this.gameObject.GetComponent<Renderer>().material.color;
        m_Medium = Color.HSVToRGB(0.023f, 0.647f, 0.747f);
        m_Ready = Color.HSVToRGB(0.044f, 0.707f, 0.567f);
        m_Burned = Color.HSVToRGB(0.004f, 1.000f, 0.000f);
    }


    void Update()
    {

        if(m_BurgerTime == 0)
        {
            if (this.transform.parent != null && transform.parent.name == "GRILL")
                m_BurgerTime = Time.realtimeSinceStartup;
            return;
        }
        else
        {
            float realTime = Time.realtimeSinceStartup;
            float updatedTime = realTime - m_BurgerTime;

            if (updatedTime < 5)
                this.gameObject.GetComponent<Renderer>().material.color = m_Rare;
            else if (updatedTime < 10)
                this.gameObject.GetComponent<Renderer>().material.color = m_Medium;
            else if(updatedTime < 15)
                this.gameObject.GetComponent<Renderer>().material.color = m_Ready;
            else
                this.gameObject.GetComponent<Renderer>().material.color = m_Burned;

        }

    }
}
