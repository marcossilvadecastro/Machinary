using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Utils
{

    public static bool isDebug = true;

    public static Component GetComponentByName(Transform transform, string name)
    {
        return transform.GetComponentsInChildren<Component>().Where(g => g.tag.ToLower().Contains(name)).FirstOrDefault();
    }

    public static GameObject GetSpawnByName(GameObject[] element, string name)
    {
        return element.Where(a => a.name.ToLower().Contains(name)).First();
    }

    public static void LoadDebugPreferences()
    {
        if (isDebug)
        {
            PlayerPrefs.SetString("player1_control", "teclado");
            PlayerPrefs.SetString("player2_control", "teclado");

            PlayerPrefs.SetInt("player1_hero", 0);
            PlayerPrefs.SetInt("player2_hero", 1);
            PlayerPrefs.Save();
        }
        
    }

    public static string SelectControl(string value, int player)
    {
        string control = string.Format("player{0}T{0}", player);
        if (value.Equals("POSITION_2"))
        {
            control = string.Format("player{0}C{0}", player);
        }
        return control;
    }

    public static int SelectPlayer(string value)
    {
        int player = 0;
        if (value.Equals("POSITION_2"))
        {
            player = 1;
        }
        return player;
    }
}

