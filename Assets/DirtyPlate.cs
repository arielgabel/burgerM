using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirtyPlate : MonoBehaviour
{

    public List<GameObject> m_DirtyPlates = new List<GameObject>();
    public PlateContainer m_CleanPlates;
    private int numOfPlatesInGame = 0;
    private bool m_ActiveState = false;
    // Start is called before the first frame update
    void Awake()
    {
        m_CleanPlates = FindObjectOfType<PlateContainer>();
        if (m_DirtyPlates.Count < 1)
            Debug.Log("have no plates!"); // for debugging
        else
            changePlatesActiveState(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (m_CleanPlates.getHowManyPlatesTaken() == 3 && GameObject.FindGameObjectsWithTag("Plate").Length == 0)
        {
            changePlatesActiveState(true);
        }
    }

    public bool getState()
    {
        return m_ActiveState;
    }


    public void changePlatesActiveState(bool state)
    {
        for (int i = 0; i < m_DirtyPlates.Count; i++) // to be sure
        {
            m_DirtyPlates[i].SetActive(state);
        }
        m_ActiveState = state;
    }
}
