using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Store this component when saving data
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class StoreComponent : Attribute
{
	
}

[AttributeUsage(AttributeTargets.Class)]
public class DontStoreAttribute : Attribute
{
	
}

[ExecuteInEditMode]
[DontStore]
[AddComponentMenu("Storage/Unique Identifier")]
public class UniqueIdentifier : MonoBehaviour
{
	public static void ClearAllNames ()
	{
		AllIdentifiers.Clear ();
	}
	
	

	public string _id = string.Empty;

	public string Id {
		get {
			return _id;
		}
		set {
			_id = value;
			SaveGameManager.Instance.SetId (gameObject, value);
		}
	}

	public static GameObject GetByName (string id)
	{
		var result = SaveGameManager.Instance.GetById (id);
		return result ?? GameObject.Find (id);
	}

	public static List<UniqueIdentifier> AllIdentifiers = new List<UniqueIdentifier> ();
	[HideInInspector]
	public string classId = Guid.NewGuid ().ToString ();
	
	public string ClassId {
		get {

			return classId;
		}
		set {
			if (string.IsNullOrEmpty (value)) {
				value = Guid.NewGuid ().ToString ();
			}
			classId = value;
		}
	}

	void Awake ()
	{
		SaveGameManager.Initialize (() =>
		{
			ConfigureId ();
			foreach (var c in GetComponentsInChildren<UniqueIdentifier>(true).Where(c=>c.gameObject.active == false)) {
				c.ConfigureId ();
			}
		});
	}

	void ConfigureId ()
	{
		_id = SaveGameManager.GetId (gameObject);
		AllIdentifiers.Add (this);
	}

	void OnDestroy ()
	{
		AllIdentifiers.Remove (this);
	}

}






