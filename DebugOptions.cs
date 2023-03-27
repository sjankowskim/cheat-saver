using System;
using ThunderRoad;

namespace CheatSaver
{
    [Serializable]
    public class DebugOptions
    {
        public bool invincibility;
        public bool infiniteMana;
        public bool infiniteFocus;
        public bool infiniteImbue;
        public bool infiniteArrows;
        public bool fallDamage;
        public bool selfCollision;
        public bool armorDetection;
        public float slowMotionScale;
        public GameManager.CollisionDebug collisionMarkers;
        public bool climbFree;
        public bool easyDismemberment;
    }
}