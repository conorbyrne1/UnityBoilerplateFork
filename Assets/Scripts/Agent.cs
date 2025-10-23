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

    /* D* Psuedo code taken from wikipedia: https://en.wikipedia.org/wiki/D*
    while (!openList.isEmpty()) {
    point = openList.getFirst();
    expand(point);

    void expand(currentPoint) {
    boolean isRaise = isRaise(currentPoint);
    double cost;
    for each (neighbor in currentPoint.getNeighbors()) {
        if (isRaise) {
            if (neighbor.nextPoint == currentPoint) {
                neighbor.setNextPointAndUpdateCost(currentPoint);
                openList.add(neighbor);
            } else {
                cost = neighbor.calculateCostVia(currentPoint);
                if (cost < neighbor.getCost()) {
                    currentPoint.setMinimumCostToCurrentCost();
                    openList.add(currentPoint);
                }
            }
        } else {
            cost = neighbor.calculateCostVia(currentPoint);
            if (cost < neighbor.getCost()) {
                neighbor.setNextPointAndUpdateCost(currentPoint);
                openList.add(neighbor);
            }
        }
    }
}
}

    boolean isRaise(point) {
    double cost;
    if (point.getCurrentCost() > point.getMinimumCost()) {
        for each(neighbor in point.getNeighbors()) {
            cost = point.calculateCostVia(neighbor);
            if (cost < point.getCurrentCost()) {
                point.setNextPointAndUpdateCost(neighbor);
            }
        }
    }
    return point.getCurrentCost() > point.getMinimumCost();
}
     */
}
