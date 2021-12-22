using System.Collections;
using ThunderRoad;
using UnityEngine;

namespace CheatSaver
{
    public class CheatSaverLevelModule : LevelModule
    {
        private const string DebugFileName = "DebugOptions.opt";
        private const string DebugFileNameBackup = "DebugOptionsOriginal.opt";
        private static string modTag = "(Cheat Saver)";
        private static DebugOptions debugOptions;
        private bool isFlipped, skipBackup;

        public override IEnumerator OnLoadCoroutine()
        {
            Debug.Log(modTag + " Loaded successfully!");
            debugOptions = new DebugOptions();
            EventManager.onLevelLoad += OnLoadEvent;
            EventManager.onUnpossess += OnUnpossessionEvent;
            return base.OnLoadCoroutine();
        }

        private void OnLoadEvent(LevelData levelData, EventTime eventTime)
        {
            if (eventTime == EventTime.OnEnd)
            {
                if (!skipBackup)
                {
                    skipBackup = true;
                    SaveDebugSettings(true);
                }
                LoadDebugSettings();
            }
        }

        private void OnUnpossessionEvent(Creature creature, EventTime eventTime)
        {
            if (eventTime == EventTime.OnEnd)
                isFlipped = false;
        }

        public override void Update()
        {
            base.Update();
            if (Player.currentCreature != null)
            {
                if (MenuBook.isShownToPlayer != isFlipped)
                {
                    isFlipped = !isFlipped;
                    if (!MenuBook.isShownToPlayer && DebugSettingsChanged())
                        SaveDebugSettings(false);
                }
            }
        }

        private static bool DebugSettingsChanged()
        {
            return debugOptions.invincibility != Player.invincibility ||
                debugOptions.infiniteMana != Mana.infiniteMana ||
                debugOptions.infiniteFocus != Mana.infiniteFocus ||
                debugOptions.infiniteImbue != Imbue.infiniteImbue ||
                debugOptions.infiniteArrows != ItemModuleBow.forceAutoSpawnArrow ||
                debugOptions.fallDamage != Player.fallDamage ||
                debugOptions.selfCollision != Player.selfCollision ||
                debugOptions.armorDetection != Creature.meshRaycast ||
                debugOptions.slowMotionScale != Player.currentCreature.mana.GetPowerSlowTime().scale ||
                debugOptions.collisionMarkers != GameManager.local.collisionDebug;
        }

        private static void LoadDebugSettings()
        {
            var options = DataManager.LoadLocalFile<DebugOptions>(DebugFileName);
            if (options != null)
            {
                Player.invincibility = options.invincibility;
                Mana.infiniteMana = options.infiniteMana;
                Mana.infiniteFocus = options.infiniteFocus;
                Imbue.infiniteImbue = options.infiniteImbue;
                ItemModuleBow.forceAutoSpawnArrow = options.infiniteArrows;
                Player.fallDamage = options.fallDamage;
                Player.selfCollision = options.selfCollision;
                Creature.meshRaycast = options.armorDetection;
                GameManager.local.collisionDebug = options.collisionMarkers;
                Player.currentCreature.mana.GetPowerSlowTime().scale = options.slowMotionScale;
            }
        }

        private static void SaveDebugSettings(bool backup)
        {
            debugOptions.invincibility = Player.invincibility;
            debugOptions.infiniteMana = Mana.infiniteMana;
            debugOptions.infiniteFocus = Mana.infiniteFocus;
            debugOptions.infiniteImbue = Imbue.infiniteImbue;
            debugOptions.infiniteArrows = ItemModuleBow.forceAutoSpawnArrow;
            debugOptions.fallDamage = Player.fallDamage;
            debugOptions.selfCollision = Player.selfCollision;
            debugOptions.armorDetection = Creature.meshRaycast;
            debugOptions.slowMotionScale = Player.currentCreature.mana.GetPowerSlowTime().scale;
            debugOptions.collisionMarkers = GameManager.local.collisionDebug;
            DataManager.SaveLocalFile(debugOptions, backup ? DebugFileNameBackup : DebugFileName);
        }
    }
}