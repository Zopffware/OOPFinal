using UnityEngine;
using System.Collections;

[System.Serializable]
public class Character
{

    public int lovePoints;
    public string name;

    public Character()
    {
        this.lovePoints = 0;
        this.name = "";
    }
}