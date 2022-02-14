using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistancePerception : Perception {

    public override GameObject[] GetGameObjects() {
        List<GameObject> result = new List<GameObject>();

        Collider[] colliders = Physics.OverlapSphere(transform.position, distance);

        foreach (Collider collider in colliders) {
            if (collider.gameObject == gameObject) continue;
            if (tagName == "" || collider.CompareTag(tagName)) {
                result.Add(collider.gameObject);
            }
        }

        return result.ToArray();
    }
}
