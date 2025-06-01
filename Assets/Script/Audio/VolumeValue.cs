using UnityEngine;

public class VolumeValue : MonoBehaviour 
{
    private AudioSource audioSrc;
    private float musicVolume = 1f;
    void Start() {
        audioSrc = GetComponent<AudioSource>();
    }
    void Update() {
        audioSrc.volume = musicVolume;
    }

    public void setVolume(float vol) {
        musicVolume = vol;
    }
}
