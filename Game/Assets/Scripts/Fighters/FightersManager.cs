using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class FightersManager
{
    public class Fighter
    {
        public string name;
        public GameObject prefab;

        public Fighter(string name, GameObject prefab)
        {
            this.name = name;
            this.prefab = prefab;
        }
    }

    private static List<Fighter> fighters = new List<Fighter>();
    public static List<Fighter> Fighters {
        get
        {
            if (fighters.Count == 0)
            {
                LoadDefaultPrefabs();
            }

            return fighters;
        }
    }

    private static List<Fighter> simpleFighters = new List<Fighter>();
    public static List<Fighter> SimpleFighters
    {
        get
        {
            if (simpleFighters.Count == 0)
            {
                LoadSimpleFighters();
            }

            return simpleFighters;
        }
    }

    private static void LoadSimpleFighters()
    {
        GameObject[] f = Resources.LoadAll<GameObject>("Prefabs/SelectAnimation");
        foreach (GameObject o in f)
        {
            PlayerSimple ps = o.GetComponent<PlayerSimple>();
            simpleFighters.Add(new Fighter(ps.Name, o));
        }
    }

    private static void LoadDefaultPrefabs()
    {
        GameObject[] f = Resources.LoadAll<GameObject>("Prefabs/Characters/PlayerPrefabs") ;
        foreach (GameObject go in f)
        {
            FighterBase ps = go.GetComponent<FighterBase>();
            fighters.Add(new Fighter(ps.playerName, go));
        }
    }
}
