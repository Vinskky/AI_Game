using System;
using UnityEngine;

[Serializable]
public class TankManager
{
    public Color m_PlayerColor;            
    public Transform m_SpawnPoint;
    public BBUnity.InternalBrickAsset m_TankBehaviour;
    public Rigidbody m_Shell;
    public AudioClip m_FireClip;
    [HideInInspector] public int m_PlayerNumber;             
    [HideInInspector] public string m_ColoredPlayerText;
    [HideInInspector] public GameObject m_Instance;          
    [HideInInspector] public int m_Wins;                     
    private BehaviorExecutor m_BExecutor;
    private GameObject m_CanvasGameObject;
    private Transform m_FireTransform;


    public void Setup()
    {
        m_BExecutor = m_Instance.GetComponent<BehaviorExecutor>();
        m_CanvasGameObject = m_Instance.GetComponentInChildren<Canvas>().gameObject;
        m_BExecutor.behavior = m_TankBehaviour;

        m_BExecutor.SetBehaviorParam("Shell", m_Shell);

        GameObject[] gos = GameObject.FindObjectsOfType(typeof(GameObject)) as GameObject[]; //will return an array of all GameObjects in the scene
        foreach (GameObject go in gos)
        {
            //we have to avoid FireTransform because it's inside layer player
            if (go.layer == 9 && go.name == "FireTransform" && go.transform.parent.parent.parent.gameObject == m_Instance)
            {
                m_FireTransform = go.transform;
            }
        }
        m_BExecutor.SetBehaviorParam("Fire Transform", m_FireTransform);
    
        m_BExecutor.SetBehaviorParam("Shooting Audio", m_Instance.GetComponents<AudioSource>()[1]);
        m_BExecutor.SetBehaviorParam("Fire Clip", m_FireClip);
        m_Instance.tag = "Player " + m_PlayerNumber.ToString();

        m_ColoredPlayerText = "<color=#" + ColorUtility.ToHtmlStringRGB(m_PlayerColor) + ">PLAYER " + m_PlayerNumber + "</color>";

        MeshRenderer[] renderers = m_Instance.GetComponentsInChildren<MeshRenderer>();

        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material.color = m_PlayerColor;
        }
    }


    public void DisableControl()
    {
        m_BExecutor.enabled = false;

        m_CanvasGameObject.SetActive(false);
    }


    public void EnableControl()
    {
        m_BExecutor.enabled = true;

        m_CanvasGameObject.SetActive(true);
    }


    public void Reset()
    {
        m_Instance.transform.position = m_SpawnPoint.position;
        m_Instance.transform.rotation = m_SpawnPoint.rotation;

        m_Instance.SetActive(false);
        m_Instance.SetActive(true);
    }
}
