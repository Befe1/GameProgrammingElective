using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Test : MonoBehaviour
{
   
    private void Awake()
    {
        if (ChatGptManager.Instance != null)
        {
            List<int> chapterNumbers = ChatGptManager.Instance.GetChapterNumbers();
            // Use chapterNumbers here
             foreach (int chapter in chapterNumbers)
        {
            Debug.Log("Chapter number: " + chapter);
        }
        }
        else
        {
            Debug.LogError("ChatGptManager is not initialized yet.");
        }
    }
       
    }

