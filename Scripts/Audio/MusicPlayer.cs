using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using System.IO;
using Unity.VisualScripting;

public class MusicPlayer : MonoBehaviour
{
    public VideoPlayer videoPlayer;

    FMOD.ChannelGroup musicChannelGroup; // 음악 재생용 FMOD
    FMOD.Sound music;
    FMOD.Channel musicChannel;

    FMOD.ChannelGroup sfxChannelGroup; // 효과음 재생용 FMOD
    FMOD.Sound hitSfx;
    FMOD.Channel hitSfxChannel;

    static MusicPlayer instance;
    public static MusicPlayer Instance { get { return instance; } }

    string musicFileName;
    string audioType;
    string sfxFileName;
    string sfxType;

    public enum State
    {
        Playing,
        Paused,
        Unpaused,
        Stop,
    }
    public State state = State.Stop;

    public uint Length // 단위 ms
    {
        get
        {
            uint len = 1;
            if ( !musicChannel.IsUnityNull() )
                music.getLength(out len, FMOD.TIMEUNIT.MS);
            return len;
        }
    }

    public uint progressTime // 단위 ms
    {
        get
        {
            uint time = 1;
            if ( !musicChannel.IsUnityNull() )
                musicChannel.getPosition(out time, FMOD.TIMEUNIT.MS);
            return time;
        }
        set
        {
            if ( !musicChannel.IsUnityNull() )
                musicChannel.setPosition(value, FMOD.TIMEUNIT.MS);
            if (videoPlayer.isPrepared)
                videoPlayer.time = value / 1000.0;
        }
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(instance.gameObject);
            instance = this;
        }

        videoPlayer = GetComponent<VideoPlayer>();
    }

    public void Init()
    {
        videoPlayer.audioOutputMode = VideoAudioOutputMode.None;
        videoPlayer.source = VideoSource.Url;

        musicChannelGroup = new FMOD.ChannelGroup();
        musicChannel = new FMOD.Channel();
        music = new FMOD.Sound();
        musicChannel.setChannelGroup(musicChannelGroup);

        sfxChannelGroup = new FMOD.ChannelGroup();
        hitSfxChannel = new FMOD.Channel();
        hitSfx = new FMOD.Sound();
        hitSfxChannel.setChannelGroup(sfxChannelGroup);
    }

    public void Insert (string musicName, string hitSound)
    {
        // videoPlayer.audioOutputMode = VideoAudioOutputMode.None;

        // videoPlayer.source = VideoSource.Url;
        videoPlayer.url = MusicListManager.Instance.sheets[musicName].clip;


        // musicChannelGroup = new FMOD.ChannelGroup();
        // musicChannel = new FMOD.Channel();
        // music = new FMOD.Sound();

        musicFileName = musicName;
        audioType = "mp3";

        FMODUnity.RuntimeManager.CoreSystem.createSound(Path.Combine(Application.dataPath, "Sheet", musicFileName, musicFileName+"."+audioType), FMOD.MODE.CREATESAMPLE, out music);

        // sfxChannelGroup = new FMOD.ChannelGroup();
        // sfxChannel = new FMOD.Channel();
        // sfx = new FMOD.Sound();

        sfxFileName = hitSound;
        sfxType = "wav";

        FMODUnity.RuntimeManager.CoreSystem.createSound(Path.Combine(Application.dataPath, "Sounds", "SFX", "HitSounds", sfxFileName+"."+ sfxType), FMOD.MODE.CREATESAMPLE, out hitSfx);
    }

    public void InsertBGM(string musicName)
    {
        ReleaseMusic();

        musicFileName = musicName;
        audioType = "mp3";
        FMODUnity.RuntimeManager.CoreSystem.createSound(Path.Combine(Application.dataPath, "Sounds", "BGM", musicFileName, musicFileName+"."+audioType), FMOD.MODE.CREATESAMPLE, out music);
        FMODUnity.RuntimeManager.CoreSystem.playSound(music, musicChannelGroup, false, out musicChannel);
        music.setMode(FMOD.MODE.LOOP_NORMAL);
        musicChannel.setMode(FMOD.MODE.LOOP_NORMAL);

        UpdateMusicVolume();
    }

    public void InsertSyncSetting(string hitSound)
    {
        ReleaseMusic();

        videoPlayer.url = SyncSheetParser.Instance.sheet.clip;

        musicFileName = "SyncSheet";
        audioType = "mp3";
        
        FMODUnity.RuntimeManager.CoreSystem.createSound(Path.Combine(Application.dataPath, "Resources", musicFileName, musicFileName+"."+audioType), FMOD.MODE.CREATESAMPLE, out music);

        sfxFileName = hitSound;
        sfxType = "wav";

        FMODUnity.RuntimeManager.CoreSystem.createSound(Path.Combine(Application.dataPath, "Resources", sfxFileName+"."+ sfxType), FMOD.MODE.CREATESAMPLE, out hitSfx);
    }

    public void PlayPreview(string musicName)
    {
        ReleaseMusic();
        videoPlayer.isLooping = true;

        audioType = "mp3";
        FMODUnity.RuntimeManager.CoreSystem.createSound(Path.Combine(Application.dataPath, "Sheet", musicName, musicName+" preview."+audioType), FMOD.MODE.CREATESAMPLE, out music);
        videoPlayer.url = $"{Application.dataPath}/Sheet/{musicName}/{musicName} preview.mp4";

        videoPlayer.Play();
        FMODUnity.RuntimeManager.CoreSystem.playSound(music, musicChannelGroup, false, out musicChannel);
        music.setMode(FMOD.MODE.LOOP_NORMAL);
        musicChannel.setMode(FMOD.MODE.LOOP_NORMAL);

        UpdateMusicVolume();
    }

    public void Play()
    {
        state = State.Playing;

        FMODUnity.RuntimeManager.CoreSystem.playSound(music, musicChannelGroup, false, out musicChannel);
        // musicChannel.setPaused(true);
        UpdateMusicVolume();
        // musicChannel.setPaused(false);
        videoPlayer.Play();
    }

    public void Pause()
    {
        state = State.Paused;
        musicChannel.setPaused(true);
        videoPlayer.Pause();
    }

    public void UnPause()
    {
        state = State.Unpaused;
        musicChannel.setPaused(false);
        videoPlayer.Play();
    }

    public void Stop()
    {
        state = State.Stop;
        musicChannel.stop();
        videoPlayer.Stop();
    }

    public void ReleaseMusic()
    {
        music.setMode(FMOD.MODE.LOOP_OFF);
        musicChannel.setMode(FMOD.MODE.LOOP_OFF);
        videoPlayer.isLooping = false;

        videoPlayer.Stop();
        musicChannel.stop();
        music.release();
        hitSfx.release();
    }

    
    public void MovePosition(float time) // 스크롤 시 노래 위치 변경 (EditorController.cs 참고)
    {
        uint currentTime = 0;
        musicChannel.getPosition(out currentTime, FMOD.TIMEUNIT.MS);

        currentTime += (uint)time;
        currentTime = Math.Clamp(currentTime, 0, Length - 1);

        musicChannel.setPosition(currentTime, FMOD.TIMEUNIT.MS);
    }


    public float GetMilliSec()
    {
        musicChannel.getPosition(out uint pos, FMOD.TIMEUNIT.MS);
        return pos;
    }

    public bool IsPlaying()
    {
        musicChannel.isPlaying(out bool isPlaying);
        return isPlaying; // audioSource.isPlaying;
    }

    public void PlayKeySoundEffect()
    {
        hitSfxChannel.stop();
        FMODUnity.RuntimeManager.CoreSystem.playSound(hitSfx, sfxChannelGroup, false, out hitSfxChannel);
        UpdateMusicVolume();
        // hitSfxChannel.setPaused(true);
        // hitSfxChannel.setPaused(false);
    }

    public void SetMusicPitch(float pitch)
    {
        musicChannel.setPitch(pitch);
    }

    public void UpdateMusicVolume()
    {
        musicChannel.setVolume((PlayerPrefs.GetInt("BGMVolume") / 100.0f) * (PlayerPrefs.GetInt("MasterVolume") / 100.0f));
        hitSfxChannel.setVolume((PlayerPrefs.GetInt("HitSFXVolume") / 100.0f) * (PlayerPrefs.GetInt("MasterVolume") / 100.0f));
    }

    void OnDestroy()
    {
        ReleaseMusic();
    }
}
