using UnityEngine;
using System.Collections.Generic;

namespace Utility.Memory
{
	public class ObjectPool
	{
		public class ObjectPoolKeyCollisionException : System.Exception
		{
			public ObjectPoolKeyCollisionException()
			{
			}

			public ObjectPoolKeyCollisionException(string message) : base(message)
			{
			}
		}

		private Dictionary<string, Queue<GameObject>> instancePool;
		private Dictionary<string, int> keyToIDMap;

		private Transform instanceParent;

		public Transform InstanceParent
		{
			get{ return null; }
			set{ instanceParent = value; }
		}

		public ObjectPool()
		{
			instancePool = new Dictionary<string, Queue<GameObject>>();
			keyToIDMap = new Dictionary<string, int>();
		}

		public void Preallocate(GameObject prefab, int count, Transform parent = null)
		{
			if(instanceParent == null)
			{
				instanceParent = parent;
			}
			
			if(instancePool.ContainsKey(prefab.name) == false)
			{
				AddToPool(prefab);
			}
			for(int i = 0; i < count; ++i)
			{
				GameObject instance = GameObject.Instantiate(prefab) as GameObject;
				if(parent != null)
				{
					instance.transform.SetParent(parent, false);
				}
				instance.name = prefab.name;
				instancePool[prefab.name].Enqueue(instance);
			}
		}

		private void AddToPool(GameObject prefab)
		{
			instancePool.Add(prefab.name, new Queue<GameObject>());
			keyToIDMap.Add(prefab.name, prefab.GetInstanceID());
		}

		public GameObject GetInstance(GameObject prefab)
		{
			string key = prefab.name;
			GameObject instance = null;
			if(instancePool.ContainsKey(key))
			{
				CheckKeyCollision(prefab);
				if(instancePool[key].Count > 0)
				{
					instance = instancePool[key].Dequeue();
				}
			}
			else
			{
				AddToPool(prefab);
			}

			if(instance == null)
			{
				instance = GameObject.Instantiate(prefab) as GameObject;
				instance.name = key;
			}
			if(instanceParent != null && instance.transform.parent != instanceParent)
			{
				instance.transform.parent = instanceParent;
			}
			return instance;
		}

		public void StashInstance(GameObject instance)
		{
			instancePool[instance.name].Enqueue(instance);
		}

		public void ClearAllInstances()
		{
			foreach (KeyValuePair<string, Queue<GameObject>> kvp in instancePool)
			{
				Queue<GameObject> toClear = kvp.Value;
				instancePool.Remove(kvp.Key);
				toClear.Clear();
			}
			keyToIDMap.Clear();
		}

		private void CheckKeyCollision(GameObject prefab)
		{
			if(keyToIDMap[prefab.name] != prefab.GetInstanceID())
			{
				throw new ObjectPoolKeyCollisionException("Collision for key " + prefab.name);
			}
		}
	}
}
