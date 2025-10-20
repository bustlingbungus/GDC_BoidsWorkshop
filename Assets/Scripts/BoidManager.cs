using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

public class BoidManager : MonoBehaviour
{
    List<Boid> boids;

    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float turnSpeed = 180f;
    float turnSpeedRad;
    [SerializeField] float detectionRange = 50f;
    [SerializeField] float cohesionWeight = 1f;
    [SerializeField] float separationWeight = 1f;
    [SerializeField] float alignmentWeight = 1f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // get all boid objects, and get their boid component, save to array
        GameObject[] all_boids = GameObject.FindGameObjectsWithTag("Boid");
        boids = new List<Boid>();
        for (int i = 0; i < all_boids.Length; ++i)
        {
            Boid b = all_boids[i].GetComponent<Boid>();
            boids.Add(b);
        }
    }

    // Update is called once per frame
    void Update()
    {
        turnSpeedRad = turnSpeed * Mathf.Deg2Rad;

        foreach (Boid b1 in boids)
        {
            // reset each boid to prepare for new calculations
            b1.Reset();

            // react to every other boid in the scene
            foreach (Boid b2 in boids)
            {
                // dont react if current boid is self, or if it is too far
                if (b2 == b1) continue;
                float dist = (b1.transform.position - b2.transform.position).magnitude;
                if (dist <= detectionRange) b1.ReactToBoid(b2.gameObject, dist);
            }

            // complete calculations after iteration, using weights
            b1.FinalizeCalculations(cohesionWeight, separationWeight, alignmentWeight);

            // apply settings continuously
            b1.movementSpeed = moveSpeed;
            b1.turnSpeed = turnSpeedRad;
        }
    }
    

}
