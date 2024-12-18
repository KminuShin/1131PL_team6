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
            print($"<color=#3f3>玩家輸入: {input}</color>");
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
        private string role = "你是一個判斷答案的機器人，只能回答1或0來代表是或否，請根據指定的情境判斷玩家的行為是否能存活。";
        private string[] npcSentences;

        // [SerializeField, Header("NPC 物件")]
        // private NPCInteraction npc;


        /// 儲存玩家輸入欄位

        //喚醒事件:遊戲開始撥放後會執行一次
        private void Awake()
        {
            ///尋找場景上的名稱為 輸入欄位 的物件並存放到inputField 變數內
            inputFieldPlayer = GameObject.Find("輸入欄位").GetComponent<TMP_InputField>();
            ///當玩家結束編輯輸入欄位會執行 PlayerInput 方法
            inputFieldPlayer.onEndEdit.AddListener(PlayerInput);
            ///獲得NPC要分析的語句
            // npcSentences = npc.data.sentences;
        }

        private void PlayerInput(string input)
        {
            print($"<color=#363>{input}</color>");
            prompt = "現在的情境是: "+"這裡放這次的題目"+"；玩家的回答是: "+input+"；請判斷玩家是否能存活。";
            //啟動協同程式 獲得結果
            StartCoroutine(GetSimilarity());
        }

        private IEnumerator GetSimilarity()
        {
            var inputs = new
            {
                source_sentence = prompt,
                sentences = npcSentences
            };

            //將資料轉為 json 以及上傳的 byte[] 格式
            string json = JsonConvert.SerializeObject(inputs);
            byte[] postData = Encoding.UTF8.GetBytes(json);
            //透過 POST 
            UnityWebRequest request = new UnityWebRequest(url, "POST");
            request.uploadHandler = new UploadHandlerRaw(postData);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", "Bearer " + key);

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                print($"<color=#f33>要求失敗:{request.error}</color>");
            }
            else
            {
                string responseText = request.downloadHandler.text;
                var response = JsonConvert.DeserializeObject<List<float>>(responseText);

                print($"<color=#3f3>分數:{responseText}</color>");
                if (response != null && response.Count > 0)
                {
                    int best = response.Select((value, index) => new
                    {
                        Value = value,
                        Index = index
                    }).OrderByDescending(x => x.Value).First().Index;

                    print($"<color=#77f>最佳結果:{npcSentences[best]}</color>");
                }

            }
            print(request.result);
            print(request.error);
        }
    }
}
