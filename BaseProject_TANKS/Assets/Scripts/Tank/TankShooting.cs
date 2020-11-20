using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;


public class TankShooting : MonoBehaviour
{
    public int m_PlayerNumber = 1;       
    public Rigidbody m_Shell;            
    public Transform m_FireTransform;             
    public AudioSource m_ShootingAudio;   
    public AudioClip m_FireClip;
    public float m_LaunchForce = 10f;
    public float m_rechargeTime = 1;

    private GameObject target;
    private float rechargeTimer = 5;
    private float shootAngle = 0;
    private float gravity = 9.81f;


    private void OnEnable()
    {
    }


    private void Start()
    {
        GameObject[] gos = GameObject.FindObjectsOfType(typeof(GameObject)) as GameObject[]; //will return an array of all GameObjects in the scene
        foreach (GameObject go in gos)
        {
            if (go.layer == 9 && go != gameObject && go.name != "FireTransform")
            {
                target = go.gameObject;
            }
        }
    }
    

    private void Update()
    {
        // Track the current state of the fire button and make decisions based on the current launch force.

        RaycastHit ray;

        Vector3 origin = transform.position;
        origin.y = 2;

        Vector3 final = target.transform.position;
        final.y = 2;

        Vector3 direction = new Vector3(0.0f, 0.0f, 0.0f);
        direction = final - origin;
        direction.Normalize();

        Physics.Raycast(origin, direction, out ray, Mathf.Abs(Vector3.Distance(origin, final)));
        Debug.DrawLine(origin, final, Color.red);

        if (rechargeTimer > m_rechargeTime && ray.collider == null)
        {

            if (Mathf.Abs(Vector3.Distance(target.transform.position, transform.position)) < ((m_LaunchForce * m_LaunchForce) / gravity))
            {
                Fire();
            }

            rechargeTimer = 0;
        }

        rechargeTimer += Time.deltaTime;
    }


    private void Fire()
    {
        // Instantiate and launch the shell.

        shootAngle = 0.5f * (Mathf.Rad2Deg*Mathf.Asin((gravity * Mathf.Abs(Vector3.Distance(target.transform.position, transform.position))) / (m_LaunchForce * m_LaunchForce)));
        m_FireTransform.localEulerAngles = new Vector3(360 - shootAngle, m_FireTransform.localEulerAngles.y, m_FireTransform.localEulerAngles.z);

        Rigidbody shellInstance = Instantiate(m_Shell, m_FireTransform.transform.position, m_FireTransform.transform.rotation) as Rigidbody;

        shellInstance.velocity = m_LaunchForce * m_FireTransform.transform.forward;

        m_ShootingAudio.clip = m_FireClip;
        m_ShootingAudio.Play();

    }
}