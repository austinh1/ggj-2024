using System;
using UnityEngine;

public class ObjectiveController : MonoBehaviour
{
    private Objective[] objectives = new Objective[]
    {
        new(ObjectiveType.PressButton, "Find the Magical Button of Wonder that definitely won't harm you."),
        new(ObjectiveType.HitCar, "Driving golf balls is easy. Try driving a car."),
        new(ObjectiveType.EnterHole, "Make your way into the hole!"),
    };
    private int currentIndex = 0;
    private Objective currentObjective;
    private TMPro.TextMeshProUGUI textComponent;

    void Start()
    {
        currentObjective = objectives[currentIndex];
        textComponent = GetComponent<TMPro.TextMeshProUGUI>();

        SetText();
    }

    void FixedUpdate()
    {
        if (currentObjective.IsComplete)
        {
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
        IsComplete = true;
    }
}

public enum ObjectiveType
{
    PressButton,
    EnterHole,
    HitCar
}