﻿namespace SpaceInvaders
{
    public enum AsteroidState
    {
        ReadyToMove,
        OnScreen,
    }

    public enum CollisionTag
    {
        Player, 
        Enemy, 
        PlayerWeapon,
        EnemyWeapon,
        Bonus,
    }
    public enum SimpleLoggerTypes
    {
        Log, 
        Warning,
        Error
    }

    public enum BonusType
    {
        Undefined,
        Life,
        Shield,
        Weapon,
        Teleportation,
        TimeSlowDown, 
        Hacking,
        MassiveKill, 
    }

    public enum WeaponTier
    {
        Tier_0,
        Tier_1,
        Tier_2, 
        Tier_3, 
        Tier_4, 
    }
}