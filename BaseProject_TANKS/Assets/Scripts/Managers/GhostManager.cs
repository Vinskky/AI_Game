using System;
using UnityEngine;

[Serializable]

public class GhostManager
{
    // Start is called before the first frame update

    public Transform m_SpawnPoint;
    [HideInInspector] public GameObject m_Instance;

    private Patrol m_Patrol;

    public void Setup()
    {
        //Game starts with disabled render for the ghost Game Object
        m_Instance.GetComponent<Renderer>().enabled = false;
        m_Patrol = m_Instance.GetComponent<Patrol>();
    }

    public void DisableControl()
    {
        //dissable script patrol
        m_Patrol.enabled = false;
    }


    public void EnableControl()
    {
        //enable script patrol
        m_Patrol.enabled = true;
    }


    public void Reset()
    {
        // Reset the ghost position and destination to deafault
        m_Instance.transform.position = m_SpawnPoint.position;
        m_Instance.transform.rotation = m_SpawnPoint.rotation;

        m_Patrol.destPoint = 0;
        
        m_Instance.SetActive(false);
        m_Instance.SetActive(true);
    }

}
