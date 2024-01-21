using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Field;
using UnityEngine;

public class WarpSystem : MonoBehaviour
{
    [SerializeField] public Player player;
    [SerializeField] public Transform playerTransform;
    [SerializeField] public CinemachineConfiner2D cameraConfier;
    [SerializeField] public CompositeCollider2D[] compositeCollider;
    [SerializeField] public EnemyPlacer enemyPlacer;

    [SerializeField] private BGMSystem[] audioSources;

    private Vector3[] respawnPos = new[] {
        new Vector3(15f, 10f, 0f),
        new Vector3(110f, 10f, 0f),
        new Vector3(210f, 10f, 0f),
        new Vector3(310f, 10f, 0f),
        new Vector3(410f, 10f, 0f)
    };

    private void Start()
    {
        var fieldId = GetfieldId();
        
        if (1 <= fieldId && fieldId <= 3)
        {
            enemyPlacer.PlaceKemono(fieldId);
            cameraConfier.m_BoundingShape2D = compositeCollider[fieldId];
        }
        audioSources[fieldId].Play();
        
        player.WarpFunc += WarpTo;
    }

    private int GetfieldId()
    {
        var fieldId = (int)(playerTransform.position.x / 100);
        return fieldId;
    }

    private void WarpTo(int fieldId)
    {
        enemyPlacer.RemovePreviousKemono();
        enemyPlacer.PlaceKemono(fieldId);

        var oldFieldId = GetfieldId();
        audioSources[fieldId].Play();
        audioSources[oldFieldId].Stop();
        
        playerTransform.position = respawnPos[fieldId];
        cameraConfier.m_BoundingShape2D = compositeCollider[fieldId];
    }
}
