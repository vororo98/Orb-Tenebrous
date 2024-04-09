using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Vector2 pos;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setPos(Vector2 newPos)
    {
        pos = newPos;
    }

    public Vector2 getPos()
    {
        return pos;
    }
}
