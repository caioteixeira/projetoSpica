using UnityEngine;
using System.Collections;

public class TextInstantiate : MonoBehaviour {

	public GameObject textoPrefab;
	
	public bool exibiu = false;
	
	void OnTriggerEnter (Collider collider) {
		if(collider.gameObject.tag == "Player" && !exibiu)
		{
			Instantiate(textoPrefab);	
			exibiu = true;
		}
	}
}
