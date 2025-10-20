using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Scripting.APIUpdating;
using System;
using UnityEngine.UIElements;

public class Boid : MonoBehaviour
{
    /* Array of all boids in the scene */
    List<GameObject> boids;


    /* How fast the boid moves forwards (m/s) */
    [SerializeField] float moveSpeed = 5f;

    /* How fast the boid turns (deg/s) */
    [SerializeField] float turnSpeed = 180f;

    /* Maximum distance to react to other boids */
    [SerializeField] float detectionRange = 50f;


    /* Modifier for cohesion factor */
    [SerializeField] float cohesionWeight = 1f;

    /* Modifier for separation factor */
    [SerializeField] float separationWeight = 1f;

    /* Modifier for alignment factor */
    [SerializeField] float alignmentWeight = 1f;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // get all boids
        boids = new List<GameObject>(GameObject.FindGameObjectsWithTag("Boid"));
    }


    // Update is called once per frame
    void Update()
    {
        // combine cohesion, alignment, and separation into one direction
        // modify each factor by its respective weight
        Vector3 newDir =
            (CohesionDirection * cohesionWeight) +
            (AlignmentDirection * alignmentWeight) +
            (SeparationDirection * separationWeight);

        newDir.Normalize();


        // steer towards the new final direction
        Vector3 finalDir = Vector3.RotateTowards(transform.forward, newDir, turnSpeed * Time.deltaTime * Mathf.Deg2Rad, 0f);
        transform.forward = finalDir;


        // move forwards
        Vector3 vel = transform.forward * moveSpeed;
        transform.position += vel * Time.deltaTime;
    }


    /* 
     * Unweighted direction for cohesion factor-
     * The direction from self to the average position of all boids
     */
    Vector3 CohesionDirection
    {
        get
        {
            // find average position of all boids
            Vector3 avgPos = Vector3.zero;
            int nboids = 0;
            // find position of each boid 
            foreach (GameObject b in boids)
            {
                // make sure boid is not self, and not too close
                float dist = (b.transform.position - transform.position).magnitude;
                if (b != this && dist <= detectionRange)
                {
                    avgPos += b.transform.position;
                    nboids++;
                }
            }
            if (nboids > 0) avgPos /= nboids;

            // find direction from self to average position
            Vector3 dir = avgPos - transform.position;
            return dir.normalized;
        }
    }


    /*
     * Unweighted direction for alignment factor-
     * The average facing direction of all boids
     */
    Vector3 AlignmentDirection
    {
        get
        {
            // find average facing direction
            Vector3 avgDir = Vector3.zero;
            int nboids = 0;
            // find facing vector for each boid
            foreach (GameObject b in boids)
            {
                // make sure boid is not self, and not too close
                float dist = (transform.position - b.transform.position).magnitude;
                if (b != this && dist <= detectionRange)
                {
                    avgDir += b.transform.forward;
                    nboids++;
                }
            }

            if (nboids > 0) avgDir /= nboids;
            return avgDir.normalized;
        }
    }


    /*
     * Unweighted direction for separation factor-
     * Direction pointing AWAY from nearby boids. Boids closer to self will
     * influence separation direction more.
     */
    Vector3 SeparationDirection
    {
        get
        {
            Vector3 avgDir = Vector3.zero;
            int nboids = 0;
            // find direction away from each boid
            foreach (GameObject b in boids)
            {
                // make sure boid is not self, and not too close
                float dist = (b.transform.position - transform.position).magnitude;
                if (b != this && dist <= detectionRange)
                {
                    // find unit vector from other boid to self 
                    Vector3 dir = (transform.position - b.transform.position).normalized;
                    // weight by distance. further boids will have more dist, so dividing by 
                    // dist will make smaller vector for further boids
                    dir /= dist;

                    avgDir += dir;
                    nboids++;
                }
            }

            if (nboids > 0) avgDir /= nboids;
            return avgDir.normalized;
        }
    }
}
