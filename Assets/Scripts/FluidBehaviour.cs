﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FluidBehaviour : MonoBehaviour
{
    internal Vector3 Velocity { get; set; }
    internal float Mass { get; private set; }

    private Vector3 G = new Vector3(0, -9.81f, 0);
    private Vector3 prevPosition;
    private float foutside;
    private float h;
    private float k;
    private float kNear;
    private float p0;
    private float alpha;
    private float beta;
    private float dTime;
    private float[] boundX;
    private float boundY;

    public void Initialize(Vector3 initVelocity, float mass, float foutside, float h, float k, float kNear, float p0, float alpha, float beta, float dTime, float[] boundX, float boundY)
    {
        Velocity = initVelocity;
        Mass = mass;
        this.foutside = foutside;
        this.h = h;
        this.k = k;
        this.kNear = kNear;
        this.p0 = p0;
        this.alpha = alpha;
        this.beta = beta;
        this.dTime = dTime;
        this.boundX = boundX;
        this.boundY = boundY;
    }

    public void DoubleDensityRelaxation(List<GameObject> neighbours)
    {
        float p = 0;
        float pNear = 0;

        Vector3 crtPos = transform.position;

        foreach (GameObject partGO in neighbours)
        {
            float q = Vector3.Distance(crtPos, partGO.transform.position) / h;
            if (q < 1)
            {
                p += ((1 - q) * (1 - q));
                pNear += ((1 - q) * (1 - q) * (1 - q));
            }
        }

        float P = k * (p - p0);
        float PNear = kNear * pNear;
        Vector3 dx = Vector3.zero;

        foreach (GameObject partGO in neighbours)
        {
            Vector3 collPos = partGO.transform.position;
            float q = Vector3.Distance(crtPos, collPos) / h;
            if (q < 1)
            {
                Vector3 rij = Vector3.Normalize(collPos - crtPos);
                Vector3 D = (dTime * dTime) / Mass * (P * (1 - q) + PNear * (1 - q) * (1 - q)) * rij;
                collPos += (D / 2);
                dx -= (D / 2);

                partGO.transform.position = collPos;
            }
        }

        transform.position += dx;
    }

    public void ApplyViscosity(List<GameObject> neighbours)
    {
        Vector3 crtPos = transform.position;

        foreach (GameObject partGO in neighbours)
        {
            if (partGO == this)
            {
                continue;
            }

            Vector3 neighbourPos = partGO.transform.position;
            float q = Vector3.Distance(crtPos, neighbourPos) / h;
            if (q < 1)
            {
                FluidBehaviour pj = partGO.GetComponent<FluidBehaviour>();
                Vector3 rij = Vector3.Normalize(neighbourPos - crtPos);
                float u = Vector3.Dot((Velocity - pj.Velocity), rij);

                if (u > 0)
                {
                    Vector3 I = dTime * (1 - q) * ((alpha * u) + (beta * u * u)) * rij;
                    Velocity -= (I / 2);
                    pj.Velocity += (I / 2);
                }
            }
        }
    }

    public void ApplyGravity()
    {
        Velocity += (G * dTime);
    }

    public void UpdatePosition()
    {
        prevPosition = transform.position;
        transform.position += (Velocity * dTime);
    }

    public void ComputeNextVelocity()
    {
        Velocity = (transform.position - prevPosition) / dTime;
    }

    public void ClampVelocity()
    {
        float dtm = (dTime * dTime) / Mass;
        Vector3 dir;
        Vector3 crtPos = transform.position;

        if(crtPos.y <= boundY)
        {
            dir = Vector3.Normalize(new Vector3(crtPos.x, boundY, crtPos.z) - crtPos);
            crtPos += (dtm * (foutside * Vector3.Distance(new Vector3(crtPos.x, boundY, crtPos.z), crtPos)) * dir);
        }

        if (crtPos.x <= boundX[0])
        {
            dir = Vector3.Normalize(new Vector3(boundX[0], crtPos.y, crtPos.z) - crtPos);
            crtPos += (dtm * (foutside * Vector3.Distance(new Vector3(boundX[0], crtPos.y, crtPos.z), crtPos)) * dir);
        }

        if (crtPos.x >= boundX[1])
        {
            dir = Vector3.Normalize(new Vector3(boundX[1], crtPos.y, crtPos.z) - crtPos);
            crtPos += (dtm * (foutside * Vector3.Distance(new Vector3(boundX[1], crtPos.y, crtPos.z), crtPos)) * dir);
        }

        transform.position = crtPos;
    }
    
    public float GetH()
    {
        return h;
    }
}
