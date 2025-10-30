using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    // Start is called before the first frame update

    public GridManager gridManager;
    
    public void StartMovement()
    {
        gridManager.agent.TryStartMovement();
    }

    public void IncreaseSize()
    {
        PersistentData.GridSize = PersistentData.GridSize + 1;
        SceneManager.LoadScene("GridScene", LoadSceneMode.Single);
    }
    public void DecreaseSize()
    {
        PersistentData.GridSize = Mathf.Max(PersistentData.GridSize - 1,6);
        SceneManager.LoadScene("GridScene", LoadSceneMode.Single);
    }
}
