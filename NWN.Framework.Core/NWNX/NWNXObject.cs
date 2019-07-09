using NWN.Framework.Core.GameObject;

namespace NWN.Framework.Core.NWNX
{
    public static class NWNXObject
    {
        private const string NWNX_Object = "NWNX_Object";

        /// <summary>
        /// Gets the count of all local variables on the provided object.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static int GetLocalVariableCount(NWObject obj)
        {
            string sFunc = "GetLocalVariableCount";

            NWNXCore.NWNX_PushArgumentObject(NWNX_Object, sFunc, obj);
            NWNXCore.NWNX_CallFunction(NWNX_Object, sFunc);

            return NWNXCore.NWNX_GetReturnValueInt(NWNX_Object, sFunc);
        }

        /// <summary>
        /// Returns a local variable at the specified index.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static LocalVariable GetLocalVariable(NWObject obj, int index)
        {
            string sFunc = "GetLocalVariable";

            NWNXCore.NWNX_PushArgumentInt(NWNX_Object, sFunc, index);
            NWNXCore.NWNX_PushArgumentObject(NWNX_Object, sFunc, obj);
            NWNXCore.NWNX_CallFunction(NWNX_Object, sFunc);

            LocalVariable var = new LocalVariable();
            var.Key = NWNXCore.NWNX_GetReturnValueString(NWNX_Object, sFunc);
            var.Type = (LocalVariableType)NWNXCore.NWNX_GetReturnValueInt(NWNX_Object, sFunc);
            return var;
        }

        /// <summary>
        /// Returns an object from the provided object ID.
        /// This is the counterpart to ObjectToString.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static NWObject StringToObject(string id)
        {
            string sFunc = "StringToObject";

            NWNXCore.NWNX_PushArgumentString(NWNX_Object, sFunc, id);
            NWNXCore.NWNX_CallFunction(NWNX_Object, sFunc);
            return NWNXCore.NWNX_GetReturnValueObject(NWNX_Object, sFunc);
        }

        /// <summary>
        /// Set the provided object's position to the provided vector.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="pos"></param>
        public static void SetPosition(NWObject obj, Vector pos)
        {
            string sFunc = "SetPosition";

            NWNXCore.NWNX_PushArgumentFloat(NWNX_Object, sFunc, pos.m_X);
            NWNXCore.NWNX_PushArgumentFloat(NWNX_Object, sFunc, pos.m_Y);
            NWNXCore.NWNX_PushArgumentFloat(NWNX_Object, sFunc, pos.m_Z);
            NWNXCore.NWNX_PushArgumentObject(NWNX_Object, sFunc, obj);
            NWNXCore.NWNX_CallFunction(NWNX_Object, sFunc);

        }

        /// <summary>
        /// Sets the provided object's current hit points to the provided value.
        /// </summary>
        /// <param name="creature"></param>
        /// <param name="hp"></param>
        public static void SetCurrentHitPoints(NWCreature creature, int hp)
        {
            string sFunc = "SetCurrentHitPoints";

            NWNXCore.NWNX_PushArgumentInt(NWNX_Object, sFunc, hp);
            NWNXCore.NWNX_PushArgumentObject(NWNX_Object, sFunc, creature);

            NWNXCore.NWNX_CallFunction(NWNX_Object, sFunc);
        }

        /// <summary>
        /// Set object's maximum hit points; will not work on PCs.
        /// </summary>
        /// <param name="creature"></param>
        /// <param name="hp"></param>
        public static void SetMaxHitPoints(NWCreature creature, int hp)
        {
            string sFunc = "SetMaxHitPoints";

            NWNXCore.NWNX_PushArgumentInt(NWNX_Object, sFunc, hp);
            NWNXCore.NWNX_PushArgumentObject(NWNX_Object, sFunc, creature);

            NWNXCore.NWNX_CallFunction(NWNX_Object, sFunc);
        }

        /// <summary>
        /// Serialize the full object (including locals, inventory, etc) to base64 string
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string Serialize(NWObject obj)
        {
            string sFunc = "Serialize";

            NWNXCore.NWNX_PushArgumentObject(NWNX_Object, sFunc, obj);

            NWNXCore.NWNX_CallFunction(NWNX_Object, sFunc);
            return NWNXCore.NWNX_GetReturnValueString(NWNX_Object, sFunc);
        }

        /// <summary>
        /// Deserialize the object. The object will be created outside of the world and
        /// needs to be manually positioned at a location/inventory.
        /// </summary>
        /// <param name="serialized"></param>
        /// <returns></returns>
        public static NWObject Deserialize(string serialized)
        {
            string sFunc = "Deserialize";

            NWNXCore.NWNX_PushArgumentString(NWNX_Object, sFunc, serialized);

            NWNXCore.NWNX_CallFunction(NWNX_Object, sFunc);
            return NWNXCore.NWNX_GetReturnValueObject(NWNX_Object, sFunc);
        }

        /// <summary>
        /// Returns the dialog resref of the object.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string GetDialogResref(NWObject obj)
        {
            string sFunc = "GetDialogResref";

            NWNXCore.NWNX_PushArgumentObject(NWNX_Object, sFunc, obj);

            NWNXCore.NWNX_CallFunction(NWNX_Object, sFunc);
            return NWNXCore.NWNX_GetReturnValueString(NWNX_Object, sFunc);
        }

        /// <summary>
        /// Sets the dialog resref of the object.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="dialog"></param>
        public static void SetDialogResref(NWObject obj, string dialog)
        {
            string sFunc = "SetDialogResref";

            NWNXCore.NWNX_PushArgumentString(NWNX_Object, sFunc, dialog);
            NWNXCore.NWNX_PushArgumentObject(NWNX_Object, sFunc, obj);

            NWNXCore.NWNX_CallFunction(NWNX_Object, sFunc);
        }

        /// <summary>
        /// Set obj's appearance. Will not update for PCs until they
        /// re-enter the area.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="app"></param>
        public static void SetAppearance(NWObject obj, int app)
        {
            string sFunc = "SetAppearance";

            NWNXCore.NWNX_PushArgumentInt(NWNX_Object, sFunc, app);
            NWNXCore.NWNX_PushArgumentObject(NWNX_Object, sFunc, obj);

            NWNXCore.NWNX_CallFunction(NWNX_Object, sFunc);
        }

        /// <summary>
        /// Get obj's appearance
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static int GetAppearance(NWObject obj)
        {
            string sFunc = "GetAppearance";

            NWNXCore.NWNX_PushArgumentObject(NWNX_Object, sFunc, obj);

            NWNXCore.NWNX_CallFunction(NWNX_Object, sFunc);
            return NWNXCore.NWNX_GetReturnValueInt(NWNX_Object, sFunc);
        }

        /// <summary>
        /// Return true if obj has visual effect nVFX applied to it
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="nVFX"></param>
        /// <returns></returns>
        public static bool GetHasVisualEffect(NWObject obj, int nVFX)
        {
            string sFunc = "GetHasVisualEffect";

            NWNXCore.NWNX_PushArgumentInt(NWNX_Object, sFunc, nVFX);
            NWNXCore.NWNX_PushArgumentObject(NWNX_Object, sFunc, obj);

            NWNXCore.NWNX_CallFunction(NWNX_Object, sFunc);

            return NWNXCore.NWNX_GetReturnValueInt(NWNX_Object, sFunc) == _.TRUE;
        }

        /// <summary>
        /// Return true if an item of baseitem type can fit in object's inventory
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="baseitem"></param>
        /// <returns></returns>
        public static bool CheckFit(NWItem item, int baseitem)
        {
            string sFunc = "CheckFit";

            NWNXCore.NWNX_PushArgumentInt(NWNX_Object, sFunc, baseitem);
            NWNXCore.NWNX_PushArgumentObject(NWNX_Object, sFunc, item);

            NWNXCore.NWNX_CallFunction(NWNX_Object, sFunc);

            return NWNXCore.NWNX_GetReturnValueInt(NWNX_Object, sFunc) == _.TRUE;
        }

        /// <summary>
        /// Return damage immunity (in percent) against given damage type
        /// Use DAMAGE_TYPE_* constants for damageType
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="damageType"></param>
        /// <returns></returns>
        public static int GetDamageImmunity(NWObject obj, int damageType)
        {
            string sFunc = "GetDamageImmunity";

            NWNXCore.NWNX_PushArgumentInt(NWNX_Object, sFunc, damageType);
            NWNXCore.NWNX_PushArgumentObject(NWNX_Object, sFunc, obj);

            NWNXCore.NWNX_CallFunction(NWNX_Object, sFunc);

            return NWNXCore.NWNX_GetReturnValueInt(NWNX_Object, sFunc);
        }

        /// <summary>
        /// Add or move obj to area at pos
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="area"></param>
        /// <param name="pos"></param>
        public static void AddToArea(NWObject obj, NWArea area, Vector pos)
        {
            string sFunc = "AddToArea";

            NWNXCore.NWNX_PushArgumentFloat(NWNX_Object, sFunc, pos.m_Z);
            NWNXCore.NWNX_PushArgumentFloat(NWNX_Object, sFunc, pos.m_Y);
            NWNXCore.NWNX_PushArgumentFloat(NWNX_Object, sFunc, pos.m_X);
            NWNXCore.NWNX_PushArgumentObject(NWNX_Object, sFunc, area);
            NWNXCore.NWNX_PushArgumentObject(NWNX_Object, sFunc, obj);
            NWNXCore.NWNX_CallFunction(NWNX_Object, sFunc);
        }


    }
}
