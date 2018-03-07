using UnityEngine;
using System.Collections;

[System.Serializable]
public class Game
{

    public static Game current;
    public Character CS;
    public Character CPP;
    public Character Java;
    public Character Py;
    public Character HTML;
    public Character JC;
    public int lovePoints;
    public int day;
    public int hour;


    public Game()
    {
        CS = new Character();
        CPP = new Character();
        Java = new Character();
        Py = new Character();
        HTML = new Character();
        JC = new Character();
        lovePoints = 0;
        day = 0;
        hour = 0;
    }

}
