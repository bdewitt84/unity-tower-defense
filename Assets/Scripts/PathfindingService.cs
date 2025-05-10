using System.Collections.Generic;


// Author: Brett DeWitt
//
// Created on: 05/09/25
//
// Description:
//   Static class which provides methods for generating distance maps used for
//   path-finding on a grid
//


public static class PathfindingService
{
    const int sentinel = -1;

    // Finds the shortest distance to the goal from each cell on the GameBoard
    // grid, storing each value in the corresponding cell in distanceMap. The
    // algorithm is a breadth-first search (BFS) performed backwords, otherwise
    // known as a flood-fill.

    public static bool ComputeDistanceMap(
        GameBoard gameBoard,
        DistanceMap distanceMap,
        GridCoordinate goal,
        GridCoordinate start)
    {
        bool pathIsBlocked = false;
        distanceMap.Reset(sentinel);
        Queue<GridCoordinate> toVisit = new();
        distanceMap.SetDistance(goal, 0);
        toVisit.Enqueue(goal);
         
        while (toVisit.Count > 0)
        {
            GridCoordinate currentCell = toVisit.Dequeue();
            int currentDistance = distanceMap.GetDistance(currentCell);
            foreach (var neighbor in gameBoard.GetNeighbors(currentCell)) {
                if (!gameBoard.IsCellBlocked(neighbor) && distanceMap.GetDistance(neighbor) == sentinel)
                {
                    distanceMap.SetDistance(neighbor, currentDistance + 1);
                    toVisit.Enqueue(neighbor);
                }
            }
        }

        if (distanceMap.GetDistance(start) == sentinel)
        {
            pathIsBlocked = true;
        }

        ConvertSentinelToMax(distanceMap);

        return pathIsBlocked;
    }

    // Replaces all occurrances of the sentinel value in the distance map with
    // the distance map's maximum value
    private static void ConvertSentinelToMax(DistanceMap distanceMap)
    {
        for (int i = 0; i < distanceMap.Width; i++)
        {
            for (int j = 0; j < distanceMap.Height; j++)
            {
                GridCoordinate currentCell = new(i, j);
                if (distanceMap.GetDistance(currentCell) == sentinel)
                {
                    distanceMap.SetMaxDistance(currentCell);
                }
            }
        }
    }
}



