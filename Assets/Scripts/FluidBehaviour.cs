﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FluidBehaviour : MonoBehaviour
{
    public Vector3 Velocity { get; set; }
    public float Mass { get; private set; }

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

    public void DoubleDensityRelaxation()
    {
        float p = 0;
        float pNear = 0;

        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, h, LayerMask.GetMask("Sphere"));

        foreach (Collider2D coll in hitColliders)
        {
            float q = Vector3.Distance(transform.position, coll.transform.position) / h;
            if (q < 1)
            {
                p += Mathf.Pow((1 - q), 2);
                pNear += Mathf.Pow((1 - q), 3);
            }
        }

        float P = k * (p - p0);
        float PNear = kNear * pNear;
        Vector3 dx = Vector3.zero;

        foreach (Collider2D coll in hitColliders)
        {
            float q = Vector3.Distance(transform.position, coll.transform.position) / h;
            if (q < 1)
            {
                Vector3 rij = (coll.transform.position - transform.position).normalized;
                Vector3 D = (dTime * dTime) / Mass * (P * (1 - q) + PNear * Mathf.Pow((1 - q), 2)) * rij;
                coll.transform.position += (D / 2);
                dx -= (D / 2);
            }
        }

        transform.position += dx;
    }

    public void ApplyViscosity()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, h, LayerMask.GetMask("Sphere"));

        foreach (Collider2D coll in hitColliders)
        {
            float q = Vector3.Distance(transform.position, coll.transform.position) / h;
            if (q < 1)
            {
                FluidBehaviour pj = coll.GetComponent<FluidBehaviour>();
                Vector3 rij = (coll.transform.position - transform.position).normalized;
                float u = Vector3.Dot((Velocity - pj.Velocity), rij);

                if (u > 0)
                {
                    Vector3 I = dTime * (1 - q) * ((alpha * u) + (beta * Mathf.Pow(u, 2))) * rij;
                    Velocity -= (I / 2);
                    pj.Velocity += (I / 2);
                    //Debug.Log(Velocity + " " + pj.Velocity);
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
        Vector3 dir = Vector3.zero;

        if(transform.position.y <= boundY)
        {
            dir = (new Vector3(transform.position.x, boundY, transform.position.z) - transform.position).normalized;
            transform.position += (dtm * (foutside * Vector3.Distance(new Vector3(transform.position.x, boundY, transform.position.z), transform.position)) * dir);
        }

        if (transform.position.x <= boundX[0])
        {
            dir = (new Vector3(boundX[0], transform.position.y, transform.position.z) - transform.position).normalized;
            transform.position += (dtm * (foutside * Vector3.Distance(new Vector3(boundX[0], transform.position.y, transform.position.z), transform.position)) * dir);
        }

        if (transform.position.x >= boundX[1])
        {
            dir = (new Vector3(boundX[1], transform.position.y, transform.position.z) - transform.position).normalized;
            transform.position += (dtm * (foutside * Vector3.Distance(new Vector3(boundX[1], transform.position.y, transform.position.z), transform.position)) * dir);
        }
    }
}