using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject : MonoBehaviour
{
    // Start is called before the first frame update

    public int x;
    public int y;
    public GridManager.spaceType spaceType;
    GridManager gridManager;
    public Transform cubePivot;
    public Renderer cubeMesh;

    public Material emptyMaterial;
    //public Material goalMaterial;
    public Material roughMaterial;
    public Material wallMaterial;

    Dictionary<GridManager.spaceType, Material> spaceTypeMaterials;

    void Awake()
    {
        spaceTypeMaterials = new Dictionary<GridManager.spaceType, Material>()
        {
            {GridManager.spaceType.Empty, emptyMaterial },
            //{GridManager.spaceType.Goal, goalMaterial },
            {GridManager.spaceType.Rough, roughMaterial },
            {GridManager.spaceType.Wall, wallMaterial },
        };
    }

    public void GridInit()
    {
        gridManager = GetComponentInParent<GridManager>();
        transform.position = gridManager.GetWorldPosition(x, y);
        UpdateSpaceTypeDisplay();
    }

    public void UpdateSpaceTypeDisplay()
    {
        if(spaceType != GridManager.spaceType.Wall)
        {
            cubePivot.localScale = new Vector3(0.9f, 0.1f, 0.9f);
        }
        else
        {
            cubePivot.localScale = new Vector3(0.9f, 1f, 0.9f);

        }

        cubeMesh.material = spaceTypeMaterials[spaceType];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButton(0))
        {
            spaceType = GridManager.spaceType.Empty;
            UpdateSpaceTypeDisplay();
            gridManager.goal.x = x; gridManager.goal.y = y;
            gridManager.goal.GridInit();
        }
        if (Input.GetMouseButtonDown(1))
        {
            spaceType = (GridManager.spaceType)(((int)spaceType + 1) % Enum.GetNames(typeof(GridManager.spaceType)).Length);
            UpdateSpaceTypeDisplay();

        }
    }
}
