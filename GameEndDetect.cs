using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class GameEndDetect : MonoBehaviour
{
    public static int finishCount = 0;
    public static int failCount = 0;

    public static bool gameEnd = false;

    // public GameObject deathCount;
    [SerializeField] private TMP_Text deathCountText;

    [SerializeField] private TMP_Text output; // Reference to the Canvas GameObject

    // Update is called once per frame
    void Update()
    {
        deathCountText.text = "死亡次數: " + failCount;

        if (finishCount == 3||(finishCount == 2&&failCount==0))
        {
            gameEnd = true;
            // output.text = "遊戲結束!你獲勝了!";
            Debug.Log("Game Over! You Win!");
            // UnityEditor.EditorApplication.isPlaying = false;
            // Application.Quit();
        }
        else if (failCount > 1)
        {
            gameEnd = true;
            // output.text = "遊戲結束!你輸了!";
            Debug.Log("Game Over! You Lose!");
            // UnityEditor.EditorApplication.isPlaying = false;
            // Application.Quit();
        }
    }
}
