using UnityEngine;
using System.Collections;

public class RandonTextureBlood : MonoBehaviour {
	
	public string nomeMenuPricipal;
	public GUISkin guiSkin;  		
	
	public Texture[] textures;
	
	void Start(){
		int decisao;
		if (UnityEngine.Random.value>0.5f){
			decisao = 1;
		}else if(UnityEngine.Random.value < 0.2f){
			decisao = 2;
		}else if(UnityEngine.Random.value < 0.7f){
			decisao = 3;
		}else
			decisao = 4;
		
		switch(decisao){
		case 1:
			renderer.material.mainTexture = textures[0];
			break;
		case 2:
			renderer.material.mainTexture = textures[1];
			break;
		case 3:
			renderer.material.mainTexture = textures[2];
			break;
		case 4:
			renderer.material.mainTexture = textures[3];
			break;
		}
	}
}
