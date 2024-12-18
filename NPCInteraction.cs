using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NPCInteraction : MonoBehaviour
{
    // [Header("UI Elements")]
    // public TMP_Text displayText; // TextMeshPro UI element

    public string[] quiz = {"你對笑過敏", "你的煞車在陡峭的山坡上失靈了", "你年邁的鄰居一心想殺你", "如果你再次打噴嚏，你會死", "你突然發現自己無法停止奔跑", "你被困在一棟燃燒的大樓裡", "你被困在一艘正在下沉的船上", "你被困在一場龍捲風中", "你正從高空墜落", "你面對一隻憤怒的野生熊", "你在暴風雪中迷路了", "你被鎖在一輛正在下沉的車裡", "你正面臨一頭衝過來的犀牛", "你被一條毒蛇逼入了死角", "你被困在一個正在崩塌的懸崖上", "你被困在一座正在坍塌的礦坑裡", "你被鎖在一個有滴答作響炸彈的房間裡", "你正在抵擋一群殺人蜂的攻擊", "你從飛機掉了出去，而你的降落傘打不開", "你迷失在一片沒有水的沙漠裡", "一場雪崩正朝你襲來", "你被困在一個有飢餓獅子的房間裡", "地精王要求你娛樂他，否則就要你死", "你正被 500 隻小狗攻擊", "你踩到了地雷，如果抬起腳，它就會爆炸"};
    
    // public static int canPlayerMove = 1; // Accessible from anywhere

    public static Transform npcTransform; // Reference to the NPC's Transform

    public string npcDialogue = "Collision";

    public float rotationSpeed = 5f;     // Speed of rotation (for smooth turning)

    private Transform cameraTransform;   // Reference to the camera's Transform
    public static bool shouldRotate = false;   // To trigger rotation

    public static string getAIresponse;
    public static bool displayAIresponse = false;

    public GameObject canvasObject; // Reference to the Canvas GameObject
    [SerializeField] private TMP_Text output; // Reference to the Canvas GameObject

    private Collider opponentCollider;

    public static bool shouldDisplayCanvas = false;

    void Start()
    {
        // Get the camera's transform
        cameraTransform = Camera.main.transform; // Assuming the main camera
        // Get the Collider component attached to the opponent
        opponentCollider = GetComponent<Collider>();
    }

    void Update()
    {
        // print("shouldDisplayCanvas: " + shouldDisplayCanvas); // Log the condition

        // 靠近NPC顯示題目框
        if (shouldDisplayCanvas && !canvasObject.activeSelf)
        {
            ShowCanvas();
        }
        else if (!shouldDisplayCanvas && canvasObject.activeSelf)
        {
            HideCanvas();
        }

        // 當模型回覆答案時
        if (displayAIresponse) 
        {
            print("displayAIresponse: " + displayAIresponse); // Log the condition
            if (getAIresponse == "1")
            {
                output.text = "你存活了";
                GameEndDetect.finishCount += 1;
            }
            else if (getAIresponse == "0")
            {
                output.text = "你死了";
                GameEndDetect.finishCount += 1;
                GameEndDetect.failCount += 1;
            }
            else
            {
                output.text = "你的回答不合法";
            }
            displayAIresponse = false;
            // cancel no player movement
            CameraMovement.canMove = 1;
            // cancle NPC collision
            opponentCollider.enabled = false;
        }

        // "遊戲結束"文字顯示
        if (GameEndDetect.gameEnd)
        {
            if (GameEndDetect.finishCount == 3 || (GameEndDetect.finishCount == 2 && GameEndDetect.failCount == 0))
            {
                output.text = "遊戲結束!你獲勝了!";
            }
            else if (GameEndDetect.failCount > 1)
            {
                output.text = "遊戲結束!你輸了!";
            }
        }

        // Rotate the camera to face the NPC if triggered
        if (shouldRotate && npcTransform != null)
        {
            // Get direction to NPC
            Vector3 directionToNPC = npcTransform.position - cameraTransform.position;
            directionToNPC.y = 0; // Keep the rotation level on the Y-axis only

            // Calculate target rotation
            Quaternion targetRotation = Quaternion.LookRotation(directionToNPC);

            // Smoothly rotate the camera toward the NPC
            cameraTransform.rotation = Quaternion.Lerp(cameraTransform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // Optional: Stop rotating when close enough to the target rotation
            if (Quaternion.Angle(cameraTransform.rotation, targetRotation) < 1f)
            {
                shouldRotate = false; // Stop rotating when nearly aligned
            }
        }
    }

    // 玩家碰到AI出題目
    void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the trigger zone is the Player
        if (other.CompareTag("Player"))
        {
            // Start NPC dialogue or action
            StartTalking();
            shouldRotate = true; // Trigger the camera to face the NPC
            shouldDisplayCanvas = true; // Set condition to show the Canvas

            // choose a random quiz
            string randomQuiz = quiz[Random.Range(0, quiz.Length)];
            output.text = randomQuiz;
        }
    }

    // 玩家離開時可以自由移動
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) // Check if Player exits the trigger
        {
            // shouldDisplayCanvas = false; // Set condition to hide the Canvas
            shouldRotate = false; // Stop rotating the camera
            CameraMovement.canMove = 1; // Enable player movement
        }
    }


    private void StartTalking()
    {
        // Replace this with your UI Dialogue system
        // Debug.Log(npcDialogue); // Simple placeholder to log dialogue in the Console
        // stop player movement
        CameraMovement.canMove = 0;
        shouldRotate = true;
    }

    public void ShowCanvas()
    {
        canvasObject.SetActive(true); // Enable the Canvas
        // Debug.Log("Canvas is now visible.");
    }

    public void HideCanvas()
    {
        canvasObject.SetActive(false); // Disable the Canvas
        // Debug.Log("Canvas is now hidden.");
    }
}

/* gameend code
 
*/

// int randomInt = Random.Range(min, max);

/* show canvas code
 public GameObject canvasObject; // Reference to the Canvas GameObject

    // Example condition (you can replace this with any condition)
    public static bool shouldDisplayCanvas = false;

    void Update()
    {
        print("shouldDisplayCanvas: " + shouldDisplayCanvas); // Log the condition

        // Check the condition to display or hide the Canvas
        if (shouldDisplayCanvas && !canvasObject.activeSelf)
        {
            ShowCanvas();
        }
        else if (!shouldDisplayCanvas && canvasObject.activeSelf)
        {
            HideCanvas();
        }
    }

    public void ShowCanvas()
    {
        canvasObject.SetActive(true); // Enable the Canvas
        Debug.Log("Canvas is now visible.");
    }

    public void HideCanvas()
    {
        canvasObject.SetActive(false); // Disable the Canvas
        Debug.Log("Canvas is now hidden.");
    }

    // Optional: Public method to toggle Canvas visibility
    public void ToggleCanvas()
    {
        bool isActive = canvasObject.activeSelf;
        canvasObject.SetActive(!isActive); // Toggle active state
        Debug.Log($"Canvas is now {(isActive ? "hidden" : "visible")}.");
    }
}
*/
/* face to camera code
using UnityEngine;

public class FaceNPC : MonoBehaviour
{
    public Transform npcTransform;       // Reference to the NPC's Transform
    public float rotationSpeed = 5f;     // Speed of rotation (for smooth turning)

    private Transform cameraTransform;   // Reference to the camera's Transform
    private bool shouldRotate = false;   // To trigger rotation

    void Start()
    {
        // Get the camera's transform
        cameraTransform = Camera.main.transform; // Assuming the main camera
    }

    void Update()
    {
        // Rotate the camera to face the NPC if triggered
        if (shouldRotate && npcTransform != null)
        {
            // Get direction to NPC
            Vector3 directionToNPC = npcTransform.position - cameraTransform.position;
            directionToNPC.y = 0; // Keep the rotation level on the Y-axis only

            // Calculate target rotation
            Quaternion targetRotation = Quaternion.LookRotation(directionToNPC);

            // Smoothly rotate the camera toward the NPC
            cameraTransform.rotation = Quaternion.Lerp(cameraTransform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // Optional: Stop rotating when close enough to the target rotation
            if (Quaternion.Angle(cameraTransform.rotation, targetRotation) < 1f)
            {
                shouldRotate = false; // Stop rotating when nearly aligned
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player reaches the NPC
        if (other.CompareTag("Player"))
        {
            shouldRotate = true; // Trigger the camera to face the NPC
        }
    }
}
*/
