using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretRot : MonoBehaviour
{
    private GameObject target;
    public float rotationSpeed = 1.5f;

    private Quaternion lookRotation;
    private Vector3 direction;
    // Start is called before the first frame update
    void Start()
    {
        GameObject[] gos = GameObject.FindObjectsOfType(typeof(GameObject)) as GameObject[]; //will return an array of all GameObjects in the scene
        foreach (GameObject go in gos)
        {
            //we have to avoid FireTransform because it's inside layer player
            if (go.layer == 9 && go != transform.parent.parent.gameObject && go.name != "FireTransform")
            {
                target = go.gameObject;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Rotate turret to point at tank target
        direction = (target.transform.position - transform.position).normalized;

        lookRotation = Quaternion.LookRotation(direction);

        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    }
}
