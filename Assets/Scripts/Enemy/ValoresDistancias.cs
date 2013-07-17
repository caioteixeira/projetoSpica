using System.Collections;
using UnityEngine;

[System.Serializable]
public class Distancia
{
	public enum distancias
	{
		MuitoPerto,
		Perto,
		Longe,
		MuitoLonge
	}
	
}
[System.Serializable]
public class ValDistancias
{
	public float tolerancia;
	public float distanciaMax;
	public float distanciaMin;
	
	[HideInInspector] public Distancia.distancias distancia;
}
public static class ValoresDistancias
{
	public static ValDistancias muitoPerto, perto, longe, muitoLonge;
}