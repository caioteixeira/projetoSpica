using UnityEngine;
using System.Collections;

public class FimDemoTrigger : MonoBehaviour {

	void OnTriggerEnter(Collider other){
		if(other.collider.gameObject.tag == "Player"){
			Application.LoadLevel("FimDemo");
		}
	}
}
