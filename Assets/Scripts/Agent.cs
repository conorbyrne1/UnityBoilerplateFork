using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Agent : MonoBehaviour
{
    // Start is called before the first frame update

    GridManager gridManager;
    public int x;
    public int y;

    // D* Algorithm data structures
    private class Node
    {
        public int x, y;
        public NodeState state;
        public float h;  // Current cost estimate
        public float k;  // Minimum cost (used for sorting in OPEN list)
        public Node backPointer;  // Points to next node in path (toward goal)

        public Node(int x, int y)
        {
            this.x = x;
            this.y = y;
            this.state = NodeState.NEW;
            this.h = float.MaxValue;
            this.k = float.MaxValue;
            this.backPointer = null;
        }
    }
    private enum NodeState
    {
        NEW,    // Node hasn't been visited
        OPEN,   // Node is in the OPEN list
        CLOSED  // Node has been processed
    }

    private Node[,] nodes;
    private List<Node> openList;
    private Node goalNode;
    private List<Vector2Int> currentPath;

    // Visualization
    public LineRenderer pathLine;
    public float moveSpeed = 2f;
    private bool isMoving = false;


    void Start()
    {
        //Vector2D<Vector2D<int>> /* or Vector2D<int><int>*/openlist = Vector2D[gridManager.width][gridManager.height];
        //while(!openlist.isEmpty())
        //{
        //    int point = openlist.first;
        //    expand(point);
        //}
    }
    //void expand(int currentPoint)
    //{
    //    boolean isRaised = isRaised(currentPoint);
    //    double cost
    //}

    
    public void GridInit()
    {
        gridManager = GetComponentInParent<GridManager>();
        transform.position = gridManager.GetWorldPosition(x, y);

        // Initialize line renderer for path visualization
        if (pathLine == null)
            {
                pathLine = gameObject.AddComponent<LineRenderer>();
                pathLine.startWidth = 0.1f;
                pathLine.endWidth = 0.1f;
                pathLine.material = new Material(Shader.Find("Sprites/Default"));
                pathLine.startColor = Color.yellow;
                pathLine.endColor = Color.yellow;
            }
    }

    // Update is called once per frame
    void Update()
    {
        // Press Space to calculate and follow path
        if (Input.GetKeyDown(KeyCode.Space) && !isMoving)
        {
            CalculatePath();
        }

        // Press M to move along the path
        if (Input.GetKeyDown(KeyCode.M) && currentPath != null && currentPath.Count > 0 && !isMoving)
        {
            StartCoroutine(MoveAlongPath());
        }
    }

    public void CalculatePath()
    {
        if (gridManager.goal == null)
        {
            Debug.LogError("No goal set!");
            return;
        }

        InitializeDStar();
        ProcessState();
        ExtractPath();
        VisualizePath();
    }
    private void InitializeDStar()
    {
        nodes = new Node[gridManager.width, gridManager.height];
        for (int i = 0; i < gridManager.width; i++)
        {
            for (int j = 0; j < gridManager.height; j++)
            {
                nodes[i, j] = new Node(i, j);
            }
        }

        openList = new List<Node>();

        goalNode = nodes[gridManager.goal.x, gridManager.goal.y];
        goalNode.h = 0;
        goalNode.state = NodeState.OPEN;
        Insert(goalNode, 0);
    }

    private float ProcessState()
    {
        Node agentNode = nodes[x, y];

        while (openList.Count > 0)
        {
            Node current = MinState();

            if (current == null)
                break;

            if (current == agentNode)
                break;

            float kOld = GetMinK();
            Delete(current);

            if (kOld < current.h)
            {
                // RAISE state
                foreach (Node neighbor in GetNeighbors(current))
                {
                    if (neighbor.h <= kOld && current.h > neighbor.h + Cost(neighbor, current))
                    {
                        current.backPointer = neighbor;
                        current.h = neighbor.h + Cost(neighbor, current);
                    }
                }
            }

            if (kOld == current.h)
            {
                // LOWER state
                foreach (Node neighbor in GetNeighbors(current))
                {
                    if (neighbor.state == NodeState.NEW ||
                        (neighbor.backPointer == current && neighbor.h != current.h + Cost(current, neighbor)) ||
                        (neighbor.backPointer != current && neighbor.h > current.h + Cost(current, neighbor)))
                    {
                        neighbor.backPointer = current;
                        Insert(neighbor, current.h + Cost(current, neighbor));
                    }
                }
            }
            else
            {
                // RAISE state propagation
                foreach (Node neighbor in GetNeighbors(current))
                {
                    if (neighbor.state == NodeState.NEW ||
                        (neighbor.backPointer == current && neighbor.h != current.h + Cost(current, neighbor)))
                    {
                        neighbor.backPointer = current;
                        Insert(neighbor, current.h + Cost(current, neighbor));
                    }
                    else
                    {
                        if (neighbor.backPointer != current && neighbor.h > current.h + Cost(current, neighbor))
                        {
                            Insert(current, current.h);
                        }
                        else
                        {
                            if (neighbor.backPointer != current && current.h > neighbor.h + Cost(neighbor, current) &&
                                neighbor.state == NodeState.CLOSED && neighbor.h > kOld)
                            {
                                Insert(neighbor, neighbor.h);
                            }
                        }
                    }
                }
            }
        }

        return GetMinK();
    }

    private void Insert(Node node, float hNew)
    {
        if (node.state == NodeState.NEW)
        {
            node.k = hNew;
        }
        else if (node.state == NodeState.OPEN)
        {
            node.k = Mathf.Min(node.k, hNew);
        }
        else // CLOSED
        {
            node.k = Mathf.Min(node.h, hNew);
        }

        node.h = hNew;
        node.state = NodeState.OPEN;

        if (!openList.Contains(node))
        {
            openList.Add(node);
            openList = openList.OrderBy(n => n.k).ToList();
        }
    }

    private void Delete(Node node)
    {
        openList.Remove(node);
        node.state = NodeState.CLOSED;
    }

    private Node MinState()
    {
        if (openList.Count == 0)
            return null;
        return openList[0];
    }

    private float GetMinK()
    {
        if (openList.Count == 0)
            return -1;
        return openList[0].k;
    }

    private List<Node> GetNeighbors(Node node)
    {
        List<Node> neighbors = new List<Node>();

        // 4-connected grid (up, down, left, right)
        int[] dx = { 0, 0, -1, 1 };
        int[] dy = { -1, 1, 0, 0 };

        for (int i = 0; i < 4; i++)
        {
            int newX = node.x + dx[i];
            int newY = node.y + dy[i];

            if (IsValid(newX, newY))
            {
                neighbors.Add(nodes[newX, newY]);
            }
        }

     

        return neighbors;
    }

    private bool IsValid(int x, int y)
    {
        return x >= 0 && x < gridManager.width && y >= 0 && y < gridManager.height;
    }

    private float Cost(Node from, Node to)
    {
        // Check if target cell is traversable
        GridManager.spaceType spaceType = gridManager.GetCellState(to.x, to.y);

        if (spaceType == GridManager.spaceType.Wall)
        {
            return float.MaxValue;  // CAnt move thru -> wall
        }

        // Calculate base distance
        float distance = Vector2.Distance(new Vector2(from.x, from.y), new Vector2(to.x, to.y));

        // cost multipiler
        switch (spaceType)
        {
            case GridManager.spaceType.Empty:
                return distance * 1f;
            case GridManager.spaceType.Rough:
                return distance * 3f;  // Rough terrain costs 3x more
            default:
                return distance;
        }
    }

    private void ExtractPath()
    {
        currentPath = new List<Vector2Int>();
        Node current = nodes[x, y];

        // Follow backpointers from agent to goal
        while (current != null && current != goalNode)
        {
            currentPath.Add(new Vector2Int(current.x, current.y));
            current = current.backPointer;

            if (currentPath.Count > gridManager.width * gridManager.height)
            {
                Debug.LogError("Path extraction failed - possible cycle detected");
                currentPath.Clear();
                return;
            }
        }

        if (current == goalNode)
        {
            currentPath.Add(new Vector2Int(goalNode.x, goalNode.y));
            Debug.Log($"Path found with {currentPath.Count} steps");
        }
        else
        {
            Debug.LogWarning("No path to goal found");
            currentPath.Clear();
        }
    }

    private void VisualizePath()
    {
        if (currentPath == null || currentPath.Count == 0)
        {
            pathLine.positionCount = 0;
            return;
        }

        pathLine.positionCount = currentPath.Count;
        for (int i = 0; i < currentPath.Count; i++)
        {
            Vector3 worldPos = gridManager.GetWorldPosition(currentPath[i].x, currentPath[i].y);
            worldPos.y = 0.5f;  // Raise path above grid
            pathLine.SetPosition(i, worldPos);
        }
    }

    private IEnumerator MoveAlongPath()
    {
        if (currentPath == null || currentPath.Count == 0)
            yield break;

        isMoving = true;

        for (int i = 0; i < currentPath.Count; i++)
        {
            Vector2Int targetPos = currentPath[i];
            Vector3 targetWorldPos = gridManager.GetWorldPosition(targetPos.x, targetPos.y);

            // Move to target position
            while (Vector3.Distance(transform.position, targetWorldPos) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetWorldPos, moveSpeed * Time.deltaTime);
                yield return null;
            }

            // Update agent grid position
            x = targetPos.x;
            y = targetPos.y;
        }

        isMoving = false;
        Debug.Log("Agent reached goal!");
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
