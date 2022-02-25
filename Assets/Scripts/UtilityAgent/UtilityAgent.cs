using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class UtilityAgent : Agent
{
    [SerializeField] Perception perception;
    [SerializeField] MeterUI meter;

    const float MIN_SCORE = 0.2f;

    Need[] needs;

    UtilityObject activeUtilityObject = null;
    public bool isUsingUtilityObject { get { return activeUtilityObject != null; } }
    public float happiness
    {
        get
        {
            float totalMotive = 0;
            foreach (var need in needs)
            {
                totalMotive += need.motive;
            }

            return 1 - (totalMotive / needs.Length);
        }
    }

    void Start()
    {
        needs = GetComponentsInChildren<Need>();
        meter.text.text = "";
    }
    void Update()
    {
        animator.SetFloat("speed", movement.velocity.magnitude);

        if (activeUtilityObject == null)
        {
            var gameObjects = perception.GetGameObjects();
            List<UtilityObject> utilityObjects = new List<UtilityObject>();
            foreach (var go in gameObjects)
            {
                if (go.TryGetComponent<UtilityObject>(out UtilityObject utilityObject))
                {
                    utilityObject.visible = true;
                    utilityObject.score = GetUtilityObjectScore(utilityObject);
                    if (utilityObject.score > MIN_SCORE) utilityObjects.Add(utilityObject);
                }
            }

            activeUtilityObject = (utilityObjects.Count == 0) ? null : utilityObjects[0];

            if (activeUtilityObject != null)
            {
                StartCoroutine(ExecuteUtilityObject(activeUtilityObject));
            }
        }
    }

    private void LateUpdate()
    {
        meter.slider.value = happiness;
        meter.worldPosition = transform.position + Vector3.up * 4;
    }

    IEnumerator ExecuteUtilityObject(UtilityObject utilityObject)
    {
        movement.MoveTowards(utilityObject.location.position);
        while (Vector3.Distance(transform.position, utilityObject.location.position) > 0.25f)
        {
            Debug.DrawLine(transform.position, utilityObject.location.position);

            yield return null;
        }

        print("start effect");

        if (utilityObject.effect != null) utilityObject.effect.SetActive(true);

        yield return new WaitForSeconds(utilityObject.duration);

        print("stop effect");

        if (utilityObject.effect != null) utilityObject.effect.SetActive(false);

        ApplyUtilityObject(utilityObject);

        activeUtilityObject = null;

        yield return null;
    }

    float ApplyUtilityObject(UtilityObject utilityObject)
    {
        foreach (var effector in utilityObject.effectors)
        {
            Need need = GetNeedByType(effector.type);
            if (need != null)
            {
                need.input += effector.change;
                need.input = Mathf.Clamp(need.input, -1, 1);
            }
        }
        return 0.2f;
    }

    float GetUtilityObjectScore(UtilityObject utilityObject)
    {
        float score = 0;

        foreach (var effector in utilityObject.effectors)
        {
            Need need = GetNeedByType(effector.type);
            if (need != null)
            {
                float futureNeed = need.GetMotive(need.input + effector.change);
            }
        }

        return score;
    }

    Need GetNeedByType(Need.Type type)
    {
        return needs.First(need => need.type == type);
    }

    /*private void OnGUI()
    {
        Vector2 screen = Camera.main.WorldToScreenPoint(transform.position);

        GUI.color = Color.black;
        int offset = 0;
        foreach (var need in needs)
        {
            GUI.Label(new Rect(screen.x + 20, Screen.height - screen.y - offset, 300, 20), need.type.ToString() + ": " + need.motive);
            offset += 20;
        }
        //GUI.Label(new Rect(screen.x + 20, Screen.height - screen.y - offset, 300, 20), mood.ToString());
    }*/
}
