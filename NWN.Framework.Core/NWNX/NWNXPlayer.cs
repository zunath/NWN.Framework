using NWN.Framework.Core.GameObject;

namespace NWN.Framework.Core.NWNX
{
    public static class NWNXPlayer
    {
        private const string NWNX_Player = "NWNX_Player";

        /// <summary> 
        /// Force display placeable examine window for player
        /// If used on a placeable in a different area than the player, the portait will not be shown.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="placeable"></param>
        public static void ForcePlaceableExamineWindow(NWPlayer player, NWPlaceable placeable)
        {
            string sFunc = "ForcePlaceableExamineWindow";
            NWNXCore.NWNX_PushArgumentObject(NWNX_Player, sFunc, placeable.Object);
            NWNXCore.NWNX_PushArgumentObject(NWNX_Player, sFunc, player.Object);

            NWNXCore.NWNX_CallFunction(NWNX_Player, sFunc);
        }

        /// <summary>
        /// Force opens the target object's inventory for the player.
        /// A few notes about this function:
        /// - If the placeable is in a different area than the player, the portrait will not be shown
        /// - The placeable's open/close animations will be played
        /// - Clicking the 'close' button will cause the player to walk to the placeable;
        ///     If the placeable is in a different area, the player will just walk to the edge
        ///     of the current area and stop. This action can be cancelled manually.
        /// - Walking will close the placeable automatically.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="placeable"></param>
        public static void ForcePlaceableInventoryWindow(NWPlayer player, NWPlaceable placeable)
        {
            string sFunc = "ForcePlaceableInventoryWindow";
            NWNXCore.NWNX_PushArgumentObject(NWNX_Player, sFunc, placeable);
            NWNXCore.NWNX_PushArgumentObject(NWNX_Player, sFunc, player);

            NWNXCore.NWNX_CallFunction(NWNX_Player, sFunc);
        }

        /// <summary>
        /// Starts displaying a timing bar.
        /// Will run a script at the end of the timing bar, if specified.
        /// </summary>
        /// <param name="creature">The creature who will see the timing bar.</param>
        /// <param name="seconds">How long the timing bar should come on screen.</param>
        /// <param name="script">The script to run at the end of the timing bar.</param>
        public static void StartGuiTimingBar(NWCreature creature, float seconds, string script)
        {
            // only one timing bar at a time!
            if (_.GetLocalInt(creature.Object, "NWNX_PLAYER_GUI_TIMING_ACTIVE") == 1)
                return;

            string sFunc = "StartGuiTimingBar";
            NWNXCore.NWNX_PushArgumentFloat(NWNX_Player, sFunc, seconds);
            NWNXCore.NWNX_PushArgumentObject(NWNX_Player, sFunc, creature.Object);

            NWNXCore.NWNX_CallFunction(NWNX_Player, sFunc);

            int id = _.GetLocalInt(creature.Object, "NWNX_PLAYER_GUI_TIMING_ID") + 1;
            _.SetLocalInt(creature.Object, "NWNX_PLAYER_GUI_TIMING_ACTIVE", id);
            _.SetLocalInt(creature.Object, "NWNX_PLAYER_GUI_TIMING_ID", id);

            _.DelayCommand(seconds, () =>
            {
                StopGuiTimingBar(creature, script, -1);
            });
        }

        /// <summary>
        /// Stops displaying a timing bar.
        /// Runs a script if specified.
        /// </summary>
        /// <param name="creature">The creature's timing bar to stop.</param>
        /// <param name="script">The script to run once ended.</param>
        /// <param name="id">ID number of this timing bar.</param>
        public static void StopGuiTimingBar(NWCreature creature, string script, int id)
        {
            int activeId = _.GetLocalInt(creature.Object, "NWNX_PLAYER_GUI_TIMING_ACTIVE");
            // Either the timing event was never started, or it already finished.
            if (activeId == 0)
                return;

            // If id != -1, we ended up here through DelayCommand. Make sure it's for the right ID
            if (id != -1 && id != activeId)
                return;

            _.DeleteLocalInt(creature.Object, "NWNX_PLAYER_GUI_TIMING_ACTIVE");

            string sFunc = "StopGuiTimingBar";
            NWNXCore.NWNX_PushArgumentObject(NWNX_Player, sFunc, creature.Object);
            NWNXCore.NWNX_CallFunction(NWNX_Player, sFunc);

            if (!string.IsNullOrWhiteSpace(script))
            {
                _.ExecuteScript(script, creature.Object);
            }
        }


        /// <summary>
        /// Stops displaying a timing bar.
        /// Runs a script if specified.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="script"></param>
        public static void StopGuiTimingBar(NWPlayer player, string script)
        {
            StopGuiTimingBar(player, script, -1);
        }

        /// <summary>
        /// Sets whether the player should always walk when given movement commands.
        /// If true, clicking on the ground or using WASD will trigger walking instead of running.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="bWalk"></param>
        public static void SetAlwaysWalk(NWPlayer player, int bWalk)
        {
            string sFunc = "SetAlwaysWalk";
            NWNXCore.NWNX_PushArgumentInt(NWNX_Player, sFunc, bWalk);
            NWNXCore.NWNX_PushArgumentObject(NWNX_Player, sFunc, player.Object);

            NWNXCore.NWNX_CallFunction(NWNX_Player, sFunc);
        }

        /// <summary>
        /// Gets the player's quickbar slot info
        /// </summary>
        /// <param name="player"></param>
        /// <param name="slot"></param>
        /// <returns></returns>
        public static QuickBarSlot GetQuickBarSlot(NWPlayer player, int slot)
        {
            string sFunc = "GetQuickBarSlot";
            QuickBarSlot qbs = new QuickBarSlot();

            NWNXCore.NWNX_PushArgumentInt(NWNX_Player, sFunc, slot);
            NWNXCore.NWNX_PushArgumentObject(NWNX_Player, sFunc, player.Object);
            NWNXCore.NWNX_CallFunction(NWNX_Player, sFunc);

            qbs.Associate = (NWNXCore.NWNX_GetReturnValueObject(NWNX_Player, sFunc));
            qbs.AssociateType = NWNXCore.NWNX_GetReturnValueInt(NWNX_Player, sFunc);
            qbs.DomainLevel = NWNXCore.NWNX_GetReturnValueInt(NWNX_Player, sFunc);
            qbs.MetaType = NWNXCore.NWNX_GetReturnValueInt(NWNX_Player, sFunc);
            qbs.INTParam1 = NWNXCore.NWNX_GetReturnValueInt(NWNX_Player, sFunc);
            qbs.ToolTip = NWNXCore.NWNX_GetReturnValueString(NWNX_Player, sFunc);
            qbs.CommandLine = NWNXCore.NWNX_GetReturnValueString(NWNX_Player, sFunc);
            qbs.CommandLabel = NWNXCore.NWNX_GetReturnValueString(NWNX_Player, sFunc);
            qbs.Resref = NWNXCore.NWNX_GetReturnValueString(NWNX_Player, sFunc);
            qbs.MultiClass = NWNXCore.NWNX_GetReturnValueInt(NWNX_Player, sFunc);
            qbs.ObjectType = (QuickBarSlotType)NWNXCore.NWNX_GetReturnValueInt(NWNX_Player, sFunc);
            qbs.SecondaryItem = (NWNXCore.NWNX_GetReturnValueObject(NWNX_Player, sFunc));
            qbs.Item = (NWNXCore.NWNX_GetReturnValueObject(NWNX_Player, sFunc));

            return qbs;
        }

        /// <summary>
        /// Sets a player's quickbar slot
        /// </summary>
        /// <param name="player"></param>
        /// <param name="slot"></param>
        /// <param name="qbs"></param>
        public static void SetQuickBarSlot(NWPlayer player, int slot, QuickBarSlot qbs)
        {
            string sFunc = "SetQuickBarSlot";

            NWNXCore.NWNX_PushArgumentObject(NWNX_Player, sFunc, qbs.Item.Object);
            NWNXCore.NWNX_PushArgumentObject(NWNX_Player, sFunc, qbs.SecondaryItem.Object);
            NWNXCore.NWNX_PushArgumentInt(NWNX_Player, sFunc, (int)qbs.ObjectType);
            NWNXCore.NWNX_PushArgumentInt(NWNX_Player, sFunc, qbs.MultiClass);
            NWNXCore.NWNX_PushArgumentString(NWNX_Player, sFunc, qbs.Resref);
            NWNXCore.NWNX_PushArgumentString(NWNX_Player, sFunc, qbs.CommandLabel);
            NWNXCore.NWNX_PushArgumentString(NWNX_Player, sFunc, qbs.CommandLine);
            NWNXCore.NWNX_PushArgumentString(NWNX_Player, sFunc, qbs.ToolTip);
            NWNXCore.NWNX_PushArgumentInt(NWNX_Player, sFunc, qbs.INTParam1);
            NWNXCore.NWNX_PushArgumentInt(NWNX_Player, sFunc, qbs.MetaType);
            NWNXCore.NWNX_PushArgumentInt(NWNX_Player, sFunc, qbs.DomainLevel);
            NWNXCore.NWNX_PushArgumentInt(NWNX_Player, sFunc, qbs.AssociateType);
            NWNXCore.NWNX_PushArgumentObject(NWNX_Player, sFunc, qbs.Associate.Object);

            NWNXCore.NWNX_PushArgumentInt(NWNX_Player, sFunc, slot);
            NWNXCore.NWNX_PushArgumentObject(NWNX_Player, sFunc, player.Object);
            NWNXCore.NWNX_CallFunction(NWNX_Player, sFunc);
        }


        /// <summary>
        /// Get the name of the .bic file associated with the player's character.
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public static string GetBicFileName(NWPlayer player)
        {
            string sFunc = "GetBicFileName";
            NWNXCore.NWNX_PushArgumentObject(NWNX_Player, sFunc, player.Object);
            NWNXCore.NWNX_CallFunction(NWNX_Player, sFunc);
            return NWNXCore.NWNX_GetReturnValueString(NWNX_Player, sFunc);
        }

        /// <summary>
        /// Plays the VFX at the target position in current area for the given player only
        /// </summary>
        /// <param name="player"></param>
        /// <param name="effectId"></param>
        /// <param name="position"></param>
        public static void ShowVisualEffect(NWPlayer player, int effectId, Vector position)
        {
            string sFunc = "ShowVisualEffect";
            NWNXCore.NWNX_PushArgumentFloat(NWNX_Player, sFunc, position.m_X);
            NWNXCore.NWNX_PushArgumentFloat(NWNX_Player, sFunc, position.m_Y);
            NWNXCore.NWNX_PushArgumentFloat(NWNX_Player, sFunc, position.m_Z);
            NWNXCore.NWNX_PushArgumentInt(NWNX_Player, sFunc, effectId);
            NWNXCore.NWNX_PushArgumentObject(NWNX_Player, sFunc, player);

            NWNXCore.NWNX_CallFunction(NWNX_Player, sFunc);
        }

        /// <summary>
        /// Changes the daytime music track for the given player only
        /// </summary>
        /// <param name="player"></param>
        /// <param name="track"></param>
        public static void MusicBackgroundChangeDay(NWPlayer player, int track)
        {
            string sFunc = "ChangeBackgroundMusic";
            NWNXCore.NWNX_PushArgumentInt(NWNX_Player, sFunc, track);
            NWNXCore.NWNX_PushArgumentInt(NWNX_Player, sFunc, _.TRUE); // bool day = TRUE
            NWNXCore.NWNX_PushArgumentObject(NWNX_Player, sFunc, player);

            NWNXCore.NWNX_CallFunction(NWNX_Player, sFunc);
        }

        /// <summary>
        /// Changes the nighttime music track for the given player only
        /// </summary>
        /// <param name="player"></param>
        /// <param name="track"></param>
        public static void MusicBackgroundChangeNight(NWPlayer player, int track)
        {
            string sFunc = "ChangeBackgroundMusic";
            NWNXCore.NWNX_PushArgumentInt(NWNX_Player, sFunc, track);
            NWNXCore.NWNX_PushArgumentInt(NWNX_Player, sFunc, _.FALSE); // bool day = FALSE
            NWNXCore.NWNX_PushArgumentObject(NWNX_Player, sFunc, player);

            NWNXCore.NWNX_CallFunction(NWNX_Player, sFunc);
        }

        /// <summary>
        /// Starts the background music for the given player only
        /// </summary>
        /// <param name="player"></param>
        public static void MusicBackgroundStart(NWPlayer player)
        {
            string sFunc = "PlayBackgroundMusic";
            NWNXCore.NWNX_PushArgumentInt(NWNX_Player, sFunc, _.TRUE); // bool play = TRUE
            NWNXCore.NWNX_PushArgumentObject(NWNX_Player, sFunc, player);

            NWNXCore.NWNX_CallFunction(NWNX_Player, sFunc);
        }

        /// <summary>
        /// Stops the background music for the given player only
        /// </summary>
        /// <param name="player"></param>
        public static void MusicBackgroundStop(NWPlayer player)
        {
            string sFunc = "PlayBackgroundMusic";
            NWNXCore.NWNX_PushArgumentInt(NWNX_Player, sFunc, _.FALSE); // bool play = FALSE
            NWNXCore.NWNX_PushArgumentObject(NWNX_Player, sFunc, player);

            NWNXCore.NWNX_CallFunction(NWNX_Player, sFunc);
        }

        /// <summary>
        /// Changes the battle music track for the given player only
        /// </summary>
        /// <param name="player"></param>
        /// <param name="track"></param>
        public static void MusicBattleChange(NWPlayer player, int track)
        {
            string sFunc = "ChangeBattleMusic";
            NWNXCore.NWNX_PushArgumentInt(NWNX_Player, sFunc, track);
            NWNXCore.NWNX_PushArgumentObject(NWNX_Player, sFunc, player);

            NWNXCore.NWNX_CallFunction(NWNX_Player, sFunc);
        }

        /// <summary>
        /// Starts the battle music for the given player only
        /// </summary>
        /// <param name="player"></param>
        public static void MusicBattleStart(NWPlayer player)
        {
            string sFunc = "PlayBattleMusic";
            NWNXCore.NWNX_PushArgumentInt(NWNX_Player, sFunc, _.TRUE); // bool play = TRUE
            NWNXCore.NWNX_PushArgumentObject(NWNX_Player, sFunc, player);

            NWNXCore.NWNX_CallFunction(NWNX_Player, sFunc);
        }

        /// <summary>
        /// Stops the background music for the given player only
        /// </summary>
        /// <param name="player"></param>
        public static void MusicBattleStop(NWPlayer player)
        {
            string sFunc = "PlayBattleMusic";
            NWNXCore.NWNX_PushArgumentInt(NWNX_Player, sFunc, _.FALSE); // bool play = FALSE
            NWNXCore.NWNX_PushArgumentObject(NWNX_Player, sFunc, player);

            NWNXCore.NWNX_CallFunction(NWNX_Player, sFunc);
        }

        /// <summary>
        /// Play a sound at the location of target for the given player only
        /// If target is OBJECT_INVALID the sound will play at the location of the player
        /// </summary>
        /// <param name="player"></param>
        /// <param name="sound"></param>
        /// <param name="target"></param>
        public static void PlaySound(NWPlayer player, string sound, NWObject target)
        {
            string sFunc = "PlaySound";
            NWNXCore.NWNX_PushArgumentObject(NWNX_Player, sFunc, target);
            NWNXCore.NWNX_PushArgumentString(NWNX_Player, sFunc, sound);
            NWNXCore.NWNX_PushArgumentObject(NWNX_Player, sFunc, player);

            NWNXCore.NWNX_CallFunction(NWNX_Player, sFunc);
        }

        /// <summary>
        /// Toggle a placeable's usable flag for the given player only
        /// </summary>
        /// <param name="player"></param>
        /// <param name="placeable"></param>
        /// <param name="isUseable"></param>
        public static void SetPlaceableUseable(NWPlayer player, NWPlaceable placeable, bool isUseable)
        {
            string sFunc = "SetPlaceableUsable";
            NWNXCore.NWNX_PushArgumentInt(NWNX_Player, sFunc, isUseable ? _.TRUE : _.FALSE);
            NWNXCore.NWNX_PushArgumentObject(NWNX_Player, sFunc, placeable);
            NWNXCore.NWNX_PushArgumentObject(NWNX_Player, sFunc, player);

            NWNXCore.NWNX_CallFunction(NWNX_Player, sFunc);
        }

        /// <summary>
        /// Override player's rest duration
        /// Duration is in milliseconds, 1000 = 1 second
        /// Minimum duration of 10ms
        /// -1 clears the override
        /// </summary>
        /// <param name="player"></param>
        /// <param name="duration"></param>
        public static void SetRestDuration(NWPlayer player, int duration)
        {
            string sFunc = "SetRestDuration";
            NWNXCore.NWNX_PushArgumentInt(NWNX_Player, sFunc, duration);
            NWNXCore.NWNX_PushArgumentObject(NWNX_Player, sFunc, player);

            NWNXCore.NWNX_CallFunction(NWNX_Player, sFunc);
        }


        /// <summary>
        /// Apply visualeffect to target that only player can see
        /// Note: Only works with instant effects: VFX_COM_*, VFX_FNF_*, VFX_IMP_*
        /// </summary>
        /// <param name="player"></param>
        /// <param name="target"></param>
        /// <param name="visualeffect"></param>
        public static void ApplyInstantVisualEffectToObject(NWPlayer player, NWObject target, int visualeffect)
        {
            string sFunc = "ApplyInstantVisualEffectToObject";
            NWNXCore.NWNX_PushArgumentInt(NWNX_Player, sFunc, visualeffect);
            NWNXCore.NWNX_PushArgumentObject(NWNX_Player, sFunc, target);
            NWNXCore.NWNX_PushArgumentObject(NWNX_Player, sFunc, player);

            NWNXCore.NWNX_CallFunction(NWNX_Player, sFunc);
        }

        /// <summary>
        /// // Refreshes the players character sheet
        /// Note: You may need to use DelayCommand if you're manipulating values
        /// through nwnx and forcing a UI refresh, 0.5s seemed to be fine
        /// </summary>
        /// <param name="player"></param>
        public static void UpdateCharacterSheet(NWPlayer player)
        {
            string sFunc = "UpdateCharacterSheet";
            NWNXCore.NWNX_PushArgumentObject(NWNX_Player, sFunc, player);

            NWNXCore.NWNX_CallFunction(NWNX_Player, sFunc);
        }

        /// <summary>
        /// Allows player to open target's inventory
        /// Target must be a creature or another player
        /// Note: only works if player and target are in the same area
        /// </summary>
        /// <param name="player"></param>
        /// <param name="target"></param>
        /// <param name="open"></param>
        public static void OpenInventory(NWPlayer player, NWObject target, bool open = true)
        {
            string sFunc = "OpenInventory";

            NWNXCore.NWNX_PushArgumentInt(NWNX_Player, sFunc, open ? 1 : 0);
            NWNXCore.NWNX_PushArgumentObject(NWNX_Player, sFunc, target);
            NWNXCore.NWNX_PushArgumentObject(NWNX_Player, sFunc, player);

            NWNXCore.NWNX_CallFunction(NWNX_Player, sFunc);
        }

    }
}
