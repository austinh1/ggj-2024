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
        audioSource.PlayOneShot(audioClips.First(ac => ac.name == clipName));
    }
}
