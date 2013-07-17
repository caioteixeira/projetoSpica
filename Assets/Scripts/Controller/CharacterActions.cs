using UnityEngine;
using System.Collections;

public class CharacterActions{
	
	GameObject gameOject;

	public CharacterActions(GameObject player){
		this.gameOject = player;
	}
	
	public void IdlePlayer(){
		gameOject.SendMessage("IdleAnimationPlayer");
	}
	public void WalkPlayer(){
		gameOject.SendMessage("WalkAnimationPlayer");
	}
}
