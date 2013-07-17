public var waitTime:float;
public var fadeTime:float;
var timer:float;
var fadeValue:float;
	
function Update () {
	
	if(timer>waitTime)
	{
		fadeValue -= Mathf.Clamp01(Time.deltaTime/fadeTime);
		
		if(fadeValue < 0)
		{
			GameObject.Destroy(gameObject);
		}	
		
	}
	else
	{
		timer += Time.deltaTime;
	}
	guiText.font.material.color.a = fadeValue;
}