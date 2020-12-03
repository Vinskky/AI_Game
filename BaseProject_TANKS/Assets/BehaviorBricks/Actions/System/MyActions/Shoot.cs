using Pada1.BBCore.Tasks;
using Pada1.BBCore;
using UnityEngine;
using System;

namespace BBUnity.Actions
{
    /// <summary>
    /// It is an action to make a tank shoot a bullet.
    /// </summary>
    [Action("MyActions/Shoot")]
    [Help("Gets a launch force, finds an angle on the y axis by which instantiating the bullet with said force will reach the enemy tank, and shoots")]
    public class Shoot : GOAction
    {
        ///<value>Input Shell Prefab.</value>
        [InParam("Shell")]
        [Help("Rigidbody of the Shell Prefab")]
        public Rigidbody m_Shell;

        ///<value>Input Transform.</value>
        [InParam("Fire Transform")]
        [Help("Transform from which the shell is launched")]
        public Transform m_FireTransform;

        ///<value>Input Shooting Audio Source.</value>
        [InParam("Shooting Audio")]
        [Help("Audio Source that will reproduce the Shooting Audio Clip.")]
        public AudioSource m_ShootingAudio;

        ///<value>Input Fire Audio Clip.</value>
        [InParam("Fire Clip")]
        [Help("Audio Clip to be played at launch.")]
        public AudioClip m_FireClip;

        ///<value>Input Launch Force.</value>
        [InParam("Launch Force")]
        [Help("Force at which the shell will be launched.")]
        public float m_LaunchForce;

        ///<value>Input Enemy Tank.</value>
        [InParam("Enemy")]
        [Help("Enemy tank")]
        public GameObject enemy;

        ///<value>Input Agent Tank.</value>
        [InParam("Agent")]
        [Help("Agent tank")]
        public GameObject agent;

        private float shootAngle = 0;
        private float gravity = 9.81f;

        /// <summary>Initialization Method of MoveToRandomPosition.</summary>
        /// <remarks>Check if there is a NavMeshAgent to assign it one by default and assign it
        /// to the NavMeshAgent the destination a random position calculated with <see cref="getRandomPosition()"/> </remarks>
        public override void OnStart()
        {


        }
        /// <summary>Method of Update of MoveToRandomPosition </summary>
        /// <remarks>Check the status of the task, if it has traveled the road or is close to the goal it is completed
        /// and otherwise it will remain in operation.</remarks>
        public override TaskStatus OnUpdate()
        {
            // Track the current state of the fire button and make decisions based on the current launch force.

            RaycastHit ray;

            Vector3 origin = agent.transform.position;
            origin.y = 2;

            Vector3 final = enemy.transform.position;
            final.y = 2;

            Vector3 direction = new Vector3(0.0f, 0.0f, 0.0f);
            direction = final - origin;
            direction.Normalize();

            Physics.Raycast(origin, direction, out ray, Mathf.Abs(Vector3.Distance(origin, final)));
            Debug.DrawLine(origin, final, Color.red);


            Fire();

            return TaskStatus.COMPLETED;
        }

        private void Fire()
        {
            GameObject enemy = GameObject.Find("Enemy");
            GameObject agent = GameObject.Find("Agent");

            // Instantiate and launch the shell.

            shootAngle = 0.5f * (Mathf.Rad2Deg * Mathf.Asin((gravity * Mathf.Abs(Vector3.Distance(enemy.transform.position, agent.transform.position))) / (m_LaunchForce * m_LaunchForce)));
            m_FireTransform.localEulerAngles = new Vector3(360 - shootAngle, m_FireTransform.localEulerAngles.y, m_FireTransform.localEulerAngles.z);

            Rigidbody shellInstance = GameObject.Instantiate(m_Shell, m_FireTransform.transform.position, m_FireTransform.transform.rotation) as Rigidbody;

            shellInstance.velocity = m_LaunchForce * m_FireTransform.transform.forward;

            m_ShootingAudio.clip = m_FireClip;
            m_ShootingAudio.Play();

        }
    }
}
