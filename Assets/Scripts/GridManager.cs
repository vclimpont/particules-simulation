using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {

    }

    // return true if the rectangle and circle are colliding
    bool RectCircleColliding(Vector3 cPos, float cR, Vector3 sPos, float sSize)
    {
        float distX = Mathf.Abs(cPos.x - sPos.x - sSize / 2);
        float distY = Mathf.Abs(cPos.y - sPos.y - sSize / 2);

        if (distX > (sSize / 2 + cR)) 
        { 
            return false; 
        }
        if (distY > (sSize / 2 + cR)) 
        { 
            return false; 
        }

        if (distX <= (sSize / 2)) 
        { 
            return true; 
        }
        if (distY <= (sSize / 2)) 
        { 
            return true; 
        }

        float dx = distX - sSize / 2;
        float dy = distY - sSize / 2;
        return (dx * dx + dy * dy <= (cR * cR));
    }
}
