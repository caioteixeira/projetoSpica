using UnityEngine;
using System.Collections;
public class EnemyFunctions {
	#region Testes de distância    
	
	#region Muito Perto
    public static float EstaMuitoPerto(float distancia, float distanciaMax, float tolerancia)
    {
        if (distancia > distanciaMax)
        {
            if ((distancia - distanciaMax) > tolerancia)
            {
                return 0.0f;
            }
            else
            {
                return 1.0f - (distancia - distanciaMax) / tolerancia;
            }
        }
        else
        {
            return 1.0f;
        }
    }
	
	public static float EstaMuitoPerto(ValDistancias valores, float distancia)
    {
		float distanciaMax = valores.distanciaMax;
		float tolerancia = valores.tolerancia;
		
       	return EstaMuitoPerto(distancia, distanciaMax, tolerancia);
    }
	#endregion
	
	#region Perto
	public static float EstaPerto (float distancia, float distanciaMax, float tolerancia) {
        if (distancia > distanciaMax)
        {
            if ((distancia - distanciaMax) > tolerancia)
            {
                return 0.0f;
            }
            else
            {
                return 1.0f - (distancia - distanciaMax) / tolerancia;
            }
        }
        else
        {
            return 1.0f;
        }
	}
	
	public static float EstaPerto (ValDistancias valores, float distancia) {
       
		float distanciaMax = valores.distanciaMax;
		float tolerancia = valores.tolerancia;
		
		return EstaPerto(distancia, distanciaMax, tolerancia);
		
	}
	#endregion
	
	#region Longe
    public static float EstaLonge(float distancia, float distanciaMin, float distanciaMax, float tolerancia)
    {
        float pontoMedioLimites = (distanciaMax + distanciaMin) / 2;
        if (distancia == pontoMedioLimites)
        {
            return 1.0f;
        }
        else
        {
            float diferenca = distancia - pontoMedioLimites;
            diferenca = diferenca < 0 ? diferenca * -1 : diferenca;

            if (diferenca > tolerancia)
                return 0.0f;

            return 1.0f - diferenca / tolerancia;
        }
       
    }
	public static float EstaLonge (ValDistancias valores, float distancia) {
       
		float distanciaMax = valores.distanciaMax;
		float distanciaMin = valores.distanciaMin;
		float tolerancia = valores.tolerancia;
		
		return EstaLonge(distancia, distanciaMin, distanciaMax, tolerancia);	
	}
	#endregion
	
	#region Muito Longe
    public static float EstaMuitoLonge(float distancia, float distanciaMin, float tolerancia)
    {
        if (distancia >= distanciaMin)
        {
            return 1.0f;
        }
      
        else
        {
            float diferenca = distancia - distanciaMin;
            diferenca = diferenca < 0 ? diferenca * -1 : diferenca;

            if (diferenca > tolerancia)
                return 0.0f;

            return 1.0f - diferenca / tolerancia;
		}
    }
	public static float EstaMuitoLonge (ValDistancias valores, float distancia) {
       
		
		float distanciaMin = valores.distanciaMin;
		float tolerancia = valores.tolerancia;
		
		return EstaMuitoLonge(distancia, distanciaMin, tolerancia);	
	}
	#endregion
	
	#endregion
	
	public static bool EnemyRay(float campoDeVisao, GameObject gameObject, Color rayColor, int layer)	
	{	
		RaycastHit rayCastHit = new RaycastHit();
		LayerMask layerMask = 1 << layer;
		
		if(Physics.Raycast(gameObject.transform.position, gameObject.transform.TransformDirection(Vector3.forward), out rayCastHit, campoDeVisao, layerMask))
		{
			Debug.DrawRay(gameObject.transform.position,gameObject.transform.TransformDirection(Vector3.forward), rayColor,campoDeVisao);
			//Debug.Log("Visualisou...:D");
			return true;
		}
		else
		{
			Debug.DrawRay(gameObject.transform.position,gameObject.transform.TransformDirection(Vector3.forward), rayColor,campoDeVisao);
			return false;	
		}
		
	}
	public static bool EnemyRay(float campoDeVisao, GameObject gameObject, Color rayColor)	
	{	
		RaycastHit rayCastHit = new RaycastHit();
		
		if(Physics.Raycast(gameObject.transform.position, gameObject.transform.TransformDirection(Vector3.forward), out rayCastHit, campoDeVisao))
		{
			Debug.DrawRay(gameObject.transform.position,gameObject.transform.TransformDirection(Vector3.forward), rayColor,campoDeVisao);
			Debug.Log("Visualisou...:D");
			return true;
		}
		else
		{
			return false;	
		}
		
	}
	
    /*public static float inimigoForte(Player npc, Player enemy)
    {
        float somaNPC = npc.defesa + npc.defesaEspiritual + npc.manipulacao; 
		float somaEnemy = enemy.stamina + enemy.poderEspiritual + enemy.manipulacao;
		
		float final = somaEnemy/somaNPC;
		
		if(final - 1.0f <= -1)
		{
			return 0.0f;	
		}
		else if(final - 1.0f >= 1)
		{
			return 1.0f;
		}
		else
		{
			
			if(final >= 0.5)
			{
				return final - 0.5f;
			}
			if(final < 0.5)
			{
				return 0.5 - final;
			}
		}		  
    }
    public static float inimigoFraco(Player npc, Player enemy)
    {
        float soma;



        return 0.0f;
    }*/
}
