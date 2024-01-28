using System;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public enum ObjectiveType
{
    PressButton,
    EnterHole,
    HitCar,
    ClimbMountain,
    MaxSwing,
    RightClickToBrake,
    PumpkinForest,
    ObstacleCourse,
    FireHydrant,
    Bowling,
    Donut,
    StickyPizza,
    Dominoes,
    DumpsterFire,
    Orangatang,
}

public class ObjectiveController : MonoBehaviour
{
    public TMP_Text playerText;
    
    private readonly Objective[] objectives = new Objective[]
    {
        new(ObjectiveType.Dominoes, "Knock over some dominoes!", "Bet that fridge had some tasty stuff in it."),
        new(ObjectiveType.StickyPizza, "Pizza time!", "A dish best served cold, in my opinion."),
        new(ObjectiveType.Donut, "Time to grab a snack! Where's that donut again?"),
        new(ObjectiveType.Bowling, "Strike! Smack some pins around and pretend you're a bowling ball."),
        new(ObjectiveType.ObstacleCourse, "Show me your skillz by collecting the trophy at the top of the tower!"),
        new(ObjectiveType.FireHydrant, "Become a firefighter by using all of the fire hydrants.", "", 7),
        new(ObjectiveType.MaxSwing, "Swing for the moon! Achieve a full power swing."),
        new(ObjectiveType.RightClickToBrake, "Cool it there hot shot. Press right click to slow yourself down!", "Don't make me turn this golf ball around!"),
        new(ObjectiveType.PressButton, "Find the Magical Button of Wonder that definitely won't harm you."),
        new(ObjectiveType.HitCar, "Driving golf balls is easy. Try driving a car."),
        new(ObjectiveType.ClimbMountain, "Make the long trek up a treacherous mountain."),
        new(ObjectiveType.EnterHole, "Make your way into the hole!"),
        new(ObjectiveType.PumpkinForest, "Travel into the mystical pumpkin forest, but beware of what you find..."),
        new(ObjectiveType.DumpsterFire, "Start a dumpster fire for the heck of it.", "Honestly? Kinda toasty in here, I like it."),
        new(ObjectiveType.Orangatang, "Gaze in the upward direction, I dare you.", "AAAHH he's been up there the whole time?!"),
    };
    private Objective currentObjective;
    private TMPro.TextMeshProUGUI textComponent;
    private Transform completionMarker;

    public UnityEvent<ObjectiveType> onObjectiveCompleted = new();

    void Start()
    {
        currentObjective = GetNextObjective();
        textComponent = GetComponent<TMPro.TextMeshProUGUI>();
        completionMarker = transform.Find("CompletionMark");
        onObjectiveCompleted.AddListener((ot) =>
        {
            var objective = GetObjective(ot);
            SetText(objective);
            SetCompletionText(objective.CompletionPlayerText);
            
            if (!completionMarker.gameObject.activeSelf)
            {
                // Show that the objective was complete and set a timer for when the next objective will display
                completionMarker.gameObject.SetActive(true);
            }
            
            StartCoroutine(DelayNextObjective(5));
        });

        SetText(currentObjective);
    }

    private void SetCompletionText(string objectiveCompletionPlayerText)
    {
        playerText.text = objectiveCompletionPlayerText;
        StartCoroutine(ClearCompletionTextAfterDelay(3f));
    }

    private IEnumerator ClearCompletionTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        playerText.text = string.Empty;
    }

    private Objective GetNextObjective()
    {
        return objectives.FirstOrDefault(o => !o.IsComplete);
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
        return objectives.First(search => search.Type == type);
    }

    private IEnumerator DelayNextObjective(float delay)
    {
        yield return new WaitForSeconds(delay);

        completionMarker.gameObject.SetActive(false);
        currentObjective = GetNextObjective();

        SetText(currentObjective);
    }

    public void SetText(Objective objective)
    {
        var newText = "OBJECTIVE:\n";
        if (objective != null)
        {
            newText += objective.Desc;
            if (objective.Quantity > 1)
            {
                newText += string.Format("\n{0}/{1}", objective.CompletionCount, objective.Quantity);
            }
        }
        else
        {
            newText = "You have completed all the objectives. Well done!";
        }

        if (textComponent != null)
        {
            textComponent.text = newText;
        }

        StopAllCoroutines();
    }
}

public class Objective
{
    public ObjectiveType Type { get; }
    public string Desc { get; }
    public int Quantity { get; }
    public string CompletionPlayerText { get; }
    public int CompletionCount { get; private set; } = 0;
    public bool IsComplete { get; private set; } = false;

    public Objective(ObjectiveType type, string desc, string completionPlayerText = "", int quantity = 1)
    {
        Type = type;
        Desc = desc;
        Quantity = quantity;
        CompletionPlayerText = completionPlayerText;
    }

    public void Increment(int amount = 1)
    {
        if (IsComplete)
        {
            return;
        }
        
        CompletionCount = Math.Min(CompletionCount + amount, Quantity);
        if (CompletionCount >= Quantity)
        {
            Complete();
        }
        else
        {
            ObjectiveController.Instance().SetText(this);
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
        ObjectiveController.Instance().onObjectiveCompleted.Invoke(Type);
    }
}
