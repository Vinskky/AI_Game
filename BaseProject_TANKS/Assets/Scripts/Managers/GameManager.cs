using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public int m_NumRoundsToWin = 5;        
    public float m_StartDelay = 3f;         
    public float m_EndDelay = 3f;           
    public CameraControl m_CameraControl;   
    public Text m_MessageText;              
    public GameObject m_TankPrefab;         
    public TankManager[] m_Tanks;

    public GameObject m_GhostPrefab;
    public GhostManager m_Ghosts;

    public GameObject m_AmmoPrefab;
    public int m_AmountAmmo = 4;
    private Transform[] m_AmmoSpawnPoints;

    private int m_RoundNumber;              
    private WaitForSeconds m_StartWait;     
    private WaitForSeconds m_EndWait;       
    private TankManager m_RoundWinner;
    private TankManager m_GameWinner;       


    private void Start()
    {
        GameObject[] gos = GameObject.FindGameObjectsWithTag("Recharge").OrderBy(go => go.name).ToArray();

        m_AmmoSpawnPoints = new Transform[gos.Length];

        for (int i = 0; i < gos.Length; i++)
        {
            m_AmmoSpawnPoints[i] = gos[i].transform;
        }

        m_StartWait = new WaitForSeconds(m_StartDelay);
        m_EndWait = new WaitForSeconds(m_EndDelay);

        SpawnAllTanks();
        SpawnAllAmmo();
        SetCameraTargets();

        StartCoroutine(GameLoop());
    }


    private void SpawnAllTanks()
    {
        for (int i = 0; i < m_Tanks.Length; i++)
        {
            m_Tanks[i].m_Instance =
                Instantiate(m_TankPrefab, m_Tanks[i].m_SpawnPoint.position, m_Tanks[i].m_SpawnPoint.rotation) as GameObject;
            m_Tanks[i].m_PlayerNumber = i + 1;
            m_Tanks[i].Setup();
        }

        m_Ghosts.m_Instance = Instantiate(m_GhostPrefab, m_Ghosts.m_SpawnPoint.position, m_Ghosts.m_SpawnPoint.rotation) as GameObject;
        m_Ghosts.Setup();
    }

    private void SpawnAllAmmo()
    {
        int pos1 = Random.Range(0, 7);
        int pos2 = Random.Range(0, 7);
        int pos3 = Random.Range(0, 7);
        int pos4 = Random.Range(0, 7);

        while (pos1 == pos2 || pos1 == pos3 || pos1 == pos4)
            pos1 = Random.Range(0, 7);

        while (pos2 == pos1 || pos2 == pos3 || pos2 == pos4)
            pos2 = Random.Range(0, 7);

        while (pos3 == pos1 || pos3 == pos2 || pos3 == pos4)
            pos3 = Random.Range(0, 7);

        while (pos4 == pos1 || pos4 == pos2 || pos4 == pos3)
            pos4 = Random.Range(0, 7);

        Instantiate(m_AmmoPrefab, m_AmmoSpawnPoints[pos1].position, m_AmmoSpawnPoints[pos1].rotation);
        Instantiate(m_AmmoPrefab, m_AmmoSpawnPoints[pos2].position, m_AmmoSpawnPoints[pos2].rotation);
        Instantiate(m_AmmoPrefab, m_AmmoSpawnPoints[pos3].position, m_AmmoSpawnPoints[pos3].rotation);
        Instantiate(m_AmmoPrefab, m_AmmoSpawnPoints[pos4].position, m_AmmoSpawnPoints[pos4].rotation);
    }


    private void SetCameraTargets()
    {
        Transform[] targets = new Transform[m_Tanks.Length];

        for (int i = 0; i < targets.Length; i++)
        {
            targets[i] = m_Tanks[i].m_Instance.transform;
        }

        m_CameraControl.m_Targets = targets;
    }


    private IEnumerator GameLoop()
    {
        yield return StartCoroutine(RoundStarting());
        yield return StartCoroutine(RoundPlaying());
        yield return StartCoroutine(RoundEnding());

        if (m_GameWinner != null)
        {
            Application.LoadLevel(Application.loadedLevel);
        }
        else
        {
            StartCoroutine(GameLoop());
        }
    }


    private IEnumerator RoundStarting()
    {
        ResetAllTanks();
        DisableTankControl();

        m_CameraControl.SetStartPositionAndSize();

        m_RoundNumber++;
        m_MessageText.text = "Round" + m_RoundNumber;
        yield return m_StartWait;
    }


    private IEnumerator RoundPlaying()
    {
        EnableTankControl();
        m_MessageText.text = string.Empty;

        while(!OneTankLeft())
        {
            yield return null;
        }
        
    }


    private IEnumerator RoundEnding()
    {
        DisableTankControl();

        m_RoundWinner = null;

        m_RoundWinner = GetRoundWinner();

        if (m_RoundWinner != null)
            m_RoundWinner.m_Wins++;

        m_GameWinner = GetGameWinner();

        string message = EndMessage();
        m_MessageText.text = message;

        yield return m_EndWait;
    }


    private bool OneTankLeft()
    {
        int numTanksLeft = 0;

        for (int i = 0; i < m_Tanks.Length; i++)
        {
            if (m_Tanks[i].m_Instance.activeSelf)
                numTanksLeft++;
        }

        return numTanksLeft <= 1;
    }


    private TankManager GetRoundWinner()
    {
        for (int i = 0; i < m_Tanks.Length; i++)
        {
            if (m_Tanks[i].m_Instance.activeSelf)
                return m_Tanks[i];
        }

        return null;
    }


    private TankManager GetGameWinner()
    {
        for (int i = 0; i < m_Tanks.Length; i++)
        {
            if (m_Tanks[i].m_Wins == m_NumRoundsToWin)
                return m_Tanks[i];
        }

        return null;
    }


    private string EndMessage()
    {
        string message = "DRAW!";

        if (m_RoundWinner != null)
            message = m_RoundWinner.m_ColoredPlayerText + " WINS THE ROUND!";

        message += "\n\n\n\n";

        for (int i = 0; i < m_Tanks.Length; i++)
        {
            message += m_Tanks[i].m_ColoredPlayerText + ": " + m_Tanks[i].m_Wins + " WINS\n";
        }

        if (m_GameWinner != null)
            message = m_GameWinner.m_ColoredPlayerText + " WINS THE GAME!";

        return message;
    }


    private void ResetAllTanks()
    {
        for (int i = 0; i < m_Tanks.Length; i++)
        {
            m_Tanks[i].Reset();
        }
        m_Ghosts.Reset();
    }


    private void EnableTankControl()
    {
        for (int i = 0; i < m_Tanks.Length; i++)
        {
            m_Tanks[i].EnableControl();
        }
        m_Ghosts.EnableControl();
    }


    private void DisableTankControl()
    {
        for (int i = 0; i < m_Tanks.Length; i++)
        {
            m_Tanks[i].DisableControl();
        }
        m_Ghosts.DisableControl();
    }
}