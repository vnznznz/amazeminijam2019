﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : Entity
{

    public int rowIndex = 0;
    public int colIndex = 0;

    public Tile currentTile;

    // Use this for initialization
    public override void init()
    {
        base.init();
    }

    // Update is called once per frame
    protected override void update()
    {


        switch (this.level.currentStatus)
        {
            case GameStatus.Playing:
                this.currentTile = this.level.map.getTileAt(this.rowIndex, this.colIndex);
                wander();
                if (this.currentTile.State == TileState.GOAL)
                {
                    this.level.gameWon();
                }
                break;
        }
    }

Vector3 walkingDirection = Vector3.zero;
    void wander()
    {
        walkingDirection = getWalkingDir(walkingDirection);

    }

    Vector3 getWalkingDir(Vector3 currentDirection)
    {
        // get current tile
        var current = tileAtWorldPos(this.transform.position);

        // look to the left when we are walking to the left
        if (currentDirection.x <= 0)
        {
            var leftNeighbor = tileAtIndices(current.rowIndex, current.colIndex - 1);
            if (leftNeighbor != null)
            {
                if (leftNeighbor.State == TileState.WALKABLE || leftNeighbor.State == TileState.GOAL)
                {
                    return Vector3.left;
                }
                else if (leftNeighbor.State == TileState.BLOCKED)
                {
                    return Vector3.right;
                }
                else
                {
                    return Vector3.zero;
                }
            }
            else
            { // we are at the edge of the map
                return Vector3.right;
            }
        } else {
			// we are walking to the right 
			var rightNeighbor = tileAtIndices(current.rowIndex, current.colIndex + 1);
            if (rightNeighbor != null)
            {
                if (rightNeighbor.State == TileState.WALKABLE || rightNeighbor.State == TileState.GOAL)
                {
                    return Vector3.right;
                }
                else if (rightNeighbor.State == TileState.BLOCKED)
                {
                    return Vector3.left; // bounce
                }
                else
                {
                    return Vector3.zero;
                }
            }
            else
            { // we are at the edge of the map
                return Vector3.left;
            }
		}

    }

    Tile tileAtWorldPos(Vector3 pos)
    {
        var rowIndex = (int)(Level.TILESIZE * Level.MAPHEIGHT - pos.y / Level.TILESIZE);
        var colIndex = (int)(pos.x / Level.TILESIZE);

        return tileAtIndices(rowIndex, colIndex);
    }

    Tile tileAtIndices(int rowIndex, int colIndex)
    {
        if (areIndicesInsideMap(rowIndex, colIndex))
            return level.map.content[rowIndex, colIndex];
        return null;
    }
    bool areIndicesInsideMap(int rowIndex, int colIndex)
    {
        return rowIndex >= 0 && rowIndex < Level.MAPHEIGHT && colIndex >= 0 && colIndex < Level.MAPWIDTH;
    }

}
