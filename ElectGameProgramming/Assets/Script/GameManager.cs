using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
   
private void Start()
    {
        if (ChatGptManager.Instance != null)
        {
            ChatGptManager.Instance.GenerateQuestion(chapter :5);
            StartCoroutine(WaitForChapterNumbers());
        }
        else
        {
            Debug.LogError("ChatGptManager is not initialized yet.");
        }
    }

    private IEnumerator WaitForChapterNumbers()
    {
        // Wait until the list is populated
        yield return new WaitUntil(() => ChatGptManager.Instance.ChapterNumbers.Count > 0);

        // Now the list is populated, use it here
        foreach (int chapter in ChatGptManager.Instance.ChapterNumbers)
        {
            Debug.Log("Chapter number: " + chapter);
        }
    }
    }


    

