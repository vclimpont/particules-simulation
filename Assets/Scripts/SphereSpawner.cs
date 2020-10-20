using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class SphereSpawner : MonoBehaviour
{
    [Header("Spheres settings")]
    [SerializeField] GameObject spherePrefab;
    [SerializeField] int nbSpheres;
    [SerializeField] Vector3 maxInitVelocity;
    [SerializeField] float massMin;
    [SerializeField] float massMax;
    [SerializeField] float foutside;
    [SerializeField] float h;
    [SerializeField] float k;
    [SerializeField] float k_Near;
    [SerializeField] float p0;
    [SerializeField] float alpha;
    [SerializeField] float beta;
    [SerializeField] float dTime;

    [Header("Boundaries")]
    [SerializeField] float[] sizeX;
    [SerializeField] float[] sizeY;


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
            float x = Random.Range(sizeX[0], sizeX[1]);
            float y = Random.Range(sizeY[1], sizeY[1]);

            float vx = Random.Range(-maxInitVelocity.x, maxInitVelocity.x);
            float vy = Random.Range(0, maxInitVelocity.y);

            float m = Random.Range(massMin, massMax);
            Color c = Color.Lerp(Color.blue, Color.red, m / (massMin + massMax));

            GameObject sphereGO = Instantiate(spherePrefab, new Vector3(x, y, 0), Quaternion.identity);
            sphereGO.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
            sphereGO.transform.parent = transform;
            sphereGO.GetComponent<MeshRenderer>().material.color = c;

            FluidBehaviour particle = sphereGO.GetComponent<FluidBehaviour>();
            particle.Initialize(new Vector3(vx, vy, 0), m, foutside, h, k, k_Near, p0, alpha, beta, dTime, sizeX, sizeY[0]);
            particles.Add(particle);
        }
    }

    void ActivateFluidSimulator()
    {
        fs.Initialize(dTime, particles);
        fs.StartSimulating();
    }
}
