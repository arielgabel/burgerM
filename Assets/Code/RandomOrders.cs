using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
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
        m_MyOrdersTime.Add(Time.deltaTime);
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

        for(int i = 0; i <m_MyOrdersTime.Count; i++)
        {
            if (Time.realtimeSinceStartup - m_MyOrdersTime[i] > 30)
            {
                Animator orderAnimator;
                orderAnimator = myOrders[i].transform.GetChild(1).GetComponent<Animator>();
                orderAnimator.SetBool("Time", true);
            }
        }
        

       // if (myOrders.Count < 2)
       //      CreateNewOrder();
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
                    DestroyDish(true, i);
                    return;
                }
                else
                    m_RecievedDish.setRedColor();

            }
        }
        
        DestroyDish(false, -1);
    }

    void DestroyDish(bool WithOrder, int deleteOrder)
    {
        Destroy(m_RecievedDish.myFood.gameObject, 0.3f);
        m_RecievedDish.myFood = null;


        if(WithOrder)
        { // it's not m_currectOrder - its the order that i deleted!!!
            Destroy(myOrders[deleteOrder].gameObject, 0.3f);
            myOrders.Remove(myOrders[deleteOrder]); // remove gameobject
            m_MyOrdersLocation.Remove(m_MyOrdersLocation[deleteOrder]);
            m_MyOrdersTime.Remove(m_MyOrdersTime[deleteOrder]);
            m_CurrentOrder--;
        }

        /*
        if (m_CurrentOrder >= 0 && WithOrder)
        { // it's not m_currectOrder - its the order that i deleted!!!
            Destroy(myOrders[m_CurrentOrder].gameObject, 0.3f);
            myOrders.Remove(myOrders[m_CurrentOrder]); // remove gameobject
            m_MyOrdersLocation.Remove(m_MyOrdersLocation[m_CurrentOrder]);
            m_MyOrdersTime.Remove(m_MyOrdersTime[m_CurrentOrder]);
            m_CurrentOrder--;
        }*/
    }
}
