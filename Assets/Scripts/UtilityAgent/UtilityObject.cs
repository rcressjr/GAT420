using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilityObject : MonoBehaviour
{
    [System.Serializable]
    public class Effector
    {
        public Need.Type type;
        [Range(-2, 2)] public float change;
    }

    public float duration;
    public Transform location;

    public Effector[] effectors;
    public Dictionary<Need.Type, float> registry = new Dictionary<Need.Type, float>();

    void Start()
    {
        foreach(var effector in effectors) {
            registry[effector.type] = effector.change;
        }
    }

    // Update is called once per frame
    public float GetEffectorChange(Need.Type type)
    {
        registry.TryGetValue(type, out float change);

        return change;
    }

    public bool HasEffector(Need.Type type)
    {
        return registry.ContainsKey(type);
    }
}
