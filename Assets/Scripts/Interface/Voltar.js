
var levelToLoad: String;

function Update () {
	if(Input.anyKeyDown)
		Application.LoadLevel(levelToLoad);
}