using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[DontStore]
[AddComponentMenu("Storage/Store Information")]
public class StoreInformation : UniqueIdentifier
{
	public bool StoreAllComponents = true;
	public Dictionary<string, bool> Components = new Dictionary<string, bool>();

}

