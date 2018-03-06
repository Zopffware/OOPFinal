using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static BonusTypeUtilities;

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

    public class MatchesInfo
    {
        private List<GameObject> matchedCandies;

        public void AddObject(GameObject go)
        {
            if (!matchedCandies.Contains(go))
                matchedCandies.Add(go);
        }

        public MatchesInfo()
        {
            matchedCandies = new List<GameObject>();
            BonusesContained = BonusType.None;
        }

        public BonusType BonusesContained { get; set; }
    }

    public class ShapesArray
    {

        private GameObject[,] shapes = new GameObject[Constants.Rows, Constants.Columns];

        public GameObject this[int row, int column]
        {

            get
            {
                try
                {
                    return shapes[row, column];
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            set
            {
                shapes[row, column] = value;
            }
        }

        public void Swap(GameObject g1, GameObject g2)
        {
            //hold backup in case of no match
            backupG1 = g1;
            backupG2 = g2;

            var g1Shape = g1.GetComponent<Shape>();
            var g2Shape = g2.GetComponent<Shape>();

            //array indexes
            int g1Row = g1Shape.Row;
            int g1Column = g1Shape.Column;
            int g2Row = g2Shape.Row;
            int g2Column = g2Shape.Column;

            //swap in the array
            var temp = shapes[g1Row, g1Column];
            shapes[g1Row, g1Column] = shapes[g2Row, g2Column];
            shapes[g2Row, g2Column] = temp;

            //swap their properties
            Shape.SwapColumnRow(g1Shape, g2Shape);
        }

        public void UndoSwap()
        {
            if (backupG1 == null || backupG2 == null)
                throw new Exception("Backup is null");

            Swap(backupG1, backupG2);
        }

        private GameObject backupG1;
        private GameObject backupG2;


        private IEnumerable<GameObject> GetMatchesHorizontally(GameObject go)
        {
            List<GameObject> matches = new List<GameObject>();
            matches.Add(go);
            var shape = go.GetComponent<Shape>();
            //check left
            if (shape.Column != 0)
                for (int column = shape.Column - 1; column >= 0; column--)
                {
                    if (shapes[shape.Row, column].GetComponent<Shape>().IsSameType(shape))
                    {
                        matches.Add(shapes[shape.Row, column]);
                    }
                    else
                        break;
                }

            //check right
            if (shape.Column != Constants.Columns - 1)
                for (int column = shape.Column + 1; column < Constants.Columns; column++)
                {
                    if (shapes[shape.Row, column].GetComponent<Shape>().IsSameType(shape))
                    {
                        matches.Add(shapes[shape.Row, column]);
                    }
                    else
                        break;
                }

            //we want more than three matches
            if (matches.Count < Constants.MinimumMatches)
                matches.Clear();

            return matches.Distinct();
        }

        private IEnumerable<GameObject> GetMatchesVertically(GameObject go)
        {
            List<GameObject> matches = new List<GameObject>();
            matches.Add(go);
            var shape = go.GetComponent<Shape>();
            //check bottom
            if (shape.Row != 0)
                for (int row = shape.Row - 1; row >= 0; row--)
                {
                    if (shapes[row, shape.Column] != null &&
                        shapes[row, shape.Column].GetComponent<Shape>().IsSameType(shape))
                    {
                        matches.Add(shapes[row, shape.Column]);
                    }
                    else
                        break;
                }

            //check top
            if (shape.Row != Constants.Rows - 1)
                for (int row = shape.Row + 1; row < Constants.Rows; row++)
                {
                    if (shapes[row, shape.Column] != null &&
                        shapes[row, shape.Column].GetComponent<Shape>().IsSameType(shape))
                    {
                        matches.Add(shapes[row, shape.Column]);
                    }
                    else
                        break;
                }

            if (matches.Count < Constants.MinimumMatches)
                matches.Clear();

            return matches.Distinct();
        }

        private IEnumerable<GameObject> GetEntireRow(GameObject go)
        {
            List<GameObject> matches = new List<GameObject>();
            int row = go.GetComponent<Shape>().Row;
            for (int column = 0; column < Constants.Columns; column++)
            {
                matches.Add(shapes[row, column]);
            }
            return matches;
        }

        private IEnumerable<GameObject> GetEntireColumn(GameObject go)
        {
            List<GameObject> matches = new List<GameObject>();
            int column = go.GetComponent<Shape>().Column;
            for (int row = 0; row < Constants.Rows; row++)
            {
                matches.Add(shapes[row, column]);
            }
            return matches;
        }

        private bool ContainsDestroyRowColumnBonus(IEnumerable<GameObject> matches)
        {
            if (matches.Count() >= Constants.MinimumMatches)
            {
                foreach (var go in matches)
                {
                    if (BonusTypeUtilities.ContainsDestroyWholeRowColumn
                        (go.GetComponent<Shape>().Bonus))
                        return true;
                }
            }

            return false;
        }
    }
}




