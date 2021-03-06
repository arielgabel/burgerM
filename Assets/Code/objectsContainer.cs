﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objectsContainer : MonoBehaviour
{
    private bool m_isCollideWithPlayer = false;
    public Transform myFood;
    public GameObject myPlayer;
    public joyButton m_getInfo;
    Animator m_Animator;
    // Start is called before the first frame update
    public bool youCanClick = true;
    void Start()
    {
        m_Animator = GetComponent<Animator>();

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
            if (myPlayer.transform.childCount <= 2) // doesn't hold a thing
                Instantiate(myFood).transform.SetParent(myPlayer.transform);
        }
    }


    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            m_isCollideWithPlayer = true;
            if(m_Animator != null)
                m_Animator.SetBool("IsCollideWithPlayer", true);
            FindObjectOfType<AudioManager>().Play("OpeningBox");
        }
    }
    void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            m_isCollideWithPlayer = false;
            if(m_Animator != null)
                m_Animator.SetBool("IsCollideWithPlayer", false);
        }
    }
}
