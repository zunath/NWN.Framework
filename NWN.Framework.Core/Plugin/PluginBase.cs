using System;
using NWN.Framework.Core.Messaging;

namespace NWN.Framework.Core.Plugin
{
    public abstract class PluginBase: MarshalByRefObject, IPlugin
    {
        public abstract string Name { get; }
        public abstract string Description { get; }
        public abstract void Register(Hub hub);
        public abstract void Unregister();

        public virtual void OnModuleAcquireItem() { }
        public virtual void OnModuleActivateItem() { }
        public virtual void OnModuleApplyDamage() { }
        public virtual void OnModuleAttack() { }
        public virtual void OnModuleChat() { }
        public virtual void OnModuleCutsceneAbort() { }
        public virtual void OnModuleDeath() { }
        public virtual void OnModuleDying() { }
        public virtual void OnModuleEnter() { }
        public virtual void OnModuleEquipItem() { }
        public virtual void OnModuleExamine() { }
        public virtual void OnModuleHeartbeat() { }
        public virtual void OnModuleLeave() { }
        public virtual void OnModuleLevelUp() { }
        public virtual void OnModuleLoad() { }
        public virtual void OnModuleNWNXChat() { }
        public virtual void OnModuleRespawn() { }
        public virtual void OnModuleRest() { }
        public virtual void OnModuleUnacquireItem() { }
        public virtual void OnModuleUnequipItem() { }
        public virtual void OnModuleUseFeat() { }
        public virtual void OnModuleUserDefined() { }
        
        public virtual void OnAreaEnter() { }
        public virtual void OnAreaExit() { }
        public virtual void OnAreaHeartbeat() { }
        public virtual void OnAreaUserDefined() { }

        public virtual void OnCreatureBlocked() { }
        public virtual void OnCreatureCombatRoundEnd() { }
        public virtual void OnCreatureConversation() { }
        public virtual void OnCreatureDamaged() { }
        public virtual void OnCreatureDeath() { }
        public virtual void OnCreatureDisturbed() { }
        public virtual void OnCreatureHeartbeat() { }
        public virtual void OnCreaturePerception() { }
        public virtual void OnCreaturePhysicalAttacked() { }
        public virtual void OnCreatureRested() { }
        public virtual void OnCreatureSpawn() { }
        public virtual void OnCreatureSpellCastAt() { }
        public virtual void OnCreatureUserDefined() { }
    }
}
