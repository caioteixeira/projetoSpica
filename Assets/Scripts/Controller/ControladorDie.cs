using UnityEngine;
using System.Collections;

public class ControladorDie : MonoBehaviour {
	
	private bool isDead = false;

	void Start(){
		if(!isDead){
			animation["dead"].wrapMode = WrapMode.Once;
			animation.Play("dead");
			isDead = true;
		}
	}

	void Update(){
		if(!animation.isPlaying)
			Application.LoadLevel("GameOver");
	}
	
	
}
