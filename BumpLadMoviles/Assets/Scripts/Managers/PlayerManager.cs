using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;

public class PlayerManager : MonoBehaviour
{
    public SpriteRenderer playerCar;
    public int money;
    public Vector3 startPos;
    private GameManager GM;
    private GameUI GUI;
    public GameplayManager GameplayManager;

    private void Awake()
    {
        GM = FindObjectOfType<GameManager>();
        GUI = FindObjectOfType<GameUI>();
    }
    void Start()
    {
        this.transform.position = startPos;
        playerCar.sprite = GM.selectedCar;
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("Car"))
        {
            Debug.Log("ChoquePlayer-Auto");
            GUI.Defeat();
            GM.UnlockAchivments("CgkIqKrH6swPEAIQAw");
            Social.ReportScore (GameplayManager.kmTraveled, "CgkIqKrH6swPEAIQBA", (bool success) =>
            {
                if (success) { Debug.Log("Nuevo score"); }
                else { Debug.Log("Error en la carga del score");}

            });
            PlayGamesPlatform.Instance.ReportScore(GameplayManager.kmTraveled, "CgkIqKrH6swPEAIQBA", "Score", (bool success) => {
                if (success) { Debug.Log("Nuevo score"); }
                else { Debug.Log("Error en la carga del score"); }
            });
        }

        
    }



}
