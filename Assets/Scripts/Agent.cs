using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    // Start is called before the first frame update

    GridManager gridManager;
    public int x;
    public int y;
    void Start()
    {
        
    }
    public void GridInit()
    {
        gridManager = GetComponentInParent<GridManager>();
        transform.position = gridManager.GetWorldPosition(x, y);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
