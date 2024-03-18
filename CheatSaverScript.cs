using ThunderRoad;
using System.IO;
using Newtonsoft.Json;

namespace CheatSaver
{
    public class CheatSaverScript : ThunderScript
    {
        private static string DebugFileName = FileManager.aaModPath + "/CheatSaver/DebugOptions.opt";
        private static DebugOptions debugOptions;
        private bool isFlipped;

        public override void ScriptLoaded(ModManager.ModData modData)
        {
            base.ScriptLoaded(modData);
            debugOptions = new DebugOptions();
            EventManager.onPossess += onPossessionEvent;
            EventManager.onUnpossess += OnUnpossessionEvent;
        }

        private void onPossessionEvent(Creature creature, EventTime eventTime)
        {
            if (eventTime == EventTime.OnEnd)
            {
                if (!File.Exists(DebugFileName))
                    SaveDebugSettings();

                LoadDebugSettings();
            }
        }

        private void OnUnpossessionEvent(Creature creature, EventTime eventTime)
        {
            if (eventTime == EventTime.OnEnd)
                isFlipped = false;
        }

        public override void ScriptUpdate()
        {
            base.ScriptUpdate();
            if (Player.currentCreature != null)
            {
                if (UIPlayerMenu.instance.IsShownToPlayer != isFlipped)
                {
                    isFlipped = !isFlipped;
                    if (!UIPlayerMenu.instance.IsShownToPlayer && DebugSettingsChanged())
                        SaveDebugSettings();
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
                debugOptions.collisionMarkers != GameManager.local.collisionDebug ||
                debugOptions.easyDismemberment != Damager.easyDismemberment ||
                debugOptions.climbFree != RagdollHandClimb.climbFree;
        }

        private static void LoadDebugSettings()
        {
            DebugOptions options;

            TextReader reader = null;
            try
            {
                reader = new StreamReader(DebugFileName);
                var fileContents = reader.ReadToEnd();
                options = JsonConvert.DeserializeObject<DebugOptions>(fileContents);
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }

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
                RagdollHandClimb.climbFree = options.climbFree;
                Damager.easyDismemberment = options.easyDismemberment;
            }
        }

        private static void SaveDebugSettings()
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
            debugOptions.climbFree = RagdollHandClimb.climbFree;
            debugOptions.easyDismemberment = Damager.easyDismemberment;

            TextWriter writer = null;
            try
            {
                var contents = JsonConvert.SerializeObject(debugOptions);
                writer = new StreamWriter(DebugFileName);
                writer.Write(contents);
            }
            finally
            {
                if (writer != null)
                    writer.Close();
            }
        }
    }
}