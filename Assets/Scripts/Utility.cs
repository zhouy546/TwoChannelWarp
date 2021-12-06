using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace MyUtility {

    public static class Utility
    {
        public static float Maping(float value, float inputMin, float inputMax, float outputMin, float outputMax, bool clamp)
        {
            float outVal = ((value - inputMin) / (inputMax - inputMin) * (outputMax - outputMin) + outputMin);

            if (clamp)
            {
                if (outputMax < outputMin)
                {
                    if (outVal < outputMax) outVal = outputMax;
                    else if (outVal > outputMin) outVal = outputMin;
                }
                else
                {
                    if (outVal > outputMax) outVal = outputMax;
                    else if (outVal < outputMin) outVal = outputMin;
                }
            }


            return outVal;
        }


        //    async void Start()
        //{
        //    _texture = await Utility.GetRemoteTexture(_imageUrl);
        //    sprite = Sprite.Create(_texture, new Rect(0, 0, _texture.width, _texture.height), new Vector2(0.5f, 0.5f));

        //}
        /// <summary>
        /// 使用次方法前 需要在Start前加async    async void ini()
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static async Task<Texture2D> GetRemoteTexture(string url)
        {
            using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(url))
            {
                // begin request:
                var asyncOp = www.SendWebRequest();

                // await until it's done: 
                while (asyncOp.isDone == false)
                    await Task.Delay(1000 / 30);//30 hertz

                // read results:
                if (www.isNetworkError || www.isHttpError)
                {
                    // log error:
#if DEBUG
                Debug.Log($"{www.error}, URL:{www.url}");
#endif

                    // nothing to return on error:
                    return null;
                }
                else
                {
                    // return valid results:
                    return DownloadHandlerTexture.GetContent(www);
                }
            }
        }
    }
}

