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
            case TutorialQuest.Quest3:
                yield return Quest3();
                break;
            case TutorialQuest.Quest4:
                yield return Quest4();
                break;
            case TutorialQuest.Quest5:
                yield return Quest5();
                break;
            case TutorialQuest.Quest6:
                yield return Quest6();
                break;
            case TutorialQuest.Quest7:
                yield return Quest7();
                break;
            
        }
    }
    private static WaitForSeconds waitTwoSeconds = new WaitForSeconds(2f);
    private static IEnumerator Quest1()
    {
        yield return waitTwoSeconds;
        TutorialManager.Instance.CompleteCurrentStep(TutorialQuest.Quest1);
    }
    
    private static IEnumerator Quest2()
    {
        yield return Quest2Condition();
        TutorialManager.Instance.CompleteCurrentStep(TutorialQuest.Quest2);
    }
    private static IEnumerator Quest3()
    {
        yield return Quest3Condition();
        TutorialManager.Instance.CompleteCurrentStep(TutorialQuest.Quest3);
    }
    private static IEnumerator Quest4()
    {
        yield return Quest4Condition();
        TutorialManager.Instance.CompleteCurrentStep(TutorialQuest.Quest4);
    }
    private static IEnumerator Quest5()
    {
        yield return Quest5Condition();
        TutorialManager.Instance.CompleteCurrentStep(TutorialQuest.Quest5);
    }
    private static IEnumerator Quest6()
    {
        yield return waitTwoSeconds;
        TutorialManager.Instance.CompleteCurrentStep(TutorialQuest.Quest6);
    }
    private static IEnumerator Quest7()
    {
        yield return waitTwoSeconds;
        TutorialManager.Instance.CompleteCurrentStep(TutorialQuest.Quest7);
    }
    
    
    
    private static bool movedUp = false ;
    private static bool movedDown = false ;
    private static bool movedLeft = false ;
    private static bool movedRight = false ;
    private static IEnumerator Quest2Condition()
    {
        Vector2 movement = InputManager.Instance.playerControls.Movement.Move.ReadValue<Vector2>();
        if (movement.y > 0) movedUp = true;    // W
        if (movement.y < 0) movedDown = true;  // S
        if (movement.x < 0) movedLeft = true;  // A
        if (movement.x > 0) movedRight = true; // D
        if (movedDown && movedUp && movedLeft && movedRight)
        {
            yield return waitTwoSeconds;
            yield break;
        }

        yield return null;
        yield return Quest2Condition();
    }
    private static IEnumerator Quest3Condition()
    {
        if (InputManager.Instance.playerControls.Combat.Dash.IsPressed())
        {
            yield return waitTwoSeconds;
            yield break;
        }
        yield return null;
        yield return Quest3Condition();
    }
    private static IEnumerator Quest4Condition()
    {
        if (InputManager.Instance.playerControls.Combat.Attack.IsPressed())
        {
            yield return waitTwoSeconds;
            yield break;
        }
        yield return null;
        yield return Quest4Condition();
    }
    private static IEnumerator Quest5Condition()
    {
        if (InputManager.Instance.playerControls.Combat.ActivateSkill.IsPressed())
        {
            yield return waitTwoSeconds;
            yield break;
        }
        yield return null;
        yield return Quest5Condition();
    }
}