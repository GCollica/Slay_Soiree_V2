using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_AI : MonoBehaviour
{
    public enum BossPhases { idle, phase1, phase2, dead };
    public BossPhases CurrentPhase = BossPhases.idle;
    private GameObject leftHand;
    private GameObject rightHand;
}
