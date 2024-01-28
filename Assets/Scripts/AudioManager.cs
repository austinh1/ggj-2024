using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public List<AudioClip> audioClips;
    public AudioSource audioSource;

    public static AudioManager Instance()
    {
        var controllers = GameObject.FindGameObjectsWithTag("AudioManager");
        for (var i = 0; i < controllers.Length; i++)
        {
            var script = controllers[i].GetComponent<AudioManager>();
            if (script != null)
            {
                return script;
            }
        }
        return null;
    }

    public void PlayAudioClip(string clipName)
    {
        var clip = audioClips.First(ac => ac.name == clipName);
        if (clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogError(string.Format("No audio clip in AudioManager with the name: '{0}'", clipName));
        }
    }
}
