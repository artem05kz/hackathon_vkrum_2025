using UnityEngine;

public class Sounds : MonoBehaviour
{
    public AudioClip[] sounds;

    private AudioSource audioSrc => GetComponent<AudioSource>();

    public void PlaySound(AudioClip clip, float volume = 1.0f, bool destroyed = false, float p1 = 1f)
    {
        audioSrc.pitch = p1;

        if (destroyed)
            AudioSource.PlayClipAtPoint(clip, transform.position);
        else
            audioSrc.PlayOneShot(clip, volume);

    }
    public void PlayMusic(AudioClip clip, float volume = 1.0f)
    {
        audioSrc.clip = clip;
        audioSrc.volume = volume;
        audioSrc.loop = true;
        audioSrc.Play();
    }
}
