using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NPCInteraction : MonoBehaviour
{
    // [Header("UI Elements")]
    // public TMP_Text displayText; // TextMeshPro UI element

    public string[] quiz = {"�A�ﯺ�L��", "�A���٨��b�~�k���s�Y�W���F�F", "�A�~�ڪ��F�~�@�߷Q���A", "�p�G�A�A�����Q���A�A�|��", "�A��M�o�{�ۤv�L�k����b�]", "�A�Q�x�b�@�ɿU�N���j�Ӹ�", "�A�Q�x�b�@�����b�U�I����W", "�A�Q�x�b�@���s������", "�A���q���żY��", "�A����@�����㪺���ͺ�", "�A�b�ɭ������g���F", "�A�Q��b�@�����b�U�I������", "�A�����{�@�Y�ĹL�Ӫ��R��", "�A�Q�@���r�D�G�J�F����", "�A�Q�x�b�@�ӥ��b�Y���a�V�W", "�A�Q�x�b�@�y���b�~���q�|��", "�A�Q��b�@�Ӧ��w���@�T���u���ж���", "�A���b��פ@�s���H��������", "�A�q�������F�X�h�A�ӧA�������ʥ����}", "�A�g���b�@���S�������F�z��", "�@�����Y���§Aŧ��", "�A�Q�x�b�@�Ӧ����j��l���ж���", "�a����n�D�A�T�֥L�A�_�h�N�n�A��", "�A���Q 500 ���p������", "�A���F�a�p�A�p�G��_�}�A���N�|�z��"};
    
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

        // �a��NPC����D�خ�
        if (shouldDisplayCanvas && !canvasObject.activeSelf)
        {
            ShowCanvas();
        }
        else if (!shouldDisplayCanvas && canvasObject.activeSelf)
        {
            HideCanvas();
        }

        // ��ҫ��^�е��׮�
        if (displayAIresponse) 
        {
            print("displayAIresponse: " + displayAIresponse); // Log the condition
            if (getAIresponse == "1")
            {
                output.text = "�A�s���F";
                GameEndDetect.finishCount += 1;
            }
            else if (getAIresponse == "0")
            {
                output.text = "�A���F";
                GameEndDetect.finishCount += 1;
                GameEndDetect.failCount += 1;
            }
            else
            {
                output.text = "�A���^�����X�k";
            }
            displayAIresponse = false;
            // cancel no player movement
            CameraMovement.canMove = 1;
            // cancle NPC collision
            opponentCollider.enabled = false;
        }

        // "�C������"��r���
        if (GameEndDetect.gameEnd)
        {
            if (GameEndDetect.finishCount == 3 || (GameEndDetect.finishCount == 2 && GameEndDetect.failCount == 0))
            {
                output.text = "�C������!�A��ӤF!";
            }
            else if (GameEndDetect.failCount > 1)
            {
                output.text = "�C������!�A��F!";
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

    // ���a�I��AI�X�D��
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

    // ���a���}�ɥi�H�ۥѲ���
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
