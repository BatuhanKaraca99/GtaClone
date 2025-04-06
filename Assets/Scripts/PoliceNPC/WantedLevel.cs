using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WantedLevel : MonoBehaviour
{
    public Player player;
    public bool level1 = false;
    public bool level2 = false;
    public bool level3 = false;
    public bool level4 = false;
    public bool level5 = false;

    private void Update()
    {
        if(player.currentkills == 1)
        {
            level1 = true;
        }
        if(player.currentkills >= 3)
        {
            level2 = true;
        }
        if(player.currentkills >= 5)
        {
            level3 = true;
        }
        if(player.currentkills >= 10)
        {
            level4 = true;
        }
        if(player.currentkills > 11)
        {
            level5 = true;
        }
    }
}
