namespace ProjectTINR;

public enum PlayerState
{
    None = 0,
    Idling = 1, 
    Moving = 2, // 1 << 1
    Jumping = 4, // 1 << 2
    Falling = 8, // 1 << 3
    TookDmg = 16, // 1 << 4
    Frozen = 32, // 1 << 5
    Shooting = 64, // 1 << 6
}

public enum PlayerDirection {
    Left,
    Right
}
