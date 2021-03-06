﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class SphereSpawner : MonoBehaviour
{
    [Header("Spheres settings")]
    [SerializeField] GameObject spherePrefab = null;
    [SerializeField] int nbSpheres = 0;
    [SerializeField] Vector3 maxInitVelocity = Vector3.zero;
    [SerializeField] float massMin = 0;
    [SerializeField] float massMax = 0;
    [SerializeField] float fOutside = 0;
    [SerializeField] float h = 0;
    [SerializeField] float k = 0;
    [SerializeField] float k_Near = 0;
    [SerializeField] float p0 = 0;
    [SerializeField] float alpha = 0;
    [SerializeField] float beta = 0;
    [SerializeField] float dTime = 0;

    [Header("Boundaries")]
    [SerializeField] float[] sizeX = null;
    [SerializeField] float[] sizeY = null;


    private List<FluidBehaviour> particles;
    private FluidSimulator fs;

    // Start is called before the first frame update
    void Start()
    {
        particles = new List<FluidBehaviour>();
        fs = GetComponent<FluidSimulator>();
        CreateSpheres();
        ActivateFluidSimulator();
    }

    void CreateSpheres()
    {
        Assert.IsTrue(sizeX.Length == 2 && sizeY.Length == 2);

        for (int i = 0; i < nbSpheres; i++)
        {
            float x = Random.Range(-7, -5);
            float y = Random.Range(sizeY[1], sizeY[1]);

            float vx = Random.Range(0, maxInitVelocity.x);
            float vy = Random.Range(0, maxInitVelocity.y);

            float m = Random.Range(massMin, massMax);
            Color c = Color.Lerp(Color.blue, Color.red, m / (massMin + massMax));

            GameObject sphereGO = Instantiate(spherePrefab, new Vector3(x, y, 0), Quaternion.identity);
            //sphereGO.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
            sphereGO.transform.parent = transform;
            //sphereGO.GetComponent<MeshRenderer>().material.color = c;
            sphereGO.GetComponent<SpriteRenderer>().color = c;

            FluidBehaviour particle = sphereGO.GetComponent<FluidBehaviour>();
            particle.Initialize(new Vector3(vx, vy, 0), m, fOutside, h, k, k_Near, p0, alpha, beta, dTime, sizeX, sizeY[0]);
            particles.Add(particle);
        }
    }

    void ActivateFluidSimulator()
    {
        fs.Initialize(dTime, particles);
        fs.StartSimulating();
    }
}
