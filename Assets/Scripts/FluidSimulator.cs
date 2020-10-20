using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FluidSimulator : MonoBehaviour
{
    private float dTime;
    private List<FluidBehaviour> particles;

    public void Initialize(float dTime, List<FluidBehaviour> particles)
    {
        this.dTime = dTime;
        this.particles = particles;
    }

    // Start is called before the first frame update
    public void StartSimulating()
    {
        InvokeRepeating("UpdatePhysics", 0, dTime);
    }

    void UpdatePhysics()
    {
        foreach(FluidBehaviour particle in particles)
        {
            particle.ApplyGravity();
        }

        foreach (FluidBehaviour particle in particles)
        {
            particle.ApplyViscosity();
        }

        foreach (FluidBehaviour particle in particles)
        {
            particle.UpdatePosition();
        }

        foreach(FluidBehaviour particle in particles)
        {
            particle.DoubleDensityRelaxation();
        }

        foreach (FluidBehaviour particle in particles)
        {
            particle.ClampVelocity();
        }

        foreach (FluidBehaviour particle in particles)
        {
            particle.ComputeNextVelocity();
        }
    }
}
