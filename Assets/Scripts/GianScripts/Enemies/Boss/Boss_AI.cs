using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_AI : MonoBehaviour
{
    public enum BossPhases { idle, phase1, phase2, dead };
    public BossPhases CurrentPhase = BossPhases.idle;

    private GameObject leftHand;
    private GameObject rightHand;

    private float idleTimer = 0f;
    private float idleDuration = 3.5f;

    private void Awake()
    {
        leftHand = this.gameObject.transform.GetChild(0).gameObject;
        rightHand = this.gameObject.transform.GetChild(1).gameObject;
    }

    private void Update()
    {
        if(CurrentPhase == BossPhases.idle)
        {
            if(idleTimer < idleDuration)
            {
                idleTimer += Time.deltaTime;
            }
            else
            {
                CurrentPhase = BossPhases.phase1;
                leftHand.GetComponent<Boss_LeftHand>().BeginEncounter();
                rightHand.GetComponent<Boss_RightHand>().BeginEncounter();
            }
        }
    }
}
