using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class ObjectiveController : MonoBehaviour
{
    private readonly Objective[] objectives = new Objective[]
    {
        new(ObjectiveType.PressButton, "Find the Magical Button of Wonder that definitely won't harm you."),
        new(ObjectiveType.HitCar, "Driving golf balls is easy. Try driving a car."),
        new(ObjectiveType.EnterHole, "Make your way into the hole!"),
    };
    private int currentIndex = 0;
    private Objective currentObjective;
    private TMPro.TextMeshProUGUI textComponent;
    private Transform completionMarker;

    void Start()
    {
        currentObjective = objectives[currentIndex];
        textComponent = GetComponent<TMPro.TextMeshProUGUI>();
        completionMarker = transform.Find("CompletionMark");

        SetText();
    }

    void FixedUpdate()
    {
        if (currentObjective.IsComplete && !completionMarker.gameObject.activeSelf)
        {
            // Show that the objective was complete and set a timer for when the next objective will display
            completionMarker.gameObject.SetActive(true);

            StartCoroutine(DelayNextObjective(5));
        }
    }

    public static ObjectiveController Instance()
    {
        var controllers = GameObject.FindGameObjectsWithTag("GameController");
        for (var i = 0; i < controllers.Length; i++)
        {
            var objectiveScript = controllers[i].GetComponent<ObjectiveController>();
            if (objectiveScript != null)
            {
                return objectiveScript;
            }
        }
        return null;
    }

    public Objective GetObjective(ObjectiveType type)
    {
        return objectives.Where(search => search.Type == type).First();
    }

    private IEnumerator DelayNextObjective(float delay)
    {
        yield return new WaitForSeconds(delay);

        completionMarker.gameObject.SetActive(false);

        currentIndex++;
        if (currentIndex < objectives.Length)
        {
            currentObjective = objectives[currentIndex];
        }
        else
        {
            currentObjective = null;
        }

        SetText();
    }

    private void SetText()
    {
        var newText = "OBJECTIVE:\n";
        if (currentIndex < objectives.Length)
        {
            newText += currentObjective.Desc;
        }
        else
        {
            newText = "You have completed all the objectives. Well done!";
        }

        if (currentObjective.Quantity > 1)
        {
            newText += string.Format("\n{0}/{1}", currentObjective.CompletionCount, currentObjective.Quantity);
        }

        if (textComponent != null)
        {
            textComponent.text = newText;
        }
    }
}

public class Objective
{
    public ObjectiveType Type { get; }
    public string Desc { get; }
    public int Quantity { get; }
    public int CompletionCount { get; private set; } = 0;
    public bool IsComplete { get; private set; } = false;

    public Objective(ObjectiveType type, string desc, int quantity = 1)
    {
        Type = type;
        Desc = desc;
        Quantity = quantity;
    }

    public void Increment(int amount = 1)
    {
        CompletionCount = Math.Min(CompletionCount + amount, Quantity);
        if (CompletionCount >= Quantity)
        {
            Complete();
        }
    }

    private void Complete()
    {
        if (IsComplete)
        {
            return;
        }

        IsComplete = true;
        CompletionCount = Quantity;
    }
}

public enum ObjectiveType
{
    PressButton,
    EnterHole,
    HitCar
}