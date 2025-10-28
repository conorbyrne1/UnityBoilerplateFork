using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    // Start is called before the first frame update

    public GridManager gridManager;
    
    public void StartMovement()
    {
        gridManager.agent.TryStartMovement();
    }
}
