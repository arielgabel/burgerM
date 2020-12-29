using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderHandler : MonoBehaviour
{
    bool m_State = false;
    float m_timeOfActive = 0;
    public void setIngradients()
    {
        m_State = !m_State;
        this.transform.GetChild(0).gameObject.SetActive(m_State);
        Debug.Log(m_State);
        if (m_State)
            m_timeOfActive = Time.realtimeSinceStartup;
    }

    public float getActiveTime()
    {
        if (!this.transform.GetChild(0).gameObject.activeInHierarchy)
        {
            m_timeOfActive = 0;
            m_State = false;
        }
           
        return m_timeOfActive;
    }

    public bool getState()
    {
        return m_State;
    }
}
