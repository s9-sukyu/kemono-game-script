using System;
using System.Collections.Generic;
using UnityEngine;

// DataManager -> DM
public static class DM
{
    public static Guid UserGuid = new Guid("00000000-0000-0000-0000-000000000001");
    public static string PlayerName = "playername";
    // public static Guid PlayerKemonoId = new Guid("00000000-0000-0000-0000-000000000001");
    public static Vector3 PlayerPos = new Vector3(15, 10, 0);
    
    public static bool IsBattle = false;
    public static Guid BattleId;
    public static Guid EnemyKemonoId = new Guid("00000000-0000-0000-0000-000000000002");

    public static GetKemonoResponse PlayerKemono = new GetKemonoResponse();
    public static GetKemonoResponse EnemyKemono;
    public static GetKemonoResponse BornKemono;
    
    public static bool IsSelectingKemono2;
    public static GetKemonoResponse SelectedKemono;
    public static GetKemonoResponse SelectedKemono2;

    public static RenderTexture RenderTex = new RenderTexture(Screen.width, Screen.height, 24);
    public static Sprite BackgroundSprite = null;

    public static string ConceptsForBear;
    public static List<Guid> ConceptIds;
    
    // Etagの実装ができたらこれが使える
    // public static HashSet<Guid> CollectedImageIds = new HashSet<Guid>();
    public static Dictionary<Guid, Texture2D> CollectedImageSprites = new ();
}

public static class BattleData
{
    public enum BattleState
    {
        PlayerTurnState, ReadPlayerTextState, EnemyTurnState, ReadEnemyTextState
    }

    public static BattleState State = BattleState.PlayerTurnState;
}