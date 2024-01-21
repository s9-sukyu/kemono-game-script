using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class Err
{
    public string Error;

    public Err(string error)
    {
        Error = error;
    }
}

public class GetKemonoResponse
{
    [JsonProperty("id")]
    public Guid Id { get; set; }
    
    [JsonProperty("image")]
    public byte[] Image { get; set; }
    
    [JsonProperty("prompt")]
    public string Prompt { get; set; }
    
    [JsonProperty("concepts")]
    public string[] Concepts { get; set; }
    
    [JsonProperty("name")]
    public string Name { get; set; }
    
    [JsonProperty("description")]
    public string Description { get; set; }
    
    [JsonProperty("kind")]
    public int Kind { get; set; }
    
    [JsonProperty("color")]
    public int Color { get; set; }
    
    [JsonProperty("is_player")]
    public bool IsPlayer { get; set; }
    
    [JsonProperty("is_for_battle")]
    public bool IsForBattle { get; set; }
    
    [JsonProperty("is_owned")]
    public bool IsOwned { get; set; }
    
    [JsonProperty("owner_id")]
    public Guid OwnerId { get; set; }
    
    [JsonProperty("is_in_field")]
    public bool IsInField { get; set; }
    
    [JsonProperty("is_boss")]
    public bool IsBoss { get; set; }
    
    [JsonProperty("field")]
    public int Field { get; set; }

    [JsonProperty("x")]
    public int X { get; set; }
    
    [JsonProperty("y")]
    public int Y { get; set; }
    
    [JsonProperty("has_parent")]
    public bool HasParent { get; set; }
    
    [JsonProperty("parent1_id")]
    public Guid Parent1Id { get; set; }
    
    [JsonProperty("parent2_id")]
    public Guid Parent2Id { get; set; }
    
    [JsonProperty("has_child")]
    public bool HasChild { get; set; }
    
    [JsonProperty("child_id")]
    public Guid ChildId { get; set; }
    
    [JsonProperty("hp")]
    public int Hp { get; set; }
    
    [JsonProperty("max_hp")]
    public int MaxHp { get; set; }

    [JsonProperty("attack")]
    public int Attack { get; set; }

    [JsonProperty("defense")]
    public int Defence { get; set; }

    [JsonProperty("created_at")]
    public string CreatedAt { get; set; }
}

public class PostBattleResponse
{
    [JsonProperty("battle_id")]
    public Guid BattleId { get; set; }
}

public class PostBattleIdBattleIdResponse
{
    [JsonProperty("text")]
    public string Text { get; set; }
}

public class GetKemonoConceptsResponse
{
    [JsonProperty("id")]
    public Guid Id { get; set; }
    
    [JsonProperty("player_id")]
    public Guid PlayerId { get; set; }
    
    [JsonProperty("concept")]
    public string Concept { get; set; }
    
    [JsonProperty("created_at")]
    public string CreatedAt { get; set; }
}

public class GetUserResponse
{
    [JsonProperty("id")]
    public Guid Id { get; set; }
    
    [JsonProperty("name")]
    public string Name { get; set; }
}

public class PostUserIdResponse
{
    [JsonProperty("id")]
    public Guid Id { get; set; }
}