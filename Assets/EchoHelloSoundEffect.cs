using UnityEngine;

public class EchoHelloSoundEffect : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            AudioManager.Instance().PlayAudioClip("voice-hello");
        }
    }
}
