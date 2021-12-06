using RenderHeads.Media.AVProVideo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MediaCtr : MonoBehaviour
{
    public static MediaCtr instance;

    MediaPlayer mediaplayer;

    public GameObject DebugUI;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        mediaplayer = this.GetComponent<MediaPlayer>();
        EventCenter.AddListener(EventDefine.ini, Ini);
        mediaplayer.Events.AddListener(MediaEvent);

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            DebugUI.SetActive(!DebugUI.active);
        }
    }

    public void Ini()
    {
        string screenProtectKey = getScreenProtectUDP();
        if (screenProtectKey != "null")
        {
            LoadVideo(ValueSheet.udp_VideoinfoPairs[screenProtectKey]);
        }
    }

    private string getScreenProtectUDP()
    {
        foreach (var item in ValueSheet.udp_VideoinfoPairs)
        {
           if(item.Value.isScreenProtect)
            {
                return item.Key;
            }
        }
        return "null";
    }

    public void LoadVideo(Videoinfo videoinfo)
    {
        ValueSheet.currentOnPlayVideoInfo = videoinfo;

        mediaplayer.OpenVideoFromFile(MediaPlayer.FileLocation.RelativeToStreamingAssetsFolder, videoinfo.url, true);

        mediaplayer.Control.SetLooping(videoinfo.isloop);
    }

    public void VolumeDown()
    {
        float currentVolume = mediaplayer.m_Volume;

        float newVolume = Mathf.Clamp01(currentVolume -= 0.1f);

        mediaplayer.Control.SetVolume(newVolume);
    }

    public void VolumeUp()
    {
        float currentVolume = mediaplayer.m_Volume;

        float newVolume = Mathf.Clamp01(currentVolume += 0.1f);

        mediaplayer.Control.SetVolume(newVolume);
    }

    public void MediaEvent(MediaPlayer mediaplayer, MediaPlayerEvent.EventType eventType,ErrorCode errorCode)
    {
        if(eventType == MediaPlayerEvent.EventType.Started)
        {
            Debug.Log("开始播放");
        }
        if (eventType == MediaPlayerEvent.EventType.FinishedPlaying)
        {
            Debug.Log("停止播放");

            if (!ValueSheet.currentOnPlayVideoInfo.isloop&&!ValueSheet.currentOnPlayVideoInfo.isScreenProtect)
            {
                Debug.Log("回到屏保");
                string screenProtectKey = getScreenProtectUDP();
                if (screenProtectKey != "null")
                {
                    LoadVideo(ValueSheet.udp_VideoinfoPairs[screenProtectKey]);
                }
            }
        }
    }

}
