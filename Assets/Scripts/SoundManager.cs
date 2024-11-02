using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    //사운드 추가할때 명칭 넣어줘야함
    public enum BGM
    {
       BattleLoop = 0,
    }

    public enum SFX
    {
        BrickThrow = 0,
    }

    [Header("BGM")]
    [Range(0, 1)] [SerializeField] float bgm_MasterVolume = 0.5f;
    [SerializeField] AudioClip[] audioClips_BGM;


    [Header("SFX")]
    [Range(0, 1)] [SerializeField] float sfx_MasterVolume = 0.5f;
    [SerializeField] AudioClip[] audioClips_SFX;


    List<AudioSource> bgm_AudioChannelList = new List<AudioSource>();
    List<AudioSource> sfx_AudioChannelList = new List<AudioSource>();

    const int inital_BGM_ChannelCount = 15;
    const int inital_SFX_ChannelCount = 15;

    private void Awake()
    {
        Init_Channels();
        PlayBGM(BGM.BattleLoop, true);
    }

    public void Init_Channels()
    {
        GameObject bgm_AudioSourceObj = new GameObject();
        bgm_AudioSourceObj.name = "BGM_AudioChannels";
        bgm_AudioSourceObj.transform.SetParent(transform);
        for (int i = 0; i < inital_BGM_ChannelCount; i++)
        {
            AudioSource tempAudioSource = bgm_AudioSourceObj.AddComponent<AudioSource>();
            bgm_AudioChannelList.Add(tempAudioSource);
        }

        GameObject sfx_AudioSourceObj = new GameObject();
        sfx_AudioSourceObj.name = "SFX_AudioChannels";
        sfx_AudioSourceObj.transform.SetParent(transform);
        for (int i = 0; i < inital_SFX_ChannelCount; i++)
        {
            AudioSource tempAudioSource = sfx_AudioSourceObj.AddComponent<AudioSource>();
            sfx_AudioChannelList.Add(tempAudioSource);
        }
    }
    public AudioSource PlayBGM(BGM _bgmName, bool _isLoop)
    {
        int soundIndex = (int)_bgmName;

        AudioSource tempAudioSource = GetEmptyAudioChannel_BGM();

        tempAudioSource.clip = audioClips_BGM[soundIndex];
        tempAudioSource.loop = _isLoop;
        tempAudioSource.volume = bgm_MasterVolume;
        tempAudioSource.Play();

        return tempAudioSource;
    }

    //플레이할때 loop해서 사용하는 경우 오디오 소스 받아와서 수동으로 꺼줄것
    public AudioSource PlaySFX(SFX _sfxName, bool _isLoop)
    {
        int soundIndex = (int)_sfxName;

        AudioSource tempAudioSource = GetEmptyAudioChannel_SFX();

        tempAudioSource.clip = audioClips_SFX[soundIndex];
        tempAudioSource.loop = _isLoop;
        tempAudioSource.volume = sfx_MasterVolume;
        tempAudioSource.Play();

        return tempAudioSource;
    }

    AudioSource GetEmptyAudioChannel_BGM()
    {
        AudioSource tempAudioSource = null;

        for (int i = 0; i < bgm_AudioChannelList.Count; i++)
        {
            if (bgm_AudioChannelList[i].isPlaying == false)
            {
                tempAudioSource = bgm_AudioChannelList[i];
                break;
            }
        }

        if (tempAudioSource == null)
        {
            tempAudioSource = gameObject.AddComponent<AudioSource>();
            bgm_AudioChannelList.Add(tempAudioSource);
        }

        return tempAudioSource;
    }
    AudioSource GetEmptyAudioChannel_SFX()
    {
        AudioSource tempAudioSource = null;

        for (int i = 0; i < sfx_AudioChannelList.Count; i++)
        {
            if (sfx_AudioChannelList[i].isPlaying == false)
            {
                tempAudioSource = sfx_AudioChannelList[i];
                break;
            }
        }

        if (tempAudioSource == null)
        {
            tempAudioSource = gameObject.AddComponent<AudioSource>();
            sfx_AudioChannelList.Add(tempAudioSource);
        }

        return tempAudioSource;
    }
}
