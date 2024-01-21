using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;

// TODO
// 全体的に: エラーハンドリングが適切にできてない: Responseクラスを定義しておくべきだったかも
// よく使う部分を共通化できそう

// https://gist.github.com/krzys-h/9062552e33dd7bd7fe4a6c12db109a1a
public class UnityWebRequestAwaiter : INotifyCompletion
{
    private UnityWebRequestAsyncOperation asyncOp;
    private Action continuation;

    public UnityWebRequestAwaiter(UnityWebRequestAsyncOperation asyncOp)
    {
        this.asyncOp = asyncOp;
        asyncOp.completed += OnRequestCompleted;
    }

    public bool IsCompleted { get { return asyncOp.isDone; } }

    public void GetResult() { }

    public void OnCompleted(Action continuation)
    {
        this.continuation = continuation;
    }

    private void OnRequestCompleted(AsyncOperation obj)
    {
        continuation();
    }
}

public static class ExtensionMethods
{
    public static UnityWebRequestAwaiter GetAwaiter(this UnityWebRequestAsyncOperation asyncOp)
    {
        return new UnityWebRequestAwaiter(asyncOp);
    }
}

public static partial class APIHandler
{
    public static string APIurl = "https://ai.trap.games/api/v1";
    public static async Task<GetKemonoResponse> GetKemonoByString(string uuid)
    {
        UnityWebRequest www = UnityWebRequest.Get(APIurl + "/kemonos/" + uuid);
        await www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(www.error + www.downloadHandler.text);
            return null;
        }
        else
        {
            GetKemonoResponse res = JsonConvert.DeserializeObject<GetKemonoResponse>(www.downloadHandler.text);
            return res;
        }
    }

    public static async Task<GetKemonoResponse> GetKemonoByGuid(Guid guid)
    {
        return await GetKemonoByString(guid.ToString());
    }

    public static async Task<GetKemonoResponse[]> GetKemonosByFieldId(int fieldId)
    {
        UnityWebRequest www = UnityWebRequest.Get(APIurl + "/fields/" + fieldId + "/kemonos");
        await www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(www.error + www.downloadHandler.text);
            return null;
        }
        else
        {
            GetKemonoResponse[] res = JsonConvert.DeserializeObject<GetKemonoResponse[]>(www.downloadHandler.text);
            return res;
        }
    }
    
    public static async Task<PostBattleResponse> PostBattle(Guid myKemonoId, Guid enemyKemonoId)
    {
        var data = new Dictionary<string, string>
        {
            {"my_kemono_id", myKemonoId.ToString()},
            {"enemy_kemono_id", enemyKemonoId.ToString()}
        };

        UnityWebRequest www = UnityWebRequest.Post(APIurl + "/battles", data);
        await www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(www.error + www.downloadHandler.text);
            return null;
        }
        else
        {
            PostBattleResponse res = JsonConvert.DeserializeObject<PostBattleResponse>(www.downloadHandler.text);
            return res;
        }
    }
    
    public static async Task<PostBattleIdBattleIdResponse> PostBattleIdWithDamage(int damage)
    {
        var data = new Dictionary<string, string>
        {
            {"damage", damage.ToString()}
        };

        UnityWebRequest www = UnityWebRequest.Post(APIurl + "/battles/" + DM.BattleId, data);
        await www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(www.error + www.downloadHandler.text);
            return null;
        }
        else
        {
            try
            {
                PostBattleIdBattleIdResponse res =
                    JsonConvert.DeserializeObject<PostBattleIdBattleIdResponse>(www.downloadHandler.text);
                return res;
            }
            catch (Exception e)
            {
                Debug.Log($"{e.Message} json:{www.downloadHandler.text}");
                return null;
            }
        }
    }
    
    public static async Task<bool> IsTruePostKemonosCatch()
    {
        var data = new Dictionary<string, string>
        {
            {"player_id", DM.UserGuid.ToString()}
        };

        UnityWebRequest www = UnityWebRequest.Post(APIurl + "/kemonos/" + DM.EnemyKemonoId + "/catch", data);
        await www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(www.error + www.downloadHandler.text);
            return false;
        }
        else if (www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(www.error + www.downloadHandler.text);
            return false;
        }
        else
        {
            return true;
        }
    }
    
    public static async Task<GetKemonoResponse[]> GetKemonos()
    {
        // ログイン機能が実装できたらこれを用いること
        UnityWebRequest www = UnityWebRequest.Get(APIurl + "/users/" + DM.UserGuid + "/kemonos");
        await www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(www.error + www.downloadHandler.text);
            return null;
        }
        else if (www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(www.error + www.downloadHandler.text);
            return null;
        }
        else
        {
            GetKemonoResponse[] res = JsonConvert.DeserializeObject<GetKemonoResponse[]>(www.downloadHandler.text);
            return res;
        }
    }
    
    public static async Task PostKemonoIdExtract()
    {
        var data = new Dictionary<string, string>
        {
            {"player_id", DM.UserGuid.ToString()}
        };
        
        // ログイン機能が実装できたらこれを用いること
        UnityWebRequest www = UnityWebRequest.Post(APIurl + "/kemonos/" + DM.EnemyKemonoId + "/extract", data);
        await www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(www.error + www.downloadHandler.text);
        }
        else if (www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(www.error + www.downloadHandler.text);
        }
    }
    
    public static async Task<GetKemonoConceptsResponse[]> GetKemonoConcepts()
    {
        // TODO
        UnityWebRequest www = UnityWebRequest.Get(APIurl + "/users/" + DM.UserGuid + "/concepts");
        // UnityWebRequest www = UnityWebRequest.Get(APIurl + "/users/" + "00000000-0000-0000-0000-000000000000" + "/concepts");
        await www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(www.error + www.downloadHandler.text);
            return null;
        }
        else if (www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(www.error + www.downloadHandler.text);
            return null;
        }
        else
        {
            GetKemonoConceptsResponse[] res = JsonConvert.DeserializeObject<GetKemonoConceptsResponse[]>(www.downloadHandler.text);
            return res;
        }
    }
    
    public static async Task<GetKemonoResponse> PostKemonoGenerate(string conceptsString)
    {
        var data = new Dictionary<string, string>
        {
            {"concepts", conceptsString},
            {"user_id", DM.UserGuid.ToString()}
        };
        // ログイン機能が実装できたらこれを用いること
        UnityWebRequest www = UnityWebRequest.Post(APIurl + "/kemonos/generate", data);
        // UnityWebRequest www = UnityWebRequest.Get(APIurl + "/kemonos/");
        await www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(www.error + www.downloadHandler.text);
            return null;
        }
        else if (www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(www.error + www.downloadHandler.text);
            return null;
        }
        else
        {
            GetKemonoResponse res = JsonConvert.DeserializeObject<GetKemonoResponse>(www.downloadHandler.text);
            return res;
        }
    }
    
    public static async Task<Err> DeleteKemonoConcept(Guid conceptString)
    {
        // ログイン機能が実装できたらこれを用いること
        UnityWebRequest www = UnityWebRequest.Delete(APIurl + "/concepts/" + conceptString);
        // UnityWebRequest www = UnityWebRequest.Get(APIurl + "/kemonos/");
        await www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(www.error + www.downloadHandler.text);
            return new Err(www.error + www.downloadHandler.text);
        }
        else if (www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(www.error + www.downloadHandler.text);
            return new Err(www.error + www.downloadHandler.text);
        }
        else
        {
            return null;
        }
    }
    
    public static async Task<GetKemonoResponse> GetUsersKemonosMe()
    {
        UnityWebRequest www = UnityWebRequest.Get(APIurl + "/users/" + DM.UserGuid + "/kemonos/me");
        // UnityWebRequest www = UnityWebRequest.Get(APIurl + "/users/" + "00000000-0000-0000-0000-000000000000" + "/concepts");
        await www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(www.error + www.downloadHandler.text);
            return null;
        }
        else if (www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(www.error + www.downloadHandler.text);
            return null;
        }
        else
        {
            GetKemonoResponse res = JsonConvert.DeserializeObject<GetKemonoResponse>(www.downloadHandler.text);
            return res;
        }
    }
    
    public static async Task<GetUserResponse> GetUsers(Guid user)
    {
        UnityWebRequest www = UnityWebRequest.Get(APIurl + "/users/" + user);
        // UnityWebRequest www = UnityWebRequest.Get(APIurl + "/users/" + "00000000-0000-0000-0000-000000000000" + "/concepts");
        await www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(www.error + www.downloadHandler.text);
            return null;
        }
        else if (www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(www.error + www.downloadHandler.text);
            return null;
        }
        else
        {
            GetUserResponse res = JsonConvert.DeserializeObject<GetUserResponse>(www.downloadHandler.text);
            return res;
        }
    }
    
    /*
    public static async Task<(Err, GetUserResponse)> PostUsers(string username)
    {
        var data = new Dictionary<string, string>
        {
            {"name", username},
        };
        
        UnityWebRequest www = UnityWebRequest.Post(APIurl + "/users", data);
        // UnityWebRequest www = UnityWebRequest.Get(APIurl + "/users/" + "00000000-0000-0000-0000-000000000000" + "/concepts");
        await www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(www.error + www.downloadHandler.text);
            return (new Err(www.error + www.downloadHandler.text), null);
        }
        else if (www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(www.error + www.downloadHandler.text);
            return (new Err(www.error + www.downloadHandler.text), null);
        }
        else
        {
            GetUserResponse res = JsonConvert.DeserializeObject<GetUserResponse>(www.downloadHandler.text);
            return (null, res);
        }
    }
    */
    
    public static async Task<(Err, GetKemonoResponse)> PostKemonosBreed()
    {
        var data = new Dictionary<string, string>
        {
            {"kemono1_id", DM.SelectedKemono.Id.ToString()},
            {"kemono2_id", DM.SelectedKemono2.Id.ToString()},
        };
        
        UnityWebRequest www = UnityWebRequest.Post(APIurl + "/kemonos/breed", data);
        // UnityWebRequest www = UnityWebRequest.Get(APIurl + "/users/" + "00000000-0000-0000-0000-000000000000" + "/concepts");
        await www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(www.error + www.downloadHandler.text);
            return (new Err(www.error + www.downloadHandler.text), null);
        }
        else if (www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(www.error + www.downloadHandler.text);
            return (new Err(www.error + www.downloadHandler.text), null);
        }
        else
        {
            GetKemonoResponse res = JsonConvert.DeserializeObject<GetKemonoResponse>(www.downloadHandler.text);
            return (null, res);
        }
    }
    
    public static async Task<Err> PostKemonosBattle(Guid kemonoId)
    {
        var data = new Dictionary<string, string>
        {
            {"kemono_id", kemonoId.ToString()}
        };
        
        UnityWebRequest www = UnityWebRequest.Post(APIurl + "/users/" +DM.UserGuid+"/kemonos/battle", data);
        await www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(www.error + www.downloadHandler.text);
            return new Err(www.error + www.downloadHandler.text);
        }
        else if (www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(www.error + www.downloadHandler.text);
            return new Err(www.error + www.downloadHandler.text);
        }
        else
        {
            return null;
        }
    }
    
    public static async Task<(Err, GetKemonoResponse)> GetKemonosBattle()
    {
        UnityWebRequest www = UnityWebRequest.Get(APIurl + "/users/" +DM.UserGuid+"/kemonos/battle");
        await www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(www.error + www.downloadHandler.text);
            return (new Err(www.error + www.downloadHandler.text), null);
        }
        else if (www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(www.error + www.downloadHandler.text);
            return (new Err(www.error + www.downloadHandler.text), null);
        }
        else
        {
            GetKemonoResponse res = JsonConvert.DeserializeObject<GetKemonoResponse>(www.downloadHandler.text);
            return (null, res);
        }
    }
    
    public static async Task<(Err, PostUserIdResponse)> PostUserSignUp(string name, string password)
    {
        var data = new Dictionary<string, string>
        {
            {"name", name},
            {"password", password}
        };
        
        UnityWebRequest www = UnityWebRequest.Post(APIurl + "/users/signup", data);
        await www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(www.error + www.downloadHandler.text);
            return (new Err(www.error + www.downloadHandler.text), null);
        }
        else if (www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(www.error + www.downloadHandler.text);
            return (new Err(www.error + www.downloadHandler.text), null);
        }
        else
        {
            PostUserIdResponse res = JsonConvert.DeserializeObject<PostUserIdResponse>(www.downloadHandler.text);
            return (null, res);
        }
    }
    
    public static async Task<(Err, PostUserIdResponse)> PostUserLogin(string name, string password)
    {
        var data = new Dictionary<string, string>
        {
            {"name", name},
            {"password", password}
        };
        
        UnityWebRequest www = UnityWebRequest.Post(APIurl + "/users/login", data);
        await www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(www.error + www.downloadHandler.text);
            return (new Err(www.error + www.downloadHandler.text), null);
        }
        else if (www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(www.error + www.downloadHandler.text);
            return (new Err(www.error + www.downloadHandler.text), null);
        }
        else
        {
            PostUserIdResponse res = JsonConvert.DeserializeObject<PostUserIdResponse>(www.downloadHandler.text);
            return (null, res);
        }
    }
    
    public static async Task<Err> PostUserKemonosPosition(int fieldId, int x, int y)
    {
        var data = new Dictionary<string, string>
        {
            {"field_id", fieldId.ToString()},
            {"x", x.ToString()},
            {"y", y.ToString()}
        };
        
        UnityWebRequest www = UnityWebRequest.Post(APIurl + "/users/"+DM.UserGuid+"/kemonos/position", data);
        await www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(www.error + www.downloadHandler.text);
            return (new Err(www.error + www.downloadHandler.text));
        }
        else if (www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(www.error + www.downloadHandler.text);
            return (new Err(www.error + www.downloadHandler.text));
        }
        else
        {
            return null;
        }
    }
}
