using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;
using System;

public class FacebookManager : MonoBehaviour
{
    private string fbLogingState = string.Empty;

    private void Awake()
    {
        if (!FB.IsInitialized)
        {
            FB.Init(() =>
            {
                if (FB.IsInitialized)
                    FB.ActivateApp();
                else
                    Debug.LogError("Couldn't initialize");
            });
        }
        else
        {
            FB.ActivateApp();
        }
    }

    private void Update()
    {
        if (FB.IsInitialized)
        {
            if (FB.IsLoggedIn)
            {
                fbLogingState = "Loged in!";
            }
            else
            {
                fbLogingState = "Loged out";
            }
        }
    }

    public static void LoginFacebook(System.Action onLogginComplete)
    {
        var permissions = new List<string>() { "public_profile", "email", "user_friends" };
        FB.LogInWithReadPermissions(permissions, (result)=> {
            if (!result.Cancelled && result.Error == null)
            {
                onLogginComplete.Invoke();
                Debug.LogError("Login Good");
            }
            else { Debug.LogError("Login not Good"); }
        });
    }
    public void LogOut()
    {
        FB.LogOut();
    }


    public void Invite()
    {
        if (FB.IsLoggedIn)
        {
            // FB.ShareLink(new System.Uri(""), "");
            FB.AppRequest("Join me on BumpLad", null,null, null, null, null, " ", delegate (IAppRequestResult result)
            {
                Debug.Log(result.RawResult);
            });
        }
        else
        {
            LoginFacebook(() => {
                FB.AppRequest("Join me on BumpLad", null, null, null, null, null, " ", delegate (IAppRequestResult result)
                {
                    Debug.Log(result.RawResult);
                });
            }); 
        }
}
    public void FeedFacebook()
    {
        if (FB.IsLoggedIn)
        {
            FB.FeedShare("", null, "", "", null);
        }
        else
        {
            LoginFacebook(() => {
                FB.FeedShare("", null, "", "", null);
            });
        }
    }
    public void FacebookGameRequest()
    {
        FB.AppRequest("Come play with me this awesome game!");
    }

    public void LogOutOfFacebook() 
    {
        FB.LogOut();
    }   
    public string GetLogedState()
    {
        return fbLogingState;
    }
}