using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class FireAttack : LeaverActivator
{
    [SerializeField] private List<Transform> fireTransforms;
    [SerializeField] private Transform movePoint;

    private List<Vector3> startPositions;
    public override void Activate()
    {
        startPositions = new List<Vector3>();
        foreach (var posTransform in fireTransforms)
        {
            startPositions.Add(posTransform.position);
            posTransform.DOMove(movePoint.position, 4f);
        }
    }

    public override void Deactivate()
    {
        for (int i = 0; i < startPositions.Count; i++)
        {
            fireTransforms[i].transform.DOMove(startPositions[i],4);
        }
    }
}
