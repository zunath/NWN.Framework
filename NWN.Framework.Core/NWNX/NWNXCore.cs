﻿using NWN.Framework.Core.GameObject;

namespace NWN.Framework.Core.NWNX
{
    public static class NWNXCore
    {
        private static string NWNX_INTERNAL_BuildString(string pluginName, string functionName, string operation)
        {
            return "NWNXEE!ABIv2!" + pluginName + "!" + functionName + "!" + operation;
        }

        public static void NWNX_CallFunction(string pluginName, string functionName)
        {
            _.PlaySound(NWNX_INTERNAL_BuildString(pluginName, functionName, "CALL"));
        }

        public static void NWNX_PushArgumentInt(string pluginName, string functionName, int value)
        {
            _.SetLocalInt(Object.OBJECT_INVALID, NWNX_INTERNAL_BuildString(pluginName, functionName, "PUSH"), value);
        }

        public static void NWNX_PushArgumentFloat(string pluginName, string functionName, float value)
        {
            _.SetLocalFloat(Object.OBJECT_INVALID, NWNX_INTERNAL_BuildString(pluginName, functionName, "PUSH"), value);
        }

        public static void NWNX_PushArgumentObject(string pluginName, string functionName, NWObject value)
        {
            _.SetLocalObject(Object.OBJECT_INVALID, NWNX_INTERNAL_BuildString(pluginName, functionName, "PUSH"), value);
        }

        public static void NWNX_PushArgumentString(string pluginName, string functionName, string value)
        {
            _.SetLocalString(Object.OBJECT_INVALID, NWNX_INTERNAL_BuildString(pluginName, functionName, "PUSH"), value);
        }

        public static void NWNX_PushArgumentEffect(string pluginName, string functionName, Effect value)
        {
            _.TagEffect(value, NWNX_INTERNAL_BuildString(pluginName, functionName, "PUSH"));
        }

        public static void NWNX_PushArgumentItemProperty(string pluginName, string functionName, ItemProperty value)
        {
            _.TagItemProperty(value, NWNX_INTERNAL_BuildString(pluginName, functionName, "PUSH"));
        }

        public static int NWNX_GetReturnValueInt(string pluginName, string functionName)
        {
            return _.GetLocalInt(Object.OBJECT_INVALID, NWNX_INTERNAL_BuildString(pluginName, functionName, "POP"));
        }

        public static float NWNX_GetReturnValueFloat(string pluginName, string functionName)
        {
            return _.GetLocalFloat(Object.OBJECT_INVALID, NWNX_INTERNAL_BuildString(pluginName, functionName, "POP"));
        }

        public static Object NWNX_GetReturnValueObject(string pluginName, string functionName)
        {
            return _.GetLocalObject(Object.OBJECT_INVALID, NWNX_INTERNAL_BuildString(pluginName, functionName, "POP"));
        }

        public static string NWNX_GetReturnValueString(string pluginName, string functionName)
        {
            return _.GetLocalString(Object.OBJECT_INVALID, NWNX_INTERNAL_BuildString(pluginName, functionName, "POP"));
        }

        public static Effect NWNX_GetReturnValueEffect(string pluginName, string functionName)
        {
            Effect e = new Effect();
            return _.TagEffect(e, NWNX_INTERNAL_BuildString(pluginName, functionName, "POP"));
        }

        public static ItemProperty NWNX_GetReturnValueItemProperty(string pluginName, string functionName)
        {
            ItemProperty ip = new ItemProperty();
            return _.TagItemProperty(ip, NWNX_INTERNAL_BuildString(pluginName, functionName, "POP"));
        }
    }
}
