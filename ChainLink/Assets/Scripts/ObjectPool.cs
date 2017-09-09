﻿using System;

using UnityEngine;

/// <summary>
/// Repository of commonly used prefabs.
/// </summary>

[AddComponentMenu("Gameplay/ObjectPool")]
public class ObjectPool : MonoBehaviour
{
		public static ObjectPool instance { get; private set; }
	#region member
		/// <summary>
		/// Member class for a prefab entered into the object pool
		/// </summary>
		[Serializable]
		public class ObjectPoolEntry
		{
				/// <summary>
				/// the object to pre instantiate
				/// </summary>
				[SerializeField]
				public GameObject Prefab;
				/// <summary>
				/// quantity of object to pre-instantiate
				/// </summary>
				[SerializeField]
				public int Count;
				[HideInInspector]
				public GameObject[] pool;
				[HideInInspector]
				public int objectsInPool = 0;
		}

	#endregion
		/// <summary>
		/// The object prefabs which the pool can handle
		/// by The amount of objects of each type to buffer.
		/// </summary>

		public ObjectPoolEntry[] Entries;

		/// <summary>
		/// The pooled objects currently available.
		/// Indexed by the index of the objectPrefabs
		/// </summary>
		/// <summary>
		/// The container object that we will keep unused pooled objects so we dont clog up the editor with objects.
		/// </summary>
		protected GameObject ContainerObject;
		void OnEnable ()
		{
				instance = this;
		}
		// Use this for initialization
		void Start ()
		{
				ContainerObject = new GameObject ("ObjectPool");
				//Loop through the object prefabs and make a new list for each one.
				//We do this because the pool can only support prefabs set to it in the editor,
				//so we can assume the lists of pooled objects are in the same order as object prefabs in the array
				for (int i = 0; i < Entries.Length; i++) {
						ObjectPoolEntry objectPrefab = Entries [i];
						//create the repository
						objectPrefab.pool = new GameObject[objectPrefab.Count];
						//fill it
						for (int n = 0; n < objectPrefab.Count; n++) {
								GameObject newObj = (GameObject)Instantiate (objectPrefab.Prefab);
								newObj.name = objectPrefab.Prefab.name;
								PoolObject (newObj);
						}
				}
		}
		/// <summary>
		/// Gets a new object for the name type provided.  If no object type exists or if onlypooled is true and there is no objects of that type in the pool
		/// then null will be returned.
		/// </summary>
		/// <returns>
		/// The object for type.
		/// </returns>
		/// <param name='objectType'>
		/// Object type.
		/// </param>
		/// <param name='onlyPooled'>
		/// If true, it will only return an object if there is one currently pooled.
		/// </param>
		public GameObject GetObjectForType (string objectType)
		{
			return GetObjectForType(objectType,false);
		}
		public GameObject GetObjectForType (string objectType, bool onlyPooled)
		{
				for (int i = 0; i < Entries.Length; i++) {
						GameObject prefab = Entries [i].Prefab;
						if (prefab.name != objectType)
								continue;
						if (Entries [i].objectsInPool > 0) {
								GameObject pooledObject = Entries [i].pool [--Entries [i].objectsInPool];
								pooledObject.transform.parent = null;
								pooledObject.SetActive(true);
								return pooledObject;
						} else if (!onlyPooled) {
								GameObject obj = (GameObject)Instantiate (Entries [i].Prefab);
								obj.name = obj.name + "_not_pooled";
								return obj;
						}
				}
				//If we have gotten here either there was no object of the specified type or non were left in the pool with onlyPooled set to true
				return null;
		}
	/// <summary>
	/// Pools the object specified.  Will not be pooled if there is no prefab of that type.
	/// </summary>
	/// <param name='obj'>
	/// Object to be pooled.
	/// </param>
	public void PoolObject (GameObject obj)
	{
		for (int i = 0; i < Entries.Length; i++) 
		{
			if (Entries [i].Prefab==null || Entries [i].Prefab.name != obj.name)
				continue;
			obj.SetActive (false);
			obj.transform.parent = ContainerObject.transform;
			if (obj.GetComponent<Rigidbody>() != null) 
			{
				obj.GetComponent<Rigidbody>().velocity = Vector3.zero;
			}
			//reset all colliders
			Collider2D[] childColliders = obj.GetComponentsInChildren<Collider2D>();
			for(int x=0;x<childColliders.Length;x++)
			{
				childColliders[x].enabled = false;
			}
			if(Entries [i]!=null)
			{
//				Debug.Log (Globals.levelSelect+" "+i+ " "+Entries [i].objectsInPool);
				Entries [i].pool [Entries [i].objectsInPool++] = obj;
			}
			else
			{
				Debug.Log ("entries " + i +" warning!!!");
			}
			return;
		}
		Destroy (obj);
	}
	void OnDestroy() {
		instance = null;
		//this needs a proper cleanup
	}

}