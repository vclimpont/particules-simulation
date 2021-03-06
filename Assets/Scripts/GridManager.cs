﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager
{
    internal Dictionary<Vector3, List<GameObject>> DicGridParticles { get; private set; }

    public GridManager()
    {
        DicGridParticles = new Dictionary<Vector3, List<GameObject>>();   
    }

    public List<GameObject> GetParticlesInGrid(Vector3 squarePos)
    {
        DicGridParticles.TryGetValue(squarePos, out List<GameObject> particles);
        return particles;
    }

    public List<GameObject> GetNeighboursOfParticle(Vector3 squarePos, float h)
    {
        List<GameObject> neighbours = new List<GameObject>();
        for(float i = -h; i <= h; i += h)
        {
            for (float j = -h; j <= h; j += h)
            {
                if(DicGridParticles.TryGetValue(new Vector3(squarePos.x + i, squarePos.y + j, 0), out List<GameObject> particles))
                {
                    neighbours.AddRange(particles);
                }
            }
        }

        return neighbours;
    }

    public void AddToDictionary(List<FluidBehaviour> particles)
    {
        foreach (FluidBehaviour particle in particles)
        {
            Vector3 pos = particle.transform.position;
            Vector3 squarePos = new Vector3(RoundTo(pos.x, particle.GetH()), RoundTo(pos.y, particle.GetH()), 0);

            if (DicGridParticles.TryGetValue(squarePos, out List<GameObject> partAtKey))
            {
                partAtKey.Add(particle.gameObject);
                DicGridParticles[squarePos] = partAtKey;
            }
            else
            {
                partAtKey = new List<GameObject>();
                partAtKey.Add(particle.gameObject);
                DicGridParticles.Add(squarePos, partAtKey);
            }
        }
    }

    public void ClearDictionary()
    {
        DicGridParticles = new Dictionary<Vector3, List<GameObject>>();
    }

    public float RoundTo(float value, float multipleOf)
    {
        return Mathf.Round(value / multipleOf) * multipleOf;
    }

    // return true if the rectangle and circle are colliding
    //bool RectCircleColliding(Vector3 cPos, float cR, Vector3 sPos, float sSize)
    //{
    //    float distX = Mathf.Abs(cPos.x - sPos.x - sSize / 2);
    //    float distY = Mathf.Abs(cPos.y - sPos.y - sSize / 2);

    //    if (distX > (sSize / 2 + cR)) 
    //    { 
    //        return false; 
    //    }
    //    if (distY > (sSize / 2 + cR)) 
    //    { 
    //        return false; 
    //    }

    //    if (distX <= (sSize / 2)) 
    //    { 
    //        return true; 
    //    }
    //    if (distY <= (sSize / 2)) 
    //    { 
    //        return true; 
    //    }

    //    float dx = distX - sSize / 2;
    //    float dy = distY - sSize / 2;
    //    return (dx * dx + dy * dy <= (cR * cR));
    //}
}
