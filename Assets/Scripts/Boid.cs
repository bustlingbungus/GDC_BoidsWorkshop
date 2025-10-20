using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Scripting.APIUpdating;
using System;
using UnityEngine.UIElements;

public class Boid : MonoBehaviour
{
    /* How quickly the boid moves forwards (m/s) */
    [HideInInspector] public float movementSpeed = 5f;

    /* How quickly the boid turns (rad/s) */
    [HideInInspector] public float turnSpeed = Mathf.PI;


    /* Target movement direction for cohesion */
    Vector3 cohesionDir = Vector3.zero;

    /* Target movement direction for separation */
    Vector3 separationDir = Vector3.zero;

    /* Target movement direction for alignment */
    Vector3 alignmentDir = Vector3.zero;

    /* number of boids in detection range */
    int nboids = 0;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }


    // Update is called once per frame
    void Update()
    {
        // steer towards new direction
        if (nboids > 0)
        {
            // combine calculated directions
            Vector3 newDir = cohesionDir + separationDir + alignmentDir;
            newDir.Normalize();

            // steer towards direction
            transform.forward = Vector3.RotateTowards(transform.forward, newDir, turnSpeed * Time.deltaTime, 0f);
        }

        // move forwards
        transform.position += transform.forward * movementSpeed * Time.deltaTime;
    }


    /* Resets direction vectors to (0, 0, 0). Resets nearby boid cound to 0. */
    public void Reset()
    {
        cohesionDir = Vector3.zero;
        separationDir = Vector3.zero;
        alignmentDir = Vector3.zero;
        nboids = 0;
    }


    /* Complete all calculations and apply all component weights */
    public void FinalizeCalculations(float cohesionWeight, float separationWeight, float alignmentWeight)
    {
        if (nboids > 0)
        {
            // cohesion
            cohesionDir /= nboids;
            cohesionDir = (cohesionDir - transform.position).normalized;
            cohesionDir *= cohesionWeight;

            // alignment
            alignmentDir.Normalize();
            alignmentDir *= alignmentWeight;

            // separation
            separationDir.Normalize();
            separationDir *= separationWeight;
        }
    }


    /* Updates cohesion, alignment, and separation variables for a given boid */
    public void ReactToBoid(GameObject other, float dist)
    {
        ++nboids;

        // cohesion
        cohesionDir += other.transform.position;
        // alignment
        alignmentDir += other.transform.forward;

        // separation
        Vector3 dir = transform.position - other.transform.position;
        dir /= dist * dist;
        separationDir += dir;
    }
}
