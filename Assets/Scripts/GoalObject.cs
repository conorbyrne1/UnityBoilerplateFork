using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalObject : MonoBehaviour
{
    // Start is called before the first frame update

    public int x;
    public int y;
    public GridManager.spaceType spaceType;
    GridManager gridManager;
    public Transform cubePivot;
    public Renderer cubeMesh;

    public Material emptyMaterial;
    public Material goalMaterial;

    //Dictionary<GridManager.spaceType, Material> spaceTypeMaterials;

    void Awake()
    {
        
    }

    public void GridInit()
    {
        gridManager = GetComponentInParent<GridManager>();
        transform.position = gridManager.GetWorldPosition(x, y);
        
    }

    public void UpdateSpaceTypeDisplay()
    {

        //cubePivot.localScale = new Vector3(0.9f, 1f, 0.9f);
        //cubeMesh.material = spaceTypeMaterials[spaceType];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseOver()
    {
       
    }
}
