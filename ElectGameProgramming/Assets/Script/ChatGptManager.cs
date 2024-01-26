using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenAI;
using UnityEditorInternal;
using System;
using UnityEditor;

public class ChatGptManager : MonoBehaviour
{
   private string apiKey = "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";
   public List<int> ChapterNumbers { get; private set; }

   private static ChatGptManager _instance;
   public static ChatGptManager Instance=_instance;
   private List<int> chapterNumbers = new List<int>();
   public event Action<List<int>> OnChapterNumbersReady;

   private void Awake() {
    ChapterNumbers = new List<int>();
      if (Instance == null)
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    else if (Instance != this)
    {
        Destroy(gameObject);
    }
   }
   public List<int> GetChapterNumbers()
    {
        return chapterNumbers;
    }
   
   public async void GenerateQuestion( float chapter)
   {
    var openai =new OpenAIApi(apiKey);
    List<ChatMessage>requestMessage=new List<ChatMessage>();
   

    var newMessage = new ChatMessage()
    {
        Role="user",
        Content=$"Generate a 5 different chapter numbers from bible chapter each numbers will be different and  random :[{chapter}] ,\n"+
        $"1.[number]\n"+
        $"2.[number]\n"+
        $"3.[number]\n"+
        $"4.[number]\n"+
        $"5.[number]\n"
        
    };

    requestMessage.Add(newMessage);

     CreateChatCompletionResponse response=await openai.CreateChatCompletion(new CreateChatCompletionRequest()
     {
        Model ="gpt-3.5-turbo-0301",
        Messages=requestMessage
     });
     if(response.Error == null)
        {
            string requestContent = response.Choices[0].Message.Content;
            Debug.Log("Response: \n" + requestContent);

            // Parse and store the chapter numbers
            ParseAndStoreChapterNumbers(requestContent);
        }
        OnChapterNumbersReady?.Invoke(chapterNumbers);
   }
   private void ParseAndStoreChapterNumbers(string content)
    {
        chapterNumbers.Clear(); // Clear previous numbers

        // Example parsing logic (modify as needed based on actual response format)
        string[] lines = content.Split('\n');
        foreach (var line in lines)
        {
            if (line.Contains("[number]"))
            {
                string numberStr = line.Replace("[number]", "").Trim();
                if (int.TryParse(numberStr, out int number))
                {
                    chapterNumbers.Add(number);
                }
            }
        }

        // Example: Print the numbers to the console
        foreach (var num in chapterNumbers)
        {
            Debug.Log("Chapter Number: " + num);
        }
    }
}
