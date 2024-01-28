using System.Collections;
using TMPro;
using UnityEngine;

public class SpeechBubble : MonoBehaviour
{
    public static SpeechBubble Instance()
    {
        var controllers = GameObject.FindGameObjectsWithTag("SpeechBubble");
        for (var i = 0; i < controllers.Length; i++)
        {
            var objectiveScript = controllers[i].GetComponent<SpeechBubble>();
            if (objectiveScript != null)
            {
                return objectiveScript;
            }
        }
        return null;
    }
    
    public Transform ball;
    public Transform cameraAnchor;
    public TMP_Text text;
    
    public void SetText(string t)
    {
        StopAllCoroutines();
        
        text.text = t;
        StartCoroutine(ClearTextAfterDelay(5f));
    }

    private IEnumerator ClearTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        text.text = string.Empty;
    }
    
    private void FixedUpdate()
    {
        transform.position = ball.transform.position;
        transform.eulerAngles = cameraAnchor.eulerAngles;
    }
}
