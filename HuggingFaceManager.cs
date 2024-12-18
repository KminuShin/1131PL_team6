/*
using TMPro;
using UnityEngine;
using UnityEngine.Windows;

namespace six
{
    public class HuggingFaceManager : MonoBehaviour
    {
        private string key = "AIzaSyCrSOjGwLrqzHh_KnhoIgjCO_FgdMTNOF4";
        
        private string model = "https://api-inference.huggingface.co/models/sentence-transformers/all-MiniLM-L6-v2";

        private TMP_InputField inputFieldPlayer;

        private void Awake()
        {
            inputFieldPlayer = GameObject.Find("InputField (TMP)").GetComponent<TMP_InputField>();
            inputFieldPlayer.onEndEdit.AddListener(PlayerInput);
        }

        private void PlayerInput(string input)
        {
            print($"<color=#3f3>���a��J: {input}</color>");
        }
    }
}
*/

using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Compilation;

namespace Waka
{
    public class HuggingFaceManager : MonoBehaviour
    {
        private string url = "https://api-inference.huggingface.co/models/sentence-transformers/all-MiniLM-L6-v2";
        private string key = "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";

        private TMP_InputField inputFieldPlayer;
        private string prompt;
        private string role = "�A�O�@�ӧP�_���ת������H�A�u��^��1��0�ӥN��O�Χ_�A�Юھګ��w�����ҧP�_���a���欰�O�_��s���C";
        private string[] npcSentences;

        // [SerializeField, Header("NPC ����")]
        // private NPCInteraction npc;


        /// �x�s���a��J���

        //����ƥ�:�C���}�l�����|����@��
        private void Awake()
        {
            ///�M������W���W�٬� ��J��� ������æs���inputField �ܼƤ�
            inputFieldPlayer = GameObject.Find("��J���").GetComponent<TMP_InputField>();
            ///���a�����s���J���|���� PlayerInput ��k
            inputFieldPlayer.onEndEdit.AddListener(PlayerInput);
            ///��oNPC�n���R���y�y
            // npcSentences = npc.data.sentences;
        }

        private void PlayerInput(string input)
        {
            print($"<color=#363>{input}</color>");
            prompt = "�{�b�����ҬO: "+"�o�̩�o�����D��"+"�F���a���^���O: "+input+"�F�ЧP�_���a�O�_��s���C";
            //�Ұʨ�P�{�� ��o���G
            StartCoroutine(GetSimilarity());
        }

        private IEnumerator GetSimilarity()
        {
            var inputs = new
            {
                source_sentence = prompt,
                sentences = npcSentences
            };

            //�N����ର json �H�ΤW�Ǫ� byte[] �榡
            string json = JsonConvert.SerializeObject(inputs);
            byte[] postData = Encoding.UTF8.GetBytes(json);
            //�z�L POST 
            UnityWebRequest request = new UnityWebRequest(url, "POST");
            request.uploadHandler = new UploadHandlerRaw(postData);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", "Bearer " + key);

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                print($"<color=#f33>�n�D����:{request.error}</color>");
            }
            else
            {
                string responseText = request.downloadHandler.text;
                var response = JsonConvert.DeserializeObject<List<float>>(responseText);

                print($"<color=#3f3>����:{responseText}</color>");
                if (response != null && response.Count > 0)
                {
                    int best = response.Select((value, index) => new
                    {
                        Value = value,
                        Index = index
                    }).OrderByDescending(x => x.Value).First().Index;

                    print($"<color=#77f>�̨ε��G:{npcSentences[best]}</color>");
                }

            }
            print(request.result);
            print(request.error);
        }
    }
}
