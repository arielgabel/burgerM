using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerMovement : MonoBehaviour
{
    public joyButton m_btn;

    protected Joystick joystick;
    Vector3 m_movement;
    Quaternion m_Rotation = Quaternion.identity;
    Rigidbody m_rigidbody;

    Animator m_Animator;
    public float turnSpeed = 20f;
    void Start()
    {

        m_Animator = GetComponent<Animator>();
        joystick = FindObjectOfType<Joystick>();
        m_rigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        m_Animator.SetBool("isButtonClicked", m_btn.ReturnIfPressed());
        float horizontal = joystick.Horizontal * 10f;
        float vertical = joystick.Vertical * 10f;

        m_rigidbody.velocity = new Vector3(joystick.Vertical * -10,
                                                m_rigidbody.velocity.y,
                                                joystick.Horizontal * 10);
 

        bool hasHorizontalInput = !Mathf.Approximately(vertical, 0f);
        bool hasVerticalInput = !Mathf.Approximately(horizontal, 0f);
        bool isWalking = hasHorizontalInput || hasVerticalInput;
        
        
        for(int i = 0; i < this.transform.childCount; i++) // going threw all childs
        {
            Transform child = this.transform.GetChild(i);
            if(child.tag == "Food" || child.tag == "Plate") // if theres a Food child, then player isHoling, and break
            {
                m_Animator.SetBool("IsHolding", true);
                Vector3 playerLoc = this.transform.localPosition;

               

                if (m_Animator.GetCurrentAnimatorStateInfo(0).IsName("standingWith"))
                {
                    if(child.tag == "Food")
                        child.localPosition = new Vector3(playerLoc.x + 10, playerLoc.y + 45, playerLoc.z + 30);
                    else
                        child.localPosition = new Vector3(playerLoc.x + 20, playerLoc.y + 60, playerLoc.z + 40);

                }

                break;
                //10, 45, 30
            }
            m_Animator.SetBool("IsHolding", false); // if never break, then 
        }

        m_Animator.SetBool("IsWalking", isWalking);


        Vector3 desiredForward = Vector3.RotateTowards
            (transform.forward, m_rigidbody.velocity, turnSpeed * Time.deltaTime, 0f);
        m_Rotation = Quaternion.LookRotation(desiredForward);
    }

    public void OnAnimatorMove()
    {

        m_rigidbody.MovePosition
            (m_rigidbody.position + m_rigidbody.velocity * m_Animator.deltaPosition.magnitude);
        m_rigidbody.MoveRotation(m_Rotation);
    }

}
