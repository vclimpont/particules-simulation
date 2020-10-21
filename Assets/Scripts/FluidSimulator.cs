using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FluidSimulator : MonoBehaviour
{
    private float dTime;
    private List<FluidBehaviour> particles;
    private GridManager gm;

    public void Initialize(float dTime, List<FluidBehaviour> particles)
    {
        this.dTime = dTime;
        this.particles = particles;
        gm = new GridManager();
    }

    // Start is called before the first frame update
    public void StartSimulating()
    {
        InvokeRepeating("UpdatePhysics", 0, dTime);
    }

    void Update()
    {

    }

    void UpdatePhysics()
    {
        gm.AddToDictionary(particles);

        foreach(FluidBehaviour particle in particles)
        {
            particle.ApplyGravity();
        }

        //foreach (FluidBehaviour particle in particles)
        //{
        //    Vector3 partPos = particle.transform.position;
        //    Vector3 squarePos = new Vector3(Mathf.Floor(partPos.x), Mathf.Floor(partPos.y), 0);
        //    particle.ApplyViscosity(gm.GetNeighboursOfParticle(squarePos));
        //}

        foreach (FluidBehaviour particle in particles)
        {
            particle.UpdatePosition();
        }

        foreach (FluidBehaviour particle in particles)
        {
            Vector3 partPos = particle.transform.position;
            Vector3 squarePos = new Vector3(Mathf.Floor(partPos.x), Mathf.Floor(partPos.y), 0);
            particle.DoubleDensityRelaxation(gm.GetNeighboursOfParticle(squarePos));
        }

        foreach (FluidBehaviour particle in particles)
        {
            particle.ClampVelocity();
        }

        foreach (FluidBehaviour particle in particles)
        {
            particle.ComputeNextVelocity();
        }

        gm.ClearDictionary();
    }
}
