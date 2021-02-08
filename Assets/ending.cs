using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ending : MonoBehaviour
{
    public GameObject plug;
    public GameManager gameManager;
   public Switch eeaeda;
    public GameObject congs;
    public bool ee;
  
    void Update()
    {

        if(eeaeda.turn == 1 && !ee)
        {
            gameManager.PlaySound("b");
            plug.SetActive(false);
            ee = true;
            congs.SetActive(true);
        }
        if (ee)
            if (Input.GetKeyDown(KeyCode.Return))
                gameManager.NextLevel();
        
    }

}
