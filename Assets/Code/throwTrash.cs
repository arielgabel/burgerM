using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class throwTrash : MonoBehaviour
{

    public bool isCollideWithPlayer = false;
    public GameObject myPlayer;
    public joyButton m_getInfo;
    public bool youCanClick = true;

    // Update is called once per frame
    void Update()
    {
        if (!youCanClick)
        {
            if (m_getInfo.ReturnIfPressed())
                return;
            youCanClick = true;
        }


        if (isCollideWithPlayer && m_getInfo.ReturnIfPressed()) // if theres a collision and the pickup button is pressed
        {
            youCanClick = false;
            if(myPlayer.transform.childCount > 2) // 
            {
                FindObjectOfType<AudioManager>().Play("ThrowTrash");
                Transform myChild = myPlayer.transform.GetChild(2);
                if (myChild.tag != "Plate")
                    Destroy(myChild.gameObject);
                else
                {
                    for(int i = 0; i < myChild.childCount; i++)
                        Destroy(myChild.GetChild(i).gameObject);
                }
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
