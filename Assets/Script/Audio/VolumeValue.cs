using UnityEngine;

public class VolumeValue : MonoBehaviour 
{
    private AudioSource audioSrc;
    private float musicVolume = 0.1f;
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
