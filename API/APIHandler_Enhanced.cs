using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

    public static partial class APIHandler
    {
        public static async Task<T> SendRequest<T>(string url, Dictionary<string, string> data = null)
        {
            UnityWebRequest www;

            if (data != null)
            {
                www = UnityWebRequest.Post(url, data);
            }
            else
            {
                www = UnityWebRequest.Get(url);
            }

            await www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(www.error + www.downloadHandler.text);
                return default;
            }
            else if (www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log(www.error + www.downloadHandler.text);
                return default;
            }
            else
            {
                try
                {
                    T response = JsonConvert.DeserializeObject<T>(www.downloadHandler.text);
                    return response;
                }
                catch (Exception e)
                {
                    Debug.Log(e.Message);
                    return default;
                }
            }
        }
        
        public static async Task<GetKemonoResponse[]> GetKemonosNoImage()
        {
            var res = await SendRequest<GetKemonoResponse[]>(APIurl + "/users/" + DM.UserGuid + "/kemonos/noimage",
                null);
            return res;
        }

        public static async Task<Texture2D> GetKemonoImage(Guid kemonoId)
        {
            if (DM.CollectedImageSprites.ContainsKey(kemonoId))
            {
                return DM.CollectedImageSprites[kemonoId];
            }
            
            UnityWebRequest www =
                UnityWebRequestTexture.GetTexture(APIurl + "/kemonos/" + kemonoId + "/image");

            await www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(www.error + www.downloadHandler.text);
                return default;
            }
            else if (www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log(www.error + www.downloadHandler.text);
                return default;
            }
            else
            {
                try
                {
                    Texture2D res = DownloadHandlerTexture.GetContent(www);
                    DM.CollectedImageSprites[kemonoId] = res;
                    return res;
                }
                catch (Exception e)
                {
                    Debug.Log(e.Message);
                    return default;
                }
            }
        }
    }
