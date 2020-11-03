using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class RoomProgress : MonoBehaviour
{
    public enum RoomState {Idle, QuirkChoice, Active, Completed, Failed};
    public RoomState currentState;

    UnityEvent stateChangedEvent;
    public WaveManager waveManager;

    private void Awake()
    {
        if(stateChangedEvent == null)
        {
            stateChangedEvent = new UnityEvent();
        }

        waveManager = this.gameObject.GetComponent<WaveManager>();
        currentState = RoomState.Idle;
        stateChangedEvent.AddListener(StateChanged);

        //Invoke(nameof(ChangeTest), 1f);
        
    }

    private void ChangeTest()
    {
        currentState = RoomState.Active;
        stateChangedEvent.Invoke();
    }
    private void StateChanged()
    {
        switch (currentState)
        {
            case RoomState.Idle:
                //Chill.
                //Re-assign room references.
                break;

            case RoomState.QuirkChoice:
                //Quirk picking sequence.
                break;

            case RoomState.Active:
                //Begin spawning enemies
                waveManager.BeginRoomWaves();
                break;

            case RoomState.Completed:
                //Transition into new room
                break;

            case RoomState.Failed:
                //Show game over screen? Throw players back to main menu.
                break;

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
