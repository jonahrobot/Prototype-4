using Unity.VisualScripting;
using UnityEngine;

public class PlaySound : MonoBehaviour
{
    public AudioClip first;
    public AudioClip second;

    private AudioClip current;

    private void Start()
    {
        current = first;
    }

    public void playSound()
    {
        AudioSource source = this.AddComponent<AudioSource>();

        source.clip = current;
        source.Play();

        current = current == first ? second : first;
    }
}
