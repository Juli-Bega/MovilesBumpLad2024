using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using System;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;



public class GooglePlayHadeler : MonoBehaviour
{
    public string Token;
    public string Error;
    private string LeaderboardID = "CgkIqKrH6swPEAIQBA";

    void Awake()
    {
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
    }

    // Update is called once per frame
    public async Task Init(Action OnComplete)
    {
        await UnityServices.InitializeAsync();
        await LoginGooglePlayGames();
        await SignInWithGooglePlayGamesAsync(Token);
        OnComplete.Invoke();    
    }
    //Fetch the Token / Auth code
    public Task LoginGooglePlayGames(Action onComplete = null)  //RequestServerSideAccess
    {
        var tcs = new TaskCompletionSource<object>();
        PlayGamesPlatform.Instance.Authenticate((success) =>
        {
            if (success)
            {
                Debug.Log("Login with Google Play games successful.");
                Token = PlayGamesPlatform.Instance.GetIdToken();
                // This token serves as an example to be used for SignInWithGooglePlayGames
                tcs.SetResult(null);
                Debug.Log("Authorization code: " + Token);
                onComplete?.Invoke();
            }
            else
            {
                Error = "Failed to retrieve Google play games authorization code";
                Debug.Log("Login Unsuccessful");
                tcs.SetException(new Exception("Failed"));
            }
        });
        return tcs.Task;
    }
    public void ShowLeaderBoard()  //RequestServerSideAccess
    {
        if (Social.localUser.authenticated)
        {
            PlayGamesPlatform.Instance.ShowLeaderboardUI("CgkIqKrH6swPEAIQBA");
            Social.ShowLeaderboardUI();
        }
        else
        {
            LoginGooglePlayGames(() =>
            {
                PlayGamesPlatform.Instance.ShowLeaderboardUI("CgkIqKrH6swPEAIQBA");
                Social.ShowLeaderboardUI();
            });
        }
    }

    public void PostLeaderBoard(float score)  //RequestServerSideAccess
    {
        if(Social.localUser.authenticated) {
            Social.ReportScore((long)score, LeaderboardID, (bool success) =>
            {
                if (success)
                {
                    ((PlayGamesPlatform)Social.Active).ShowLeaderboardUI();
                }
                else
                {
                    Debug.Log("Add Score Fail");
                }
            });
        }
    }

    async Task SignInWithGooglePlayGamesAsync(string authCode)
    {
        try
        {
            await AuthenticationService.Instance.SignInWithGooglePlayGamesAsync(authCode);
            Debug.Log($"PlayerID: {AuthenticationService.Instance.PlayerId}"); //Display the Unity Authentication PlayerID
            Debug.Log("SignIn is successful.");
        }
        catch (AuthenticationException ex)
        {
            // Compare error code to AuthenticationErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
        catch (RequestFailedException ex)
        {
            // Compare error code to CommonErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
    }
}
