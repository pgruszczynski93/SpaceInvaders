﻿namespace SpaceInvaders
{
    public enum ShaderProperties
    {
        EmissionPower,
    }

    public enum VFXActions
    {
        EnableVFX,
        EnableAndDetachVFX,
        EnableAndAttachVFX
    }

    public enum SimpleLoggerTypes
    {
        Log, 
        Warning,
        Error
    }

    public enum MovementDirection
    {
        Up,
        Down,
        Left, 
        Right
    }

    public enum BonusType
    {
        Life,
        Shield,
        Weapon2x,
        Weapon3x
    }

    public enum MovementType
    {
        Basic,
        Fast,
        Slow
    }

    public enum EnemyType
    {
        Basic, 
        Special
    }

    public enum WeaponType
    {
        None,
        Projectile, 
        Projectile2x,
        Projectile3x,
        Laser,
        Laser2x,
        Laser3x
    }

    public enum AddedTags
    {

    }

    public enum Events
    {
        OnPlayerMove, 
        OnPlayerShoot
    }


}