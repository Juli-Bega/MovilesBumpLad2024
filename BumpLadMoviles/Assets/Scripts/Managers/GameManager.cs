using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SocialPlatforms;



public class GameManager : MonoBehaviour
{
  
    
    public static GameManager Instance { get; private set; }
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }
    
    public int money;
    public float highscore;
    public Sprite selectedCar;
    public List<int> selectableCars;
    public List<Sprite> allCars;
    public List<Shopper> shoppers;
    public GooglePlayHadeler googleHandler;


    async void Start()
    {
        money = PlayerPrefs.GetInt("money");
        for (int i = 0; i < 5; i++)
        {
            if (PlayerPrefs.HasKey("ownedCar" + i))
            selectableCars.Add(PlayerPrefs.GetInt("ownedCar" + i));
        }
        selectedCar = allCars[PlayerPrefs.GetInt("SelectedCar")];

        
    }
    public void UnlockAchivments(string name)
    {
        CheckAchievements(name, achievementStatus => 
        {
            if (!achievementStatus)
            {
                Social.ReportProgress(name, 100.0f, (bool success) =>
                {
                    // handle success or failure
                });
            }
            else { Debug.Log("Achievement ya desbloqueado."); }
        });


        {  /*await googleHandler.Init(() =>
        {
            UnlockAchivments("CgkIqKrH6swPEAIQAQ");
        });*/

            //GooglePlay.UnlockAchievement(name);
        }
    }

    public void CheckAchievements(string name, Action<bool> OnComplete)
    {
        Social.LoadAchievements(achievements => {
            if (achievements.Length > 0)
            {               
                string myAchievements = "My achievements:\n";
                foreach (IAchievement achievement in achievements)
                {
                    if(achievement.id == name) 
                    {
                        if (achievement.percentCompleted == 100.0f)
                        {
                           OnComplete.Invoke(true);
                        }
                        else
                        {
                            OnComplete.Invoke(false);
                        }
                    }
                }
                Debug.Log(myAchievements);
            }
            else
            {
                OnComplete.Invoke(false);
            }
        });

    }

}
