using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerMovement : MonoBehaviour
{
    AudioManager forWalking;

    public joyButton m_btn;

    protected Joystick joystick;
    Vector3 m_movement;
    Quaternion m_Rotation = Quaternion.identity;
    Rigidbody m_rigidbody;
    public bool m_sinkCollide = false;
    public float washDishes = 0;
    DirtyPlate m_DirtyPlates;
    Animator m_Animator;
    public float turnSpeed = 20f;
    void Start()
    {
        m_DirtyPlates = FindObjectOfType<DirtyPlate>();

        m_Animator = GetComponent<Animator>();
        joystick = FindObjectOfType<Joystick>();
        m_rigidbody = GetComponent<Rigidbody>();

        forWalking = FindObjectOfType<AudioManager>();
    }

    void FixedUpdate()
    {
        if (m_sinkCollide)
            handleSinkAction();

        if(!m_Animator.GetBool("timeToWash"))
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




        if (isWalking)
        {
            if (!forWalking.IsPlaying("PlayerWalk"))
                forWalking.Play("PlayerWalk");
        }
        else
            forWalking.Stop("PlayerWalk");
           


        Vector3 desiredForward = Vector3.RotateTowards
            (transform.forward, m_rigidbody.velocity, turnSpeed * Time.deltaTime, 0f);
        m_Rotation = Quaternion.LookRotation(desiredForward);
    }

    public void handleSinkAction()
    {
        if (m_btn.ReturnIfPressed() && m_DirtyPlates.getState())
        {
            if (this.transform.childCount == 2) // doesn't hold a thing
            {
                m_Animator.SetBool("timeToWash", true);
                if (!FindObjectOfType<AudioManager>().IsPlaying("WashingDishes"))
                    FindObjectOfType<AudioManager>().Play("WashingDishes");

                if(washDishes == 0)
                    washDishes = Time.realtimeSinceStartup;
            }
        }

        if (washDishes > 0)
        {
            m_Animator.SetFloat("washTime", Time.realtimeSinceStartup - washDishes);
            if (Time.realtimeSinceStartup - washDishes > 8)
            {
                m_DirtyPlates.changePlatesActiveState(false);
                m_DirtyPlates.m_CleanPlates.ShowPlates();
                //להעלים צלחות מלוכלכות
                //להראות צלחות נקיות
                m_Animator.SetBool("timeToWash", false);
                washDishes = 0;
                FindObjectOfType<AudioManager>().Stop("WashingDishes");
            }
                
        }
    }

    public void OnAnimatorMove()
    {

        m_rigidbody.MovePosition
            (m_rigidbody.position + m_rigidbody.velocity * m_Animator.deltaPosition.magnitude);
        m_rigidbody.MoveRotation(m_Rotation);
    }


    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Sink"))
            m_sinkCollide = true;
    }
    void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Sink"))
        {
            m_Animator.SetBool("timeToWash", false);
            FindObjectOfType<AudioManager>().Stop("WashingDishes");
            m_sinkCollide = false;
        }
    }

}
