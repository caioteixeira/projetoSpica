// /* ------------------
//       ${Name} 
//       (c)3Radical 2012
//           by Mike Talbot 
//     ------------------- */
// 
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using Serialization;

[ExecuteInEditMode]
[AddComponentMenu("Storage/Save Game Manager")]
[DontStore]
public class SaveGameManager : MonoBehaviour
{
	public static SaveGameManager Instance;
	public static bool hasRun;
	
	public static void Loaded()
	{
		_cached = null;
		Instance.Reference.Clear();
	}

	[Serializable]
	public class StoredEntry
	{
		public GameObject gameObject;
		public string Id = Guid.NewGuid().ToString();
	}

	public List<StoredEntry> Reference = new List<StoredEntry>();
	
	private static List<StoredEntry> _cached = new List<StoredEntry>();
	private static List<Action> _initActions = new List<Action>();


	public GameObject GetById(string id)
	{
		if(id==null)
			return null;
		return  Reference.Where(r=>r.Id==id && r.gameObject != null).Select(r=>r.gameObject).FirstOrDefault();
		
	}
	
	
	
	public void SetId(GameObject gameObject, string id)
	{
		
		var rr = Reference.FirstOrDefault(r=>r.gameObject == gameObject) ?? Reference.FirstOrDefault(r=>r.Id == id);
		if(rr != null)
		{
			rr.Id = id;
			rr.gameObject = gameObject;
		} else
		{
			rr =new StoredEntry { gameObject = gameObject, Id = id };
			Reference.Add(rr);
		}
	}

	public static string GetId(GameObject gameObject)
	{
		var Reference = Instance.Reference;
		var entry = Reference.FirstOrDefault(r=>r.gameObject == gameObject);
		if(entry != null)
			return entry.Id;
		Reference.Add(entry = new StoredEntry { gameObject = gameObject});
		return entry.Id;
	}
	
	public static void Initialize(Action a)
	{
		if(Instance != null)
		{
			a();
		}
		else
		{
			_initActions.Add(a);
		}
	}
	
	static List<UnityEngine.Object> _assets = new List<UnityEngine.Object>();
	void GetAllReferences()
	{
		_assets = Resources.FindObjectsOfTypeAll(typeof(AnimationClip))
			.Concat(Resources.FindObjectsOfTypeAll(typeof(AudioClip)))
			.Concat(Resources.FindObjectsOfTypeAll(typeof(Mesh)))
			.Concat(Resources.FindObjectsOfTypeAll(typeof(Material)))
		    .ToList();
	}
	
	public static int GetAssetId(UnityEngine.Object obj)
	{
		return _assets.IndexOf(obj);
	}
	
	public static T GetAsset<T>(int id) where T : UnityEngine.Object
	{
		return id==-1 ? null :(T)_assets[id];
	}
	
	
	
	void Awake()
	{
		GetAllReferences();
		Instance = this;
		if(Application.isPlaying && !hasRun)
		{
			_cached = Reference;
			hasRun = true;
		}
		else if(_cached != null) {
			hasRun = false;
			Reference = _cached.Where(a=>a.gameObject != null).ToList();
		}
		if(_initActions.Count > 0)
		{
			foreach(var a in _initActions)
			{
				a();
			}
			_initActions.Clear();
		}
	}
}


