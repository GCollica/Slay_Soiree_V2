using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class RoomProgress : MonoBehaviour
{
    public enum RoomType {Standard, Boss, Shop, Staging};
    public RoomType roomType;
    public enum RoomState {Idle, QuirkChoice, Active, Completed, Failed, CompletedRun};
    public RoomState currentState;

    public GameObject exitDoor;

    UnityEvent stateChangedEvent;
    
    private WaveManager waveManager;
    private QuirkManager quirkManager;
    private QuirkPickerUI quirkPickerUI;
    private ScreenFadeHandler screenFadeHandler;
    private RunHandler runHandler;

    private void Awake()
    {
        if(stateChangedEvent == null)
        {
            stateChangedEvent = new UnityEvent();
        }

        stateChangedEvent.AddListener(StateChanged);

        if(roomType == RoomType.Staging)
        {
            AssignSystemsReferences();
        }
        else if (roomType == RoomType.Standard || roomType == RoomType.Boss || roomType == RoomType.Shop)
        {
            AssignSystemsReferences();
            AssignRoomReferences();         
        }

        currentState = RoomState.Idle;
        stateChangedEvent.Invoke();
        
    }

    public void AssignRoomReferences()
    {
        waveManager = this.gameObject.GetComponent<WaveManager>();
    } 

    public void AssignSystemsReferences()
    {
        quirkManager = FindObjectOfType<QuirkManager>();
        quirkPickerUI = FindObjectOfType<QuirkPickerUI>();
        screenFadeHandler = FindObjectOfType<ScreenFadeHandler>();
        runHandler = FindObjectOfType<RunHandler>();
    }

    public void BeginRoom()
    {
        stateChangedEvent.Invoke();
    }

    private void StateChanged()
    {
        switch (currentState)
        {
            case RoomState.Idle:
                screenFadeHandler.FadeOut();
                //Re-assign room references.
                break;

            case RoomState.QuirkChoice:
                //Quirk picking sequence.
                quirkManager.ChoseQuirkChoices();
                quirkPickerUI.BeginFadeInUI();
                break;

            case RoomState.Active:
                //Begin spawning enemies
                waveManager.BeginRoomWaves();
                break;

            case RoomState.Completed:
                //screenFadeHandler.FadeIn();
                exitDoor.SetActive(true);
                //runHandler.BeginNewRoomCoroutine();
                //Transition into new room
                break;

            case RoomState.Failed:
                //Show game over screen? Throw players back to main menu.
                break;

            case RoomState.CompletedRun:
                //Show win screen. Maybe throw players into a win room? pummel party style.

            default:
                break;           
        }
    }

    public void ChangeRoomState(RoomState targetState)
    {
        if(currentState == targetState)
        {
            return;
        }

        currentState = targetState;
        stateChangedEvent.Invoke();
    }

}
