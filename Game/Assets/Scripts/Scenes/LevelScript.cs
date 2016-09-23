using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class LevelScript
{
	public Transform[] Spawners { set; get; }
    private int[] validSpawnIndexs;
    private int numberOfSpawned = -1;

    internal GameObject InstanciateFighter(string name, PlayerInput input)
    {
        FightersManager.Fighter newF = FightersManager.Fighters.Where(e => e.name.ToLower().Equals(name.ToLower())).FirstOrDefault();
        PlayeParam player = new PlayeParam(newF.prefab, input);

        return InstanciateFighter(player);
    }

    public void Init(Transform[] spawnPoints)
    {
        Spawners = spawnPoints;
        LoadValidSpawnIndexs();
    }

    private void LoadValidSpawnIndexs()
    {
        validSpawnIndexs = new int[Spawners.Length];
        for (int i = 0; i < Spawners.Length; i++)
        {
            validSpawnIndexs[i] = i;
        }
    }

    GameObject InstanciateFighter(PlayeParam player) {
        int index = GetValidIndex();
        GameObject p = Instanciate(player.prefab, Spawners[index]) as GameObject;
        FighterBase fb = p.GetComponent<FighterBase>();
        fb.MyInput = player.input;

        return p;
    }

    public int GetValidIndex()
    {
        int index = Random.Range(0, validSpawnIndexs.Length);
        if (numberOfSpawned.Equals(-1))
        {
            numberOfSpawned = index;
            validSpawnIndexs = validSpawnIndexs.Where(n => !n.Equals(numberOfSpawned)).ToArray();
        }
        else
        {
            numberOfSpawned = -1;
            LoadValidSpawnIndexs();
        }

        return index;
    }

    public Object Instanciate (GameObject f, Transform s) {
		return GameObject.Instantiate(f, s.position, s.rotation);
	}

    public Object Instanciate(Component f, Vector3 s)
    {
        return GameObject.Instantiate(f, s, Quaternion.identity);
    }

    internal class PlayeParam {
        internal  GameObject prefab;
        internal PlayerInput input;

        public PlayeParam(GameObject prefab, PlayerInput input) {
            this.input = input;
            this.prefab = prefab;
        }
    }

}
