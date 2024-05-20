using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplayPlayer : MonoBehaviour
{
    public void SetReplayData(ReplayData data)
    {
        transform.position = data.position;
    }
}
