using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class RandomOrders : MonoBehaviour
{
    public GameObject[] Orders;

    public Transform myCanvas;
    int randomInt, m_CurrentOrder = -1;

    RectTransform m_RectTransform;
    checkerScript m_RecievedDish;
    // public GameObject myOrder = null;

    public List<GameObject> myOrders = new List<GameObject>();
    public List<int> m_MyOrdersLocation = new List<int>();
    public List<float> m_MyOrdersTime = new List<float>();
    int XPos = 20, YPos = -20;


    public CoinCollection m_CoinCollection;
    private float outOfTime = 0;


    
    void Start()
    {

        m_RecievedDish = GetComponent<checkerScript>();
        CreateNewOrder();
        CreateNewOrder();
        CreateNewOrder();
    }

    void CreateNewOrder()
    {
        randomInt = Random.Range(0, Orders.Length);

        myOrders.Add(Instantiate(Orders[randomInt]) as GameObject);
        m_MyOrdersTime.Add(Time.realtimeSinceStartup);
        m_CurrentOrder++;
        m_RectTransform = myOrders[m_CurrentOrder].GetComponent<RectTransform>(); // for image
        myOrders[m_CurrentOrder].transform.SetParent(myCanvas.transform);


        handleOnjectSizeNLocation(m_RectTransform);
    }

    void handleOnjectSizeNLocation(RectTransform m_RectTransform)
    {
        m_RectTransform.anchoredPosition = new Vector2(XPos + (160 * m_CurrentOrder), YPos);
        m_MyOrdersLocation.Add(XPos + (220 * m_CurrentOrder));

        m_RectTransform.anchorMin = new Vector2(0, 1);
        m_RectTransform.anchorMax = new Vector2(0, 1);
        m_RectTransform.pivot = new Vector2(0, 1);

        m_RectTransform.localScale = new Vector3(2, 2, 2);
    }


    // Update is called once per frame
    void Update()
    {
        if (m_RecievedDish.myFood != null) // there is food on checker
        {
            if (m_CurrentOrder >= 0)
                CompareThem();
            else
                DestroyDish(false, -1);
        }
        else
            checkForPressedOrders();

        outOfTime = 0;
        for (int i = 0; i <m_MyOrdersTime.Count; i++)
        {
            if (Time.realtimeSinceStartup - m_MyOrdersTime[i] > myOrders[i].GetComponent<OrderHandler>().orderTime)
            {
                outOfTime++;
                if (!FindObjectOfType<AudioManager>().IsPlaying("OrderTimeOut"))
                    FindObjectOfType<AudioManager>().Play("OrderTimeOut");

                Animator orderAnimator;
                orderAnimator = myOrders[i].transform.GetChild(1).GetComponent<Animator>();
                orderAnimator.SetBool("Time", true);
            }
        }
        if(outOfTime == 0)
            FindObjectOfType<AudioManager>().Stop("OrderTimeOut");

        checkForOrdersToCreate();
    }

    void checkForOrdersToCreate()
    {
        if (myOrders.Count == 0)
            CreateNewOrder();

        else if (myOrders.Count < 3)
        {
            randomInt = Random.Range(0, 10000);
            if (randomInt % 9999 == 0)
            {
                CreateNewOrder();
            }
        }
    }

    void checkForPressedOrders()
    {
        int indexOfActive = -1;
        float maxTime = 0, timeOfActive;
        for(int i = 0; i < myOrders.Count; i++)
        {
            timeOfActive = myOrders[i].transform.GetComponent<OrderHandler>().getActiveTime();
            if (maxTime < timeOfActive)
            {
                maxTime = timeOfActive;
                indexOfActive = i;
            }
        }
        if(indexOfActive != -1) // there is an active order
        {
            for (int i = 0; i < myOrders.Count; i++) // going threw all orders
            {
                if (myOrders[i] != myOrders[indexOfActive])
                    myOrders[i].transform.GetChild(0).gameObject.SetActive(false);
                MoveToTheLeft();
            }

            MoveToTheRight(indexOfActive); // change position of the following orders
            return;
        }

        MoveToTheLeft();


    }

    void MoveToTheLeft()
    {
        for (int i = 0 ; i < myOrders.Count; i++) // change position back
        {
            m_RectTransform = myOrders[i].GetComponent<RectTransform>();
            m_RectTransform.anchoredPosition = new Vector2(m_MyOrdersLocation[i], YPos);
        }
    }

    void MoveToTheRight(int i)
    {
        for(int order = i + 1; order < myOrders.Count; order++)
        {
            m_RectTransform = myOrders[order].GetComponent<RectTransform>(); // for image

            if (m_MyOrdersLocation[order] != m_RectTransform.anchoredPosition.x)
                break;
            m_RectTransform.anchoredPosition = new Vector2(m_RectTransform.anchoredPosition.x + 160, YPos);
           
        }
    }

    bool findAMatch(Transform order)
    {
        Transform dishObjects = m_RecievedDish.myFood.transform; //the dish that is on checker
        if (dishObjects.childCount != order.childCount)
            return false;
        for (int OrderIngradient = 0; OrderIngradient < order.childCount; OrderIngradient++)
        {
            string dishName = order.GetChild(OrderIngradient).name; // the name in variable

            if (dishObjects.transform.Find(dishName + "(Clone)") == null) //null so doesn't exists
                return false;
            
        }
        return true;
    }

    void CompareThem()
    {
        if (m_RecievedDish.myFood.tag == "Plate") // it's a plate
        {
            for(int i = m_CurrentOrder; i >= 0; i--)
            {
                if (findAMatch(myOrders[i].transform.GetChild(0)))
                {
                    m_RecievedDish.setGreenColor();
                    FindObjectOfType<AudioManager>().Play("CorrectOrder");

                    m_CoinCollection.StartCoinMove(m_RecievedDish.myFood.transform.position, 5);
                    DestroyDish(true, i);
                    return;
                }
                else
                {
                    FindObjectOfType<AudioManager>().Play("WrongOrder");
                    m_RecievedDish.setRedColor();
                }
                   

            }
        }
        
        DestroyDish(false, -1);
    }

    void DestroyDish(bool WithOrder, int deleteOrder)
    {
        Destroy(m_RecievedDish.myFood.gameObject, 0.3f);
        m_RecievedDish.myFood = null;


        if(WithOrder)
        { 
            Destroy(myOrders[deleteOrder].gameObject, 0.3f);
            myOrders.Remove(myOrders[deleteOrder]); // remove gameobject
            moveBackOtherOrders(deleteOrder);
            m_MyOrdersLocation.Remove(m_MyOrdersLocation[deleteOrder]);
            m_MyOrdersTime.Remove(m_MyOrdersTime[deleteOrder]);
            m_CurrentOrder--;
        }
    }

    void moveBackOtherOrders(int endHere)
    {
        for (int i = m_MyOrdersLocation.Count - 1; i > endHere; i--)
            m_MyOrdersLocation[i] = m_MyOrdersLocation[i - 1];
        
    }
}
