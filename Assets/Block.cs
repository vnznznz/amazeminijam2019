﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : Entity {

    public int x = -1;//On height
    public int y = 0;//on width

    public bool started = false;
    public bool landed = false;

    public Tile currentTile = null;

	// Use this for initialization
	public override void init(){
        base.init();

        this.gameObject.transform.position = new Vector3(this.y * Level.TILESIZE, (Level.TILESIZE * Level.MAPHEIGHT), 0.0f);

        if (!this.started) {
            StartCoroutine(this.updateMove());
        }
	}
	
	// Update is called once per frame
	protected override void update(){
		
	}

    public void rotateLeft()
    {
        this.gameObject.transform.Rotate(new Vector3(0.0f, 0.0f, 90.0f), Space.World);
    }

    public void rotateRight()
    {
        this.gameObject.transform.Rotate(new Vector3(0.0f, 0.0f, -90.0f), Space.World);
    }

    public void moveLeft()
    {
        Tile tileOnLeft = this.level.map.getTileAt(this.x, this.y-1);
        if (tileOnLeft != null && tileOnLeft.State == TileState.FREE && this.y > 0) {
            if (this.currentTile != null)
            {
                this.currentTile.State = TileState.FREE;
            }

            this.y--;
            this.gameObject.transform.position += Vector3.left * Level.TILESIZE;

            Tile newTile = this.level.map.getTileAt(this.x, this.y);
            if (newTile != null)
            {
                this.currentTile = newTile;
                newTile.State = TileState.BLOCKED;
            }
        }
    }

    public void moveRight()
    {
        Tile tileOnRight = this.level.map.getTileAt(this.x, this.y+1);
        if (tileOnRight != null && tileOnRight.State == TileState.FREE && this.y < Level.MAPWIDTH - 1)
        {
            if (this.currentTile != null)
            {
                this.currentTile.State = TileState.FREE;
            }

            this.y++;
            this.gameObject.transform.position += Vector3.right * Level.TILESIZE;

            Tile newTile = this.level.map.getTileAt(this.x, this.y);
            if (newTile != null)
            {
                this.currentTile = newTile;
                newTile.State = TileState.BLOCKED;
            }
        }
    }

    public void land()
    {
        this.landed = true;
        this.level.activeBlock = null;
        this.level.spawnBlock(Random.Range(0, Level.MAPWIDTH));

        Tile newTile = this.level.map.getTileAt(this.x, this.y);
        if (newTile != null)
        {
            this.currentTile = newTile;
            newTile.State = TileState.BLOCKED;
        }
    }

    private IEnumerator updateMove()
    {
        this.started = true;

        bool continued = true;

        while (continued)
        {
            if (!this.landed)
            {
                if (this.currentTile != null)
                {
                    this.currentTile.State = TileState.FREE;
                }

                this.gameObject.transform.position += Vector3.down * Level.TILESIZE;
                this.x++;

                Tile newTile = this.level.map.getTileAt(this.x, this.y);
                if (newTile != null) {
                    this.currentTile = newTile;
                    newTile.State = TileState.BLOCKED;
                }

                if (this.x == Level.MAPHEIGHT - 1)
                {
                    this.land();
                }

                Tile tileBelow = this.level.map.getTileAt(this.x + 1, this.y);
                if (tileBelow != null && tileBelow.State != TileState.FREE)
                {
                    this.land();
                }
            }

            if (!this.level.accelerate) {
                yield return new WaitForSeconds(Level.BLOCKSMOVELOOPRATE);
            }
            else
            {
                yield return new WaitForSeconds(Level.BLOCKSMOVELOOPRATE / 10.0f);
            }
        }
    }
}
