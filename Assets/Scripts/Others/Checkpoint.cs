using UnityEngine;
using System.Collections;
using Serialization;

public class Checkpoint : MonoBehaviour {
	
	public GameObject CheckpointText;
	public GameObject Particles;
	
	ParticleSystem particleSystem;
	public float taxaRevitalizacao;
	bool isEnter;
	bool isSaved;
	
	Player player;
	
	// Use this for initialization
	void Start () {
		particleSystem = Particles.GetComponent<ParticleSystem>();
		particleSystem.enableEmission = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(isEnter)
		{
			if(player.hp < player.hpMax){
				player.hp += taxaRevitalizacao/Time.deltaTime;
			}
			if(player.hp>player.hpMax)
			{
				player.hp = player.hpMax;
			}
		}
	
	}
	
	void OnTriggerEnter(Collider collider)
	{
		if(collider.gameObject.tag == "Player")
		{	
			if(!isSaved)
			{
				Instantiate(CheckpointText);
				LevelSerializer.SaveGame("Game");
				Radical.CommitLog();
				isSaved = true;
			}
			particleSystem.enableEmission = true;
			isEnter = true;
			player = collider.gameObject.GetComponent<Player>();
			
			
			
		}
	}
	void OnTriggerExit(Collider collider)
	{
		isEnter = false;
		player = null;
		particleSystem.enableEmission = false;
	}
}
