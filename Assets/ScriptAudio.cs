
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class ScriptAudio : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;
    [SerializeField]  Slider MusicSlider;
    [SerializeField] private Slider SoundEfccterSlider;
    const string MIXER_MUSIC = "MusicParam";
    const string MIXER_SFC = "SoundParam";
    private void Awake()
    {
        MusicSlider.onValueChanged.AddListener(SetMusicValue);
        SoundEfccterSlider.onValueChanged.AddListener(SetSoundValue);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void SetMusicValue(float value)
    {
        audioMixer.SetFloat(MIXER_MUSIC, Mathf.Log10(value)*20);
    }
    private void SetSoundValue(float value)
    {
        audioMixer.SetFloat(MIXER_SFC, Mathf.Log10(value) * 20);
    }
}
