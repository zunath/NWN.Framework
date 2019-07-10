namespace NWN.Framework.Core.Plugin
{
    public interface IPlugin
    {
        string Name { get; }
        string Description { get; }
        void Register();
        void Unregister();

        void OnModuleAcquireItem();
        void OnModuleActivateItem();
        void OnModuleApplyDamage();
        void OnModuleAttack();
        void OnModuleChat();
        void OnModuleCutsceneAbort();
        void OnModuleDeath();
        void OnModuleDying();
        void OnModuleEnter();
        void OnModuleEquipItem();
        void OnModuleExamine();
        void OnModuleHeartbeat();
        void OnModuleLeave();
        void OnModuleLevelUp();
        void OnModuleLoad();
        void OnModuleNWNXChat();
        void OnModuleRespawn();
        void OnModuleRest();
        void OnModuleUnacquireItem();
        void OnModuleUnequipItem();
        void OnModuleUseFeat();
        void OnModuleUserDefined();

        void OnAreaEnter();
        void OnAreaExit();
        void OnAreaHeartbeat();
        void OnAreaUserDefined();

        void OnCreatureBlocked();
        void OnCreatureCombatRoundEnd();
        void OnCreatureConversation();
        void OnCreatureDamaged();
        void OnCreatureDeath();
        void OnCreatureDisturbed();
        void OnCreatureHeartbeat();
        void OnCreaturePerception();
        void OnCreaturePhysicalAttacked();
        void OnCreatureRested();
        void OnCreatureSpawn();
        void OnCreatureSpellCastAt();
        void OnCreatureUserDefined();
    }
}
