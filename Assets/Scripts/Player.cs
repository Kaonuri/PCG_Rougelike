﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;	//Allows us to use UI.

public class Player : MovingObject
{
	public int wallDamage = 1;					//How much damage a player does to a wall when chopping it.
	public Text healthText;						//UI Text to display current player health total.
	private Animator animator;					//Used to store a reference to the Player's animator component.
	private int health;							//Used to store player health points total during level.
    public static Vector2 position;
	
	protected override void Start ()
	{
		animator = GetComponent<Animator>();
		
		health = GameManager.instance.healthPoints;
		
		healthText.text = "Health: " + health;
		
		base.Start ();

	    position.x = position.y = 2;
	}
	
	private void Update ()
	{
		if(!GameManager.instance.playersTurn) return;
		
		int horizontal = 0;  	//Used to store the horizontal move direction.
		int vertical = 0;		//Used to store the vertical move direction.

	    bool canMove = false;

		horizontal = (int) (Input.GetAxisRaw ("Horizontal"));
		
		vertical = (int) (Input.GetAxisRaw ("Vertical"));
		
		if(horizontal != 0)
		{
			vertical = 0;
		}

		if(horizontal != 0 || vertical != 0)
		{
			canMove = AttemptMove<Wall> (horizontal, vertical);
		    if (canMove)
		    {
		        position.x += horizontal;
		        position.y += vertical;
		        GameManager.instance.UpdateBoard(horizontal, vertical);
		    }
		}
	}
	
	protected override bool AttemptMove <T> (int xDir, int yDir)
	{	
		bool hit = base.AttemptMove <T> (xDir, yDir);
		
		GameManager.instance.playersTurn = false;

		return hit;
	}
	
	protected override void OnCantMove <T> (T component)
	{
		Wall hitWall = component as Wall;
		
		hitWall.DamageWall (wallDamage);
		
		animator.SetTrigger ("playerChop");
	}
	
	public void LoseHealth (int loss)
	{
		animator.SetTrigger ("playerHit");
		
		health -= loss;
		
		healthText.text = "-"+ loss + " Health: " + health;

		CheckIfGameOver ();
	}
	
	private void CheckIfGameOver ()
	{
		if (health <= 0) 
		{	
			GameManager.instance.GameOver ();
		}
	}
}

