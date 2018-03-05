using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    [System.Flags]
    public enum BonusType
    {
        None,
        DestroyWholeRowColumn
    }

    public static class BonusTypeUtilities
    {

        public static bool ContainsDestroyWholeColumn(BonusType bt)
        {
            return (bt & BonusType.DestroyWholeRowColumn)
                == BonusType.DestroyWholeRowColumn;
        }

        public enum GameState
        {
            None,
            SelectionStarted,
            Animating
        }

        public static class Constants
        {
            public static readonly int Rows = 12;
            public static readonly int Columns = 8;
            public static readonly float AnimationDuration = 0.2f;

            public static readonly float MoveAnimaationMinDuration = 0.05f;

            public static readonly float ExplosionDuration = 0.3f;

            public static readonly float WaitBeforePotentialMatchesCheck = 2f;
            public static readonly float OpacityAnimationFrameDelay = 0.05f;

            public static readonly int MinimumMatches = 3;
            public static readonly int MinimumMatchesForBonus = 5;

            public static readonly int Match3Score = 60;
            public static readonly int SubsequentMatchScore = 1000;
        }
    }

 public class Shape : MonoBehaviour
    {
       public BonusType Bonus { get; set; }
       public int Column { get; set; }
        public int Row { get; set; }

        public string Type { get; set; }

        public Shape()
    {
        Bonus = BonusType.None;
    }

        public bool IsSameType(Shape otherShape)
    {
        if (otherShape == null || !(otherShape is Shape))
        {
            throw new ArgumentException("otherShape");
        }

        return string.Compare(this.Type, (otherShape as Shape).Type) == 0;
    }

    public void Assign(string type, int row, int column)
    {

        if (string.IsNullOrEmpty(type))
            throw new ArgumentException("type");

        Column = column;
        Row = row;
        Type = type;
    }

    public static void SwapColumnRow(Shape a, Shape b)
    {
        int temp = a.Row;
        a.Row = b.Row;
        b.Row = temp;

        temp = a.Column;
        a.Column = b.Column;
        b.Column = temp;
    }

    }
//add sound maybe?
//add hints?

    public class AlteredCandyInfo
{
    private List<GameObject> newCandy { get; set; }
    public int MaxDistance { get; set; }

    public void AddCandy(GameObject go)
    {
        if (!newCandy.Contains(go))
            newCandy.Add(go);
    }

    public AlteredCandyInfo()
    {
        newCandy = new List<GameObject>();
    }

}


