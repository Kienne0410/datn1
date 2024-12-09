using UnityEngine;
using System;
using System.Collections;
using NUnit.Framework;

[Serializable]
public class TutorialStep
{
    private bool IsPassed = false;
    public string instructionText; 
    public GameObject uiElement;  
    public TutorialQuest completionQuest;
    public void Pass()
    {
        IsPassed = true;
    }

    public bool QuestIsPassed()
    {
        return IsPassed;
    }

    public static IEnumerator Step(TutorialQuest quest)
    {
        switch (quest)
        {
            case TutorialQuest.Quest1:
                yield return Quest1();
                break;
            case TutorialQuest.Quest2:
                yield return Quest2();
                break;
                
        }
    }

    private static IEnumerator Quest1()
    {
        yield return new WaitForSeconds(2f);
        TutorialManager.Instance.CompleteCurrentStep(TutorialQuest.Quest1);
    }
    
    private static IEnumerator Quest2()
    {
        yield return new WaitForSeconds(2f);
        TutorialManager.Instance.CompleteCurrentStep(TutorialQuest.Quest2);
    }
}