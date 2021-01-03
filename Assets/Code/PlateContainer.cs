using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateContainer : MonoBehaviour
{
    private bool m_isCollideWithPlayer = false;
    public GameObject myPlayer;
    public joyButton m_getInfo;
    public bool youCanClick = true;

    public List<GameObject> m_Plates = new List<GameObject>();
    private int m_PlatesTaken = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (m_Plates.Count < 1)
            Debug.Log("have no plates!");
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
            if (myPlayer.transform.childCount <= 2 && m_PlatesTaken < 3) // doesn't hold a thing
            {
                FindObjectOfType<AudioManager>().Play("PlatePickUp");

                Instantiate(m_Plates[m_PlatesTaken]).transform.SetParent(myPlayer.transform);
                m_Plates[m_PlatesTaken].SetActive(false);
                m_PlatesTaken++;
            }
        }

        if(m_PlatesTaken == 3)
            Debug.Log("out of plates");
    }

    public int getHowManyPlatesTaken()
    {
        return m_PlatesTaken;
    }

    public void ShowPlates()
    {
        for(int i = 0; i < m_Plates.Count; i++)
        {
            m_Plates[i].SetActive(true);
        }
        m_PlatesTaken = 0;
    }


    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
            m_isCollideWithPlayer = true;
    }
    void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
            m_isCollideWithPlayer = false;
    }
}
