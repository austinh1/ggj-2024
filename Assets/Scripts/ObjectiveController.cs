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
    Fore,
    MovieCamera,
    VR,
    Spring,
    Slowmo
}

public class ObjectiveController : MonoBehaviour
{
    private readonly Objective[] objectives = new Objective[]
    {
        new(ObjectiveType.Fore, "Swing ya club!", "FORE!"),
        new(ObjectiveType.MaxSwing, "FULL POWAH SWING", "I believe I can fly!"),
        new(ObjectiveType.Bowling, "Get a Stee-rike!", "I'm the Kingpin, nbd"),
        new(ObjectiveType.StickyPizza, "Pizza time!", "Mm, smacks of pepperoni, oregano, and a touch of sentient urethane."),
        new(ObjectiveType.PressButton, "Seekest Thou Yon Magical Button of Ywondere that probably* won't hurt you", "Huzzah!"),
        new(ObjectiveType.ClimbMountain, "Summit the peak", "Kinna pequeño, peak."),
        new(ObjectiveType.Dominoes, "CH-CH-CHAAAAIN REACTION!", "Bahm, bahm, bahm. Another one bites the dust."),
        new(ObjectiveType.Donut, "Where do Munchkins come from?", "GYATT I love that sound."),
        new(ObjectiveType.ObstacleCourse, "Towertop Trapese Trophy", "PARKOUR"),
        new(ObjectiveType.FireHydrant, "Become the Metropolitan Water Authority fire hydrant tester", "They work just fine.", 7),
        new(ObjectiveType.RightClickToBrake, "Cool it there, hotshot. Right-click to slow ya moves!", "Shawty got slow, slow, slow"),
        new(ObjectiveType.HitCar, "Do Golf Balls Dream of Electric SUVs?", "THE FUZZ"),
        new(ObjectiveType.EnterHole, "THIS IS MY HOLE. IT WAS MADE FOR ME.", "Ohp, maybe not."),
        new(ObjectiveType.PumpkinForest, "We're off to see the wizard, the wonderful wizard of the pumpkin forest", "Ah, I've been expecting me. You meet again."),
        new(ObjectiveType.DumpsterFire, "Remember kids, Always Set Dumpsters On Fire", "fudge, forgot my marshmallows"),
        new(ObjectiveType.Orangatang, "Find the benevolent overlord", "he prolly eats like so many mangoes"),
        new(ObjectiveType.MovieCamera, "Lights, Camera...!", "AACK"),
        new(ObjectiveType.VR, "Ride the ~waave~ of the future", "I can see two of my houses from here!"),
        new(ObjectiveType.Spring, "Spring will sprung", "it rides up a little"),
        new(ObjectiveType.Slowmo, "Goin slow", "mo"),
    };
    
    public Objective currentObjective { get; private set; }
    private TMPro.TextMeshProUGUI textComponent;
    private Transform completionMarker;
    private AudioManager audioManager;
    private Coroutine nextObjectiveCoroutine;
    private Coroutine ttsCoroutine;

    public UnityEvent<ObjectiveType> onObjectiveCompleted = new();

    void Start()
    {
        audioManager = AudioManager.Instance();
        currentObjective = GetNextObjective();
        textComponent = GetComponent<TMPro.TextMeshProUGUI>();
        completionMarker = transform.Find("CompletionMark");
        onObjectiveCompleted.AddListener((ot) =>
        {
            var objective = GetObjective(ot);
            SetText(objective);
            SetCompletionText(objective.CompletionPlayerText);
            audioManager.PlayAudioClip("objective completed");

            if (ttsCoroutine != null)
            {
                StopCoroutine(ttsCoroutine);
            }

            var ttsSoundName = "objective-" + objective.Type.ToString();
            ttsCoroutine = StartCoroutine(PlaySoundDelayed(ttsSoundName, 1.2f));
            
            if (!completionMarker.gameObject.activeSelf)
            {
                // Show that the objective was complete and set a timer for when the next objective will display
                completionMarker.gameObject.SetActive(true);
            }
            
            nextObjectiveCoroutine = StartCoroutine(DelayNextObjective(5));
        });

        SetText(currentObjective);
    }

    private void SetCompletionText(string objectiveCompletionPlayerText)
    {
        SpeechBubble.Instance().SetText(objectiveCompletionPlayerText);
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

    private IEnumerator PlaySoundDelayed(string clipName, float delay)
    {
        yield return new WaitForSeconds(delay);

        audioManager.PlayAudioClip(clipName);
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
            audioManager.PlayAudioClip("objective - all completed");
        }

        if (textComponent != null)
        {
            textComponent.text = newText;
        }

        if (nextObjectiveCoroutine != null)
        {
            StopCoroutine(nextObjectiveCoroutine);
        }
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
            // Update the text when the quantity of the current objective is updated
            var controller = ObjectiveController.Instance();
            if (controller.currentObjective == this)
            {
                controller.SetText(this);
            }
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
