using System.Collections;

using System.Collections.Generic;

using UnityEngine;

using System.IO;

using LitJson;

using UnityEngine.UI;

public class ReadJson : MonoBehaviour {


    public static ReadJson instance;

  //  public  Ntext ntext;

    private JsonData itemDate;

    private string jsonString;


    public void Start()
    {
        StartCoroutine(initialization());
    }

    public IEnumerator initialization() {
        if (instance == null)
        {

            instance = this;

        }

     yield return   StartCoroutine(readJson());
    }

    IEnumerator readJson() {
        string spath = Application.streamingAssetsPath + "/information.json";

        Debug.Log(spath);

        WWW www = new WWW(spath);

        yield return www;

        jsonString = System.Text.Encoding.UTF8.GetString(www.bytes);

        JsonMapper.ToObject(www.text);

       itemDate = JsonMapper.ToObject(jsonString.ToString());




        for (int i = 0; i < itemDate["information"]["video"].Count; i++)

        {
            JsonData videoData = itemDate["information"]["video"];
            string udp = videoData[i]["udp"].ToString();
            string url = videoData[i]["url"].ToString();
            bool isLoop =int.Parse( videoData[i]["isloop"].ToString())==1?true:false;
            bool isScreenProtect = int.Parse(videoData[i]["isScreenProtect"].ToString()) == 1 ? true : false;

            Videoinfo videoinfo = new Videoinfo(udp, url, isLoop, isScreenProtect);

            ValueSheet.udp_VideoinfoPairs.Add(udp, videoinfo);
        }


        ValueSheet.volumeup = itemDate["information"]["volumeup"].ToString();

        ValueSheet.volumedown = itemDate["information"]["volumedown"].ToString();

        EventCenter.Broadcast(EventDefine.ini);
    }

}
