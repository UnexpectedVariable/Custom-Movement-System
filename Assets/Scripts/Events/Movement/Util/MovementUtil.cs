namespace Assets.Scripts.Events.Movement.Util
{
    public static class MovementUtil
    {
        public enum MovementType
        {
            Forward,
            Backward,
            StrafeRight,
            StrafeLeft,
            Up,
            Down
        }

        public static MovementType? GetMovementType(string movementName)
        {
            return movementName switch
            {
                "Forward" => MovementType.Forward,
                "Backward" => MovementType.Backward,
                "StrafeRight" => MovementType.StrafeRight,
                "StrafeLeft" => MovementType.StrafeLeft,
                "Up" => MovementType.Up,
                "Down" => MovementType.Down,
                _ => null
            };
        }

        public static string GetMovementName(MovementType movementType)
        {
            return movementType switch
            {
                MovementType.Forward => "Forward",
                MovementType.Backward => "Backward",
                MovementType.StrafeRight => "StrafeRight",
                MovementType.StrafeLeft => "StrafeLeft",
                MovementType.Up => "Up",
                MovementType.Down => "Down",
                _ => null
            };
        }
    }
}
