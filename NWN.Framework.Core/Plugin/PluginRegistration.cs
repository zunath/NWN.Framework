using System;

namespace NWN.Framework.Core.Plugin
{
    /// <summary>
    /// Tracks each plugin and which app domain they belong to.
    /// </summary>
    internal class PluginRegistration
    {
        public AppDomain AppDomain { get; }
        public IPlugin Plugin { get; }

        public Guid OnModuleAcquireItemID { get; set; }
        public Guid OnModuleActivateItemID { get; set; }
        public Guid OnModuleApplyDamageID { get; set; }
        public Guid OnModuleAttackID { get; set; }
        public Guid OnModuleChatID { get; set; }
        public Guid OnModuleCutsceneAbortID { get; set; }
        public Guid OnModuleDeathID { get; set; }
        public Guid OnModuleDyingID { get; set; }
        public Guid OnModuleEnterID { get; set; }
        public Guid OnModuleEquipItemID { get; set; }
        public Guid OnModuleExamineID { get; set; }
        public Guid OnModuleHeartbeatID { get; set; }
        public Guid OnModuleLeaveID { get; set; }
        public Guid OnModuleLevelUpID { get; set; }
        public Guid OnModuleLoadID { get; set; }
        public Guid OnModuleNWNXChatID { get; set; }
        public Guid OnModuleRespawnID { get; set; }
        public Guid OnModuleRestID { get; set; }
        public Guid OnModuleUnacquireItemID { get; set; }
        public Guid OnModuleUnequipItemID { get; set; }
        public Guid OnModuleUseFeatID { get; set; }
        public Guid OnModuleUserDefinedID { get; set; }

        public Guid OnAreaEnterID { get; set; }
        public Guid OnAreaExitID { get; set; }
        public Guid OnAreaHeartbeatID { get; set; }
        public Guid OnAreaUserDefinedID { get; set; }

        public Guid OnCreatureBlockedID { get; set; }
        public Guid OnCreatureCombatRoundEndID { get; set; }
        public Guid OnCreatureConversationID { get; set; }
        public Guid OnCreatureDamagedID { get; set; }
        public Guid OnCreatureDeathID { get; set; }
        public Guid OnCreatureDisturbedID { get; set; }
        public Guid OnCreatureHeartbeatID { get; set; }
        public Guid OnCreaturePerceptionID { get; set; }
        public Guid OnCreaturePhysicalAttackedID { get; set; }
        public Guid OnCreatureRestedID { get; set; }
        public Guid OnCreatureSpawnID { get; set; }
        public Guid OnCreatureSpellCastAtID { get; set; }
        public Guid OnCreatureUserDefinedID { get; set; }

        public PluginRegistration(AppDomain appDomain, IPlugin plugin)
        {
            AppDomain = appDomain;
            Plugin = plugin;
        }
    }
}
