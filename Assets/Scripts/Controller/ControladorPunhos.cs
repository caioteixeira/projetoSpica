using UnityEngine;
using System.Collections;

public class ControladorPunhos : MonoBehaviour {

	public GameObject punhoEsquerdo;
	public GameObject punhoDireito;
	
	public float socoFort = 30.0f;
	public float soco = 10.0f;
	
	public void resetaPunhos()
	{
		punhoEsquerdo.GetComponent<PlayerPunch>().golpe.power = 0;
		punhoDireito.GetComponent<PlayerPunch>().golpe.power = 0;
	}
	public void DidStrongPunch()
	{
		punhoDireito.GetComponent<PlayerPunch>().golpe.power = socoFort;
		punhoEsquerdo.GetComponent<PlayerPunch>().golpe.power = 0;
	}
	public void DidPunch()
	{
		punhoDireito.GetComponent<PlayerPunch>().golpe.power = soco;
		
		punhoEsquerdo.GetComponent<PlayerPunch>().golpe.power = soco;
	}
	
}
