using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnedObject : MonoBehaviour
{
    [Serializable]
    public class SaveSpawnedObjectData
    {
        public int objectId;
        public Vector3 worldPosition;

        public SaveSpawnedObjectData(int objectId, Vector3 worldPosition)
        {
            this.objectId = objectId;
            this.worldPosition = worldPosition;
        }
    }

    public int objId;

    public void SpawnedObjectDestroyed()
    {
        transform.parent.GetComponent<ObjectSpawner>().SpawnedObjectDestroyed(this);
    }
}
