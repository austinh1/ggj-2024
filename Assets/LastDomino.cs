using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastDomino : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        var objective = ObjectiveController.Instance().GetObjective(ObjectiveType.Dominoes);
        if (objective.IsComplete)
            return;
        
        if (transform.eulerAngles.x > 30)
        {
            ObjectiveController.Instance().GetObjective(ObjectiveType.Dominoes).Increment();
        }       
    }
}
