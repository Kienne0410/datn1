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
    private static bool movedUp = false ;
    private static bool movedDown = false ;
    private static bool movedLeft = false ;
    private static bool movedRight = false ;
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
        while (!(movedDown && movedUp && movedLeft && movedRight))
        {
            Vector2 movement = InputManager.Instance.playerControls.Movement.Move.ReadValue<Vector2>();
            if (movement.y > 0) movedUp = true;    // W
            if (movement.y < 0) movedDown = true;  // S
            if (movement.x < 0) movedLeft = true;  // A
            if (movement.x > 0) movedRight = true; // D
            yield return null;
        }
        yield return waitTwoSeconds;
        TutorialManager.Instance.CompleteCurrentStep(TutorialQuest.Quest2);
    }
    private static IEnumerator Quest3()
    {
        while (!InputManager.Instance.playerControls.Combat.Dash.IsPressed())
        {
            yield return null;
        }
        yield return waitTwoSeconds;
        TutorialManager.Instance.CompleteCurrentStep(TutorialQuest.Quest3);
    }
    private static IEnumerator Quest4()
    {
        while (!InputManager.Instance.playerControls.Combat.Attack.IsPressed())
        {
            yield return null;
        }
        yield return waitTwoSeconds;
        TutorialManager.Instance.CompleteCurrentStep(TutorialQuest.Quest4);
    }
    private static IEnumerator Quest5()
    {
        while (!InputManager.Instance.playerControls.Combat.ActivateSkill.IsPressed())
        {
            yield return null;
        }
        yield return waitTwoSeconds;
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
}