using UnityEngine.EventSystems;
using UnityEngine;

public class joyButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [HideInInspector]
    private bool m_pressed = false;
    float hi;
    // Start is called before the first frame update
    void Start()
    {
        
    }
   

    public void OnPointerDown(PointerEventData eventData)
    {
        m_pressed = true;
        Debug.Log("Mouse down");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("Mouse UP");
        m_pressed = false;
    }
    

    public bool ReturnIfPressed()
    {
        return m_pressed;
    }

}