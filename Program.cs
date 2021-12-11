using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;


namespace RecursiveMaze
{
    class Program
    {
        private class Point
        {
            public int X;
            public int Y;

            public Point(int x, int y)
            {
                X = x;
                Y = y;
            }
        }

        private static List<List<Point>> mSolutionPathList = new List<List<Point>>();

        // This is the map. 
        // TODO: Store and Read as JSON file so you can create any map outside of the code
        private static Point mSize = new Point(6, 6);
        private static Point mStartLocation = new Point(0, 5);
        private static Point mGoalLocation = new Point(5, 1);
        private static List<Point> mMapBlocked = new List<Point>()
        {
            new Point(1,5),new Point(2,5),new Point(3,5),new Point(4,5),new Point(5,5),
            new Point(0,3),new Point(2,3),new Point(4,3),
            new Point(0,2),new Point(2,2),new Point(4,2),
            new Point(3,1),
            new Point(0,0),new Point(1,0),new Point(5,0)
        };
        /////////////////////////////////////////////////////////
        /// Test Maze #2 
        /*private static Point mStartLocation = new Point(5, 3);
        private static Point mGoalLocation = new Point(0, 4);
        private static List<Point> mMapBlocked = new List<Point>()
        {
            new Point(1,4),new Point(2,4),new Point(3,4),new Point(4,4),
            new Point(3,3),
            new Point(0,2),new Point(1,2),new Point(3,2),new Point(5,2),
            new Point(5,1),
            new Point(0,0),new Point(1,0),new Point(2,0),new Point(3,0),new Point(5,0),
        };*/

        static void Main(string[] args)
        {
            List<Point> currentPath = new List<Point>() { mStartLocation };

            FindSolutions(mStartLocation, null, mStartLocation);

            Debug.WriteLine("\nResults:\n");
            // Test it
            foreach (List<Point> testList in mSolutionPathList)
            {
                if (!ListsAreSame(mTestCorrectPath1, testList) && !ListsAreSame(mTestCorrectPath2, testList))
                    Debug.WriteLine("Incorrect Result");
                else
                {
                   PrintPath(testList);
                }
            }
        }

        static void FindSolutions(Point currentLocation, List<Point> currentPath, Point lastLocation)
        {
            if (currentPath == null)
                currentPath = new List<Point>();
            
            int pathLength = currentPath.Count;

            // End recursion when goal met
            if (currentLocation.X == mGoalLocation.X && currentLocation.Y == mGoalLocation.Y)
            {
                List<Point> clonedList = CloneList(currentPath);
                clonedList.Insert(0, mStartLocation);
                clonedList.Add(mGoalLocation);
                mSolutionPathList.Add(clonedList);
                return;
            }

            // Add the next path point 
            if (!ArePointsEqual(mStartLocation, currentLocation))
                currentPath.Add(currentLocation);

            // The code below could be refactored, but I'm leaving it as is so that it's clear as to how it works 
            // Note that we need to clone the current path each time to be able to visit every previous node
            // when backtracking
            
            // East (x + 1)
            Point nextEast = new Point(currentLocation.X + 1, currentLocation.Y);
            if (!ArePointsEqual(lastLocation, nextEast) && IsOnTheMap(nextEast) && !IsBlocked(nextEast))
            {
                FindSolutions(nextEast, CloneList(currentPath), currentLocation);
            }

            // South (y - 1)
            Point nextSouth = new Point(currentLocation.X, currentLocation.Y - 1);
            if (!ArePointsEqual(lastLocation, nextSouth) && IsOnTheMap(nextSouth) && !IsBlocked(nextSouth))
            {
                FindSolutions(nextSouth, CloneList(currentPath), currentLocation);
            }

            // West (x - 1)
            Point nextWest = new Point(currentLocation.X - 1, currentLocation.Y);
            if (!ArePointsEqual(lastLocation, nextWest) && IsOnTheMap(nextWest) && !IsBlocked(nextWest))
            {
                FindSolutions(nextWest, CloneList(currentPath), currentLocation);
            }

            // North (y + 1)
            Point nextNorth = new Point(currentLocation.X, currentLocation.Y + 1);
            if (!ArePointsEqual(lastLocation, nextNorth) && IsOnTheMap(nextNorth) && !IsBlocked(nextNorth))
            {
                FindSolutions(nextNorth, CloneList(currentPath), currentLocation);
            }
        }

        static List<Point> CloneList(List<Point> list)
        {
            return new List<Point>(list.ToList());
        }

        static bool ArePointsEqual(Point point1, Point point2)
        {
            return point1.X == point2.X && point1.Y == point2.Y;
        }

        static bool IsBlocked(Point testPoint)
        {
            return mMapBlocked.Any(pnt => pnt.X == testPoint.X && pnt.Y == testPoint.Y);
        }

        static bool IsOnTheMap(Point testPoint)
        {
            return testPoint.Y >= 0 && testPoint.Y < mSize.Y && testPoint.X >= 0 && testPoint.X < mSize.X;
        }

        static void PrintPath(List<Point> pointList)
        {

            foreach (Point point in pointList)
            {

                Debug.Write($"({point.X},{point.Y}) ");

            }
            Debug.WriteLine("\n");
        }

        static bool ListsAreSame(List<Point> test1, List<Point> test2)
        {
            if (test1.Count != test2.Count)
                return false;

            foreach (Point testPoint in test1)
            {
                if (test2.Any(pnt => pnt.X == testPoint.X && pnt.Y == testPoint.Y))
                    continue;
                return false;
            }
            return true;
        }

        private static List<Point> mTestCorrectPath1 = new List<Point>()
        {
            new Point(0,5),
            new Point(0,4),new Point(1,4),
            new Point(1,3),
            new Point(1,2),
            new Point(1,1), new Point(2,1), new Point(4,1), new Point(5,1),
            new Point(2,0),new Point(3,0),new Point(4,0)
        };

        private static List<Point> mTestCorrectPath2 = new List<Point>()
        {
            new Point(0,5),
            new Point(0,4),new Point(1,4), new Point(2,4), new Point(3,4),new Point(4,4), new Point(5,4),
            new Point(5,3),new Point(5,2),new Point(5,1),
        };
    }
}
