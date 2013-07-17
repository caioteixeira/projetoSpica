using UnityEngine;
using System.Collections;

public class DeathZone : MonoBehaviour {

	void OnTriggerEnter(Collider other){
		if(other.collider.gameObject.tag == "Player"){
			Application.LoadLevel("GameOver");
		}
	}
}
