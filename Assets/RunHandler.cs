using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunHandler : MonoBehaviour
{
    public List<GameObject> allStandardRooms;

    public GameObject stagingRoom;
    public GameObject shopRoom;
    public GameObject bossRoom;

    public GameObject roomParent;
    public GameObject currentRoom;
    private Transform[] currentPlayerPosArray;
    public GameObject nextRoom;

    public int completedRoomsIndex;

    private ScreenFadeHandler screenFadeHandler;
    private QuirkPickerUI quirkPickerUI;
    private PlayerCount playerCount;
    private EnemySpawner enemySpawner;
    private QuirkSpawner quirkSpawner;

    private void Awake()
    {
        ChoseNextRoom();
        screenFadeHandler = FindObjectOfType<ScreenFadeHandler>();
        quirkPickerUI = FindObjectOfType<QuirkPickerUI>();
        playerCount = FindObjectOfType<PlayerCount>();
        enemySpawner = FindObjectOfType<EnemySpawner>();
        quirkSpawner = FindObjectOfType<QuirkSpawner>();
        currentRoom = roomParent.transform.GetChild(0).gameObject;
        SetPlayerPosArray();
    }

    public int ChoseNextRoom()
    {
        int roomTypeIndex;

        if(completedRoomsIndex == 2)
        {
            roomTypeIndex = 2;
        }
        else if(completedRoomsIndex == 3)
        {
            roomTypeIndex = 3;
        }
        else
        {
            roomTypeIndex = 1;
            nextRoom = allStandardRooms[Random.Range(0, (allStandardRooms.Count))];
        }

        return roomTypeIndex;
    }

    public void SetNextRoom()
    {
        switch (ChoseNextRoom())
        {
            case 1:
                nextRoom = allStandardRooms[Random.Range(0, (allStandardRooms.Count))];
                Debug.Log("Next room set to a standard room");
                break;

            case 2:
                nextRoom = shopRoom;
                Debug.Log("Next room set to a shop room");
                break;

            case 3:
                nextRoom = bossRoom;
                Debug.Log("Next room set to a boss room");
                break;

            default:
                break;
        }
    }

    public void BeginNewRoomCoroutine()
    {
        StartCoroutine(nameof(NewRoomCoroutine));
    }

    public IEnumerator NewRoomCoroutine()
    {
        screenFadeHandler.FadeIn();
        yield return new WaitForSeconds(1f);
        ClearCurrentRoom();
        yield return new WaitForSeconds(1f);
        SpawnNewRoom();
        ReasignRoomReferences();
        SetPlayerPos();
        nextRoom = null;
        completedRoomsIndex++;
        SetNextRoom();
        yield return new WaitForSeconds(.5f);
        //screenFadeHandler.FadeOut();
        StopCoroutine(nameof(NewRoomCoroutine));      
    }

    public void ClearCurrentRoom()
    {
        Destroy(currentRoom);
        currentRoom = null;
    }

    public void SpawnNewRoom()
    {
        GameObject spawnedRoom = Instantiate(nextRoom, roomParent.transform.position, Quaternion.identity);
        spawnedRoom.transform.SetParent(roomParent.transform);
        currentRoom = spawnedRoom;
    }

    void ReasignRoomReferences()
    {
        screenFadeHandler.AssignRoomReference();
        quirkPickerUI.AssignCurrentRoomReference();
        playerCount.UpdateActivePlayers();
        SetPlayerPosArray();
        enemySpawner.ReassignRoomReferences();
        quirkSpawner.ReassignRoomReferences();
    }

    void SetPlayerPosArray()
    {
        currentPlayerPosArray = currentRoom.GetComponentInChildren<RoomSpawnPoints>().playerSpawnPoints;
    }
    void SetPlayerPos()
    {
        foreach (GameObject player in playerCount.activePlayers)
        {
            player.transform.SetPositionAndRotation(currentPlayerPosArray[playerCount.activePlayers.IndexOf(player)].position, Quaternion.identity);
        }
    }

}
