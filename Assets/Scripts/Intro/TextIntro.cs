using UnityEngine;
using System.Collections;

public class TextIntro: MonoBehaviour {
	
	public string Test;
	public GUIStyle style;
	public float recuo = 100;
	public float largura = 700;
	public float altura = 20000;
	float localAlt = 350;
	string message;
	public float letterPause = 0.2f;
	public float vel = 5.0f;
    public AudioClip sound;
	
	public string nomeDaCenaAlvo;

	void Start () {
		message = Test;
		Test = "";
		StartCoroutine(TypeText ());
	}
	void Update () {
		//Sobe a tela
		localAlt -= vel*Time.deltaTime;
	}
	void OnGUI()
	{
		GUI.TextArea(new Rect(recuo,localAlt,largura,altura),Test,style); //Desenha a TextArea
		
	}
	
    IEnumerator TypeText () {
        foreach (char letter in message.ToCharArray()) { //Gerencia a escrita do texto
            Test += letter;
            if (sound)
                audio.PlayOneShot (sound);
                yield return 0;
			
			
            yield return new WaitForSeconds (letterPause);
        }
		
		//Carrega cena alvo
		Application.LoadLevel(nomeDaCenaAlvo);
    }
}
