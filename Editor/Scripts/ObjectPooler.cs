using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ArchNet.Design.Pattern.ObjectPooler
{
	/// <summary>
	/// [MODULE] - [ARCH NET] - [DESIGN PATTERN] - Object Pooler
	/// author : LOUIS PAKEL
	/// </summary>
	public class ObjectPooler : MonoBehaviour
	{
		// List of PooledObject
		public List<GameObject> pooledObjects;

		// Prefab to pool
		public GameObject objectToPool;

		// Max instance of prefab
		public int amountToPool;

		private void Start()
		{
			pooledObjects = new List<GameObject>();


			// Find and load _scrollViewRect
			GameObject lContainer = GameObject.Find("Container") as GameObject;

			for (int i = 0; i < amountToPool; i++)
			{
				AddPooledObject(lContainer.transform);
			}
		}

		/// <summary>
		/// Description : Update pool list size and adjust current list
		/// </summary>
		/// <param name="pNewSize"></param>
		public void UpdatePooledObjects(int pNewSize, Transform pParent)
		{
			if (pNewSize > amountToPool)
			{
				int lNewAmount = pNewSize - amountToPool;

				for (int i = 0; i < lNewAmount; i++)
				{
					AddPooledObject(pParent);
				}
			}
			else if (pNewSize < amountToPool)
			{
				int lNewAmount = amountToPool - pNewSize;

				for (int i = 0; i < lNewAmount; i++)
				{
					RetrievePooledObject(pooledObjects.Last());
				}
			}

			amountToPool = pNewSize;
		}


		/// <summary>
		/// Description : Add a new pooled object into the pool
		/// </summary>
		private void AddPooledObject(Transform pParent)
		{
			// instantiate new pooled object
			GameObject lPooledObject = (GameObject)Instantiate(objectToPool, pParent.transform);

			// Set Parent Position for the pooled object
			lPooledObject.transform.SetParent(pParent, false);

			// Desactivate pooled object
			lPooledObject.SetActive(false);

			// Aadd pooled object to the pool
			pooledObjects.Add(lPooledObject);
		}

		/// <summary>
		/// Description : Delete / Retrieve a pooled object from the pool
		/// </summary>
		/// <param name="pPooledObject"></param>
		private void RetrievePooledObject(GameObject pPooledObject)
		{
			for(int i = 0; i < pooledObjects.Count; i++)
			{
				// Find similare instance ID
				if(pPooledObject.GetInstanceID() == pooledObjects[i].GetInstanceID())
				{
					pooledObjects[i].SetActive(false);
				}
			}

		}

		/// <summary>
		/// Description : Get a new pooled Object from queue
		/// </summary>
		public GameObject GetPooledObject()
		{
			for (int i = 0; i < pooledObjects.Count; i++)
			{
				if (!pooledObjects[i].activeInHierarchy)
				{
					return pooledObjects[i];
				}
			}
			return null;
		}
	}
}
