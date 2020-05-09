using UnityEngine;

public class GameplayConstants : ScriptableObject
{
    public const float ENEMY_SCALE = 6f;
    public const float GRAVITY_SCALE = 3f;
    public const float HEALTH_SIZE_SCALAR = 1.35f;
    public const float RESPAWN_HEIGHT = 32f;
    public const float SLIP_ZONE_WIDTH = 0.015f;
    public const float SPAWN_ZONE_DROP_HEIGHT = 4f;
    public const float SPAWN_ZONE_MINIMUM_HEIGHT = 3f;
    public const float SPAWN_ZONE_MINIMUM_WIDTH = 2f;
    public const float START_DISTANCE = 4.5f;

    public const int ENEMY_POOL_SIZE = 10;
    public const int LAYER_Enemy = 8;
    public const int LAYER_Radar = 12;
    public const int MAXIMUM_SECTIONS = 6;
    public const int SCORE_DISTANCE_MULTIPLIER = 5;
    public const int SCORE_ENEMY_MULTIPLIER = 50;
    public const int STARTING_LIVES = 100;

    public const string TAG_Enemy = "Enemy";
    public const string TAG_Ground = "Ground";
    public const string TAG_KillZone = "KillZone";
    public const string TAG_Player = "Player";
    public const string TAG_WakeField = "WakeField";
}
