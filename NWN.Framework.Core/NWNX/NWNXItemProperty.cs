﻿using System;

namespace NWN.Framework.Core.NWNX
{
    public static class NWNXItemProperty
    {
        private const string NWNX_ItemProperty = "NWNX_ItemProperty";

        public static ItemPropertyUnpacked UnpackIP(ItemProperty ip)
        {
            const string FunctionName = "UnpackIP";

            NWNXCore.NWNX_PushArgumentItemProperty(NWNX_ItemProperty, FunctionName, ip);
            NWNXCore.NWNX_CallFunction(NWNX_ItemProperty, FunctionName);

            var n = new ItemPropertyUnpacked
            {
                ItemPropertyID = NWNXCore.NWNX_GetReturnValueInt(NWNX_ItemProperty, FunctionName),
                Property = NWNXCore.NWNX_GetReturnValueInt(NWNX_ItemProperty, FunctionName),
                SubType = NWNXCore.NWNX_GetReturnValueInt(NWNX_ItemProperty, FunctionName),
                CostTable = NWNXCore.NWNX_GetReturnValueInt(NWNX_ItemProperty, FunctionName),
                CostTableValue = NWNXCore.NWNX_GetReturnValueInt(NWNX_ItemProperty, FunctionName),
                Param1 = NWNXCore.NWNX_GetReturnValueInt(NWNX_ItemProperty, FunctionName),
                Param1Value = NWNXCore.NWNX_GetReturnValueInt(NWNX_ItemProperty, FunctionName),
                UsesPerDay = NWNXCore.NWNX_GetReturnValueInt(NWNX_ItemProperty, FunctionName),
                ChanceToAppear = NWNXCore.NWNX_GetReturnValueInt(NWNX_ItemProperty, FunctionName),
                IsUseable = Convert.ToBoolean(NWNXCore.NWNX_GetReturnValueInt(NWNX_ItemProperty, FunctionName)),
                SpellID = NWNXCore.NWNX_GetReturnValueInt(NWNX_ItemProperty, FunctionName),
                Creator = NWNXCore.NWNX_GetReturnValueObject(NWNX_ItemProperty, FunctionName),
                Tag = NWNXCore.NWNX_GetReturnValueString(NWNX_ItemProperty, FunctionName)
            };

            return n;
        }

        public static ItemProperty PackIP(ItemPropertyUnpacked n)
        {
            const string sFunc = "PackIP";

            NWNXCore.NWNX_PushArgumentString(NWNX_ItemProperty, sFunc, n.Tag);
            NWNXCore.NWNX_PushArgumentObject(NWNX_ItemProperty, sFunc, n.Creator);
            NWNXCore.NWNX_PushArgumentInt(NWNX_ItemProperty, sFunc, n.SpellID);
            NWNXCore.NWNX_PushArgumentInt(NWNX_ItemProperty, sFunc, n.IsUseable ? 1 : 0);
            NWNXCore.NWNX_PushArgumentInt(NWNX_ItemProperty, sFunc, n.ChanceToAppear);
            NWNXCore.NWNX_PushArgumentInt(NWNX_ItemProperty, sFunc, n.UsesPerDay);
            NWNXCore.NWNX_PushArgumentInt(NWNX_ItemProperty, sFunc, n.Param1Value);
            NWNXCore.NWNX_PushArgumentInt(NWNX_ItemProperty, sFunc, n.Param1);
            NWNXCore.NWNX_PushArgumentInt(NWNX_ItemProperty, sFunc, n.CostTableValue);
            NWNXCore.NWNX_PushArgumentInt(NWNX_ItemProperty, sFunc, n.CostTable);
            NWNXCore.NWNX_PushArgumentInt(NWNX_ItemProperty, sFunc, n.SubType);
            NWNXCore.NWNX_PushArgumentInt(NWNX_ItemProperty, sFunc, n.Property);
            NWNXCore.NWNX_PushArgumentInt(NWNX_ItemProperty, sFunc, n.ItemPropertyID);

            NWNXCore.NWNX_CallFunction(NWNX_ItemProperty, sFunc);
            return NWNXCore.NWNX_GetReturnValueItemProperty(NWNX_ItemProperty, sFunc);
        }

    }
}
