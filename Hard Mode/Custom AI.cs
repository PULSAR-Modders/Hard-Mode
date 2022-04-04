﻿using HarmonyLib;

namespace Hard_Mode
{
    internal class Custom_AI
    {
        [HarmonyPatch(typeof(PLGlobal), "SetupClassDefaultData")]
        static void Postfix(ref AIDataIndividual dataInv, int classID, bool enemyAI = false)
        {
            if (classID == 0) return;
            dataInv.Priorities.Clear();
            switch (classID)
            {
                case 1:
                    AIPriority aipriority00 = new AIPriority(AIPriorityType.E_TWEAK, 82, 3);
                    dataInv.Priorities.Add(aipriority00);
                    AIPriority aipriority01 = new AIPriority(AIPriorityType.E_CLASS_MAIN, 32, 0);
                    dataInv.Priorities.Add(aipriority01);
                    aipriority01.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 22, 0, false, 50));
                    aipriority01.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 20, 0, false, 50));
                    aipriority01.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_ALERT_LEVEL, 2, 2, false, 50));
                    aipriority01.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 22, 4, true, 50));
                    AIPriority aipriority02 = new AIPriority(AIPriorityType.E_MAIN, 5, 1);
                    dataInv.Priorities.Add(aipriority02);
                    if (!enemyAI)
                    {
                        aipriority02.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CAPTAINS_ORDER, 3, 4, false, 50));
                    }
                    aipriority02.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 84, 3, true, 50));
                    aipriority02.Metadata.Add(new PLPriorityMetadata_Float(3, 1, "Range (m)"));
                    AIPriority aipriority03 = new AIPriority(AIPriorityType.E_CLASS_MAIN, 67, 0);
                    dataInv.Priorities.Add(aipriority03);
                    aipriority03.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_ALERT_LEVEL, 0, 0, true, 50));
                    aipriority03.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 1, 0, true, 50));
                    aipriority03.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 20, 0, false, 50));
                    aipriority03.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 72, 0, true, 8));
                    aipriority03.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 71, 0, true, 2));
                    aipriority03.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 89, 1, false, 45));
                    AIPriority aipriority04 = new AIPriority(AIPriorityType.E_MAIN, 66, 3);
                    dataInv.Priorities.Add(aipriority04);
                    aipriority04.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 69, 0, true, 50));
                    aipriority04.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 70, 0, true, 50));
                    AIPriority aipriority05 = new AIPriority(AIPriorityType.E_MAIN, 65, 0);
                    dataInv.Priorities.Add(aipriority05);
                    aipriority05.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_ALERT_LEVEL, 2, 0, true, 50));
                    if (!enemyAI)
                    {
                        aipriority05.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CAPTAINS_ORDER, 0, 3, false, 50));
                    }
                    AIPriority aipriority06 = new AIPriority(AIPriorityType.E_MAIN, 64, 0);
                    dataInv.Priorities.Add(aipriority06);
                    aipriority06.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 67, 4, false, 50));
                    AIPriority aipriority07 = new AIPriority(AIPriorityType.E_MAIN, 63, 2);
                    dataInv.Priorities.Add(aipriority07);
                    aipriority07.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 65, 0, true, 50));
                    aipriority07.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 66, 0, true, 50));
                    AIPriority aipriority08 = new AIPriority(AIPriorityType.E_MAIN, 60, 3);
                    dataInv.Priorities.Add(aipriority08);
                    aipriority08.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 60, 0, true, 0));
                    aipriority08.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 61, 0, true, 0));
                    aipriority08.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 68, 0, false, 0));
                    aipriority08.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_ALERT_LEVEL, 0, 0, false, 0));
                    AIPriority aipriority09 = new AIPriority(AIPriorityType.E_MAIN, 58, 0);
                    dataInv.Priorities.Add(aipriority09);
                    aipriority09.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 57, 0, true, 50));
                    aipriority09.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 56, 3, false, 50));
                    AIPriority aipriority010 = new AIPriority(AIPriorityType.E_MAIN, 49, 0);
                    dataInv.Priorities.Add(aipriority010);
                    aipriority010.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 52, 0, true, 50));
                    aipriority010.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 51, 5, false, 50));
                    aipriority010.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 12, 4, false, 2));
                    AIPriority aipriority011 = new AIPriority(AIPriorityType.E_MAIN, 11, 0);
                    dataInv.Priorities.Add(aipriority011);
                    aipriority011.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 20, 0, false, 50));
                    aipriority011.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 52, 0, false, 50));
                    aipriority011.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 13, 3, true, 30));
                    aipriority011.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 13, 2, true, 55));
                    aipriority011.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 13, 1, true, 80));
                    AIPriority aipriority012 = new AIPriority(AIPriorityType.E_CLASS_MAIN, 10, 0);
                    dataInv.Priorities.Add(aipriority012);
                    aipriority012.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 20, 0, false, 50));
                    aipriority012.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 83, 0, false, 50));
                    aipriority012.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 17, 2, true, 50));
                    aipriority012.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 18, 2, true, 50));
                    aipriority012.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 19, 2, true, 50));
                    AIPriority aipriority013 = new AIPriority(AIPriorityType.E_TWEAK, 8, 0);
                    dataInv.Priorities.Add(aipriority013);
                    AIPriority aipriority014 = new AIPriority(AIPriorityType.E_CLASS_MAIN, 6, 0);
                    dataInv.Priorities.Add(aipriority014);
                    aipriority014.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 20, 3, false, 50));
                    aipriority014.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 26, 0, true, 50));
                    aipriority014.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 58, 4, false, 50));
                    if (!enemyAI)
                    {
                        aipriority014.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CAPTAINS_ORDER, 5, 5, false, 50));
                    }
                    if (!enemyAI)
                    {
                        aipriority014.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CAPTAINS_ORDER, 6, 3, false, 50));
                    }
                    AIPriority aipriority015 = new AIPriority(AIPriorityType.E_MAIN, 4, 0);
                    dataInv.Priorities.Add(aipriority015);
                    aipriority015.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 20, 0, false, 0));
                    aipriority015.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 22, 2, false, 50));
                    AIPriority aipriority016 = new AIPriority(AIPriorityType.E_MAIN, 2, 0);
                    dataInv.Priorities.Add(aipriority016);
                    aipriority016.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 52, 0, false, 50));
                    aipriority016.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 50, 0, true, 50));
                    aipriority016.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 51, 5, false, 50));
                    aipriority016.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 12, 4, false, 2));
                    aipriority016.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 25, 4, false, 1));
                    aipriority016.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 25, 3, false, 0));
                    AIPriority aipriority017 = new AIPriority(AIPriorityType.E_MAIN, 1, 0);
                    dataInv.Priorities.Add(aipriority017);
                    aipriority017.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 98, 0, true, 50));
                    aipriority017.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 20, 0, false, 0));
                    aipriority017.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 54, 5, false, 5));
                    aipriority017.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 23, 3, false, 2));
                    aipriority017.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 23, 2, false, 0));
                    AIPriority aipriority018 = new AIPriority(AIPriorityType.E_MAIN, 0, 0);
                    dataInv.Priorities.Add(aipriority018);
                    aipriority018.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 66, 0, true, 50));
                    aipriority018.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 20, 0, false, 0));
                    aipriority018.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 54, 0, false, 5));
                    aipriority018.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 55, 4, false, 100));
                    aipriority018.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 86, 0, true, 60));
                    aipriority018.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 24, 3, false, 30));
                    aipriority018.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 24, 2, false, 20));
                    break;
                case 2:
                    AIPriority aipriority10 = new AIPriority(AIPriorityType.E_TWEAK, 82, 3);
                    dataInv.Priorities.Add(aipriority10);
                    AIPriority aipriority11 = new AIPriority(AIPriorityType.E_CLASS_MAIN, 3, 3);
                    dataInv.Priorities.Add(aipriority11);
                    aipriority11.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 20, 0, false, 50));
                    AIPriority aipriority12 = new AIPriority(AIPriorityType.E_PROGRAM, 32, 2);
                    dataInv.Priorities.Add(aipriority12);
                    AIPriority aipriority13 = new AIPriority(AIPriorityType.E_PROGRAM, 28, 2);
                    dataInv.Priorities.Add(aipriority13);
                    AIPriority aipriority14 = new AIPriority(AIPriorityType.E_PROGRAM, 27, 2);
                    dataInv.Priorities.Add(aipriority14);
                    AIPriority aipriority15 = new AIPriority(AIPriorityType.E_TWEAK, 20, 2);
                    dataInv.Priorities.Add(aipriority15);
                    aipriority15.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 49, 0, false, 50));
                    AIPriority aipriority16 = new AIPriority(AIPriorityType.E_MAIN, 4, 2);
                    dataInv.Priorities.Add(aipriority16);
                    aipriority16.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 20, 0, false, 0));
                    AIPriority aipriority17 = new AIPriority(AIPriorityType.E_TWEAK, 52, 1);
                    dataInv.Priorities.Add(aipriority17);
                    AIPriority aipriority18 = new AIPriority(AIPriorityType.E_TWEAK, 21, 1);
                    dataInv.Priorities.Add(aipriority18);
                    aipriority18.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 68, 4, false, 0));
                    if (!enemyAI)
                    {
                        aipriority18.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CAPTAINS_ORDER, 4, 4, false, 50));
                    }
                    aipriority18.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 75, 3, false, 150));
                    aipriority18.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 75, 1, true, 50));
                    aipriority18.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_ALERT_LEVEL, 0, 2, false, 50));
                    AIPriority aipriority19 = new AIPriority(AIPriorityType.E_MAIN, 5, 1);
                    dataInv.Priorities.Add(aipriority19);
                    if (!enemyAI)
                    {
                        aipriority19.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CAPTAINS_ORDER, 3, 4, false, 50));
                    }
                    aipriority19.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 84, 3, true, 50));
                    aipriority19.Metadata.Add(new PLPriorityMetadata_Float(3, 1, "Range (m)"));
                    AIPriority aipriority110 = new AIPriority(AIPriorityType.E_CLASS_MAIN, 72, 0);
                    dataInv.Priorities.Add(aipriority110);
                    aipriority110.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 32, 0, true, 1));
                    aipriority110.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 79, 5, false, 1));
                    AIPriority aipriority111 = new AIPriority(AIPriorityType.E_CLASS_MAIN, 67, 0);
                    dataInv.Priorities.Add(aipriority111);
                    aipriority111.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_ALERT_LEVEL, 0, 0, true, 50));
                    aipriority111.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 1, 0, true, 50));
                    aipriority111.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 20, 0, false, 50));
                    aipriority111.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 72, 0, true, 8));
                    aipriority111.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 71, 0, true, 10));
                    aipriority111.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 89, 1, false, 45));
                    AIPriority aipriority112 = new AIPriority(AIPriorityType.E_MAIN, 66, 3);
                    dataInv.Priorities.Add(aipriority112);
                    aipriority112.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 69, 0, true, 50));
                    aipriority112.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 70, 0, true, 50));
                    AIPriority aipriority113 = new AIPriority(AIPriorityType.E_MAIN, 65, 0);
                    dataInv.Priorities.Add(aipriority113);
                    aipriority113.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_ALERT_LEVEL, 2, 0, true, 50));
                    if (!enemyAI)
                    {
                        aipriority113.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CAPTAINS_ORDER, 0, 3, false, 50));
                    }
                    AIPriority aipriority114 = new AIPriority(AIPriorityType.E_MAIN, 64, 0);
                    dataInv.Priorities.Add(aipriority114);
                    aipriority114.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 67, 4, false, 50));
                    AIPriority aipriority115 = new AIPriority(AIPriorityType.E_MAIN, 63, 2);
                    dataInv.Priorities.Add(aipriority115);
                    aipriority115.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 65, 0, true, 50));
                    aipriority115.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 66, 0, true, 50));
                    AIPriority aipriority116 = new AIPriority(AIPriorityType.E_CLASS_MAIN, 61, 0);
                    dataInv.Priorities.Add(aipriority116);
                    aipriority116.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 62, 0, true, 50));
                    aipriority116.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 64, 0, false, 50));
                    if (!enemyAI)
                    {
                        aipriority116.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CAPTAINS_ORDER, 2, 3, false, 50));
                    }
                    AIPriority aipriority117 = new AIPriority(AIPriorityType.E_MAIN, 60, 3);
                    dataInv.Priorities.Add(aipriority117);
                    aipriority117.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 60, 0, true, 0));
                    aipriority117.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 61, 0, true, 0));
                    aipriority117.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 68, 0, false, 0));
                    aipriority117.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_ALERT_LEVEL, 0, 0, false, 0));
                    AIPriority aipriority118 = new AIPriority(AIPriorityType.E_MAIN, 58, 0);
                    dataInv.Priorities.Add(aipriority118);
                    aipriority118.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 57, 0, true, 50));
                    aipriority118.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 56, 3, false, 50));
                    AIPriority aipriority119 = new AIPriority(AIPriorityType.E_TWEAK, 51, 2);
                    dataInv.Priorities.Add(aipriority119);
                    if (!enemyAI)
                    {
                        aipriority119.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CAPTAINS_ORDER, 2, 2, false, 50));
                    }
                    aipriority119.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 53, 0, true, 50));
                    AIPriority aipriority120 = new AIPriority(AIPriorityType.E_MAIN, 49, 0);
                    dataInv.Priorities.Add(aipriority120);
                    aipriority120.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 52, 0, true, 50));
                    aipriority120.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 51, 5, false, 50));
                    aipriority120.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 12, 4, false, 2));
                    AIPriority aipriority121 = new AIPriority(AIPriorityType.E_PROGRAM, 35, 1);
                    dataInv.Priorities.Add(aipriority121);
                    aipriority121.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 1, 0, true, 10));
                    aipriority121.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_ALERT_LEVEL, 0, 0, true, 50));
                    aipriority121.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 7, 0, true, 20));
                    AIPriority aipriority122 = new AIPriority(AIPriorityType.E_PROGRAM, 34, 1);
                    dataInv.Priorities.Add(aipriority122);
                    aipriority122.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_ALERT_LEVEL, 0, 0, true, 50));
                    aipriority122.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 6, 0, false, 50));
                    AIPriority aipriority123 = new AIPriority(AIPriorityType.E_PROGRAM, 33, 0);
                    dataInv.Priorities.Add(aipriority123);
                    AIPriority aipriority124 = new AIPriority(AIPriorityType.E_PROGRAM, 31, 1);
                    dataInv.Priorities.Add(aipriority124);
                    aipriority124.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 72, 0, true, 5));
                    aipriority124.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_ALERT_LEVEL, 0, 0, true, 50));
                    AIPriority aipriority125 = new AIPriority(AIPriorityType.E_PROGRAM, 30, 1);
                    dataInv.Priorities.Add(aipriority125);
                    aipriority125.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 32, 0, true, 0));
                    aipriority125.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 7, 0, true, 20));
                    aipriority125.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_ALERT_LEVEL, 0, 0, true, 50));
                    AIPriority aipriority126 = new AIPriority(AIPriorityType.E_PROGRAM, 29, 1);
                    dataInv.Priorities.Add(aipriority126);
                    aipriority126.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 1, 0, true, 10));
                    aipriority126.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_ALERT_LEVEL, 0, 0, true, 50));
                    aipriority126.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 7, 0, true, 20));
                    AIPriority aipriority127 = new AIPriority(AIPriorityType.E_PROGRAM, 26, 1);
                    dataInv.Priorities.Add(aipriority127);
                    aipriority127.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 1, 0, true, 15));
                    aipriority127.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_ALERT_LEVEL, 0, 0, true, 50));
                    aipriority127.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 75, 0, false, 75));
                    AIPriority aipriority128 = new AIPriority(AIPriorityType.E_PROGRAM, 25, 1);
                    dataInv.Priorities.Add(aipriority128);
                    aipriority128.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 93, 0, false, 50));
                    aipriority128.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_ALERT_LEVEL, 0, 0, true, 50));
                    AIPriority aipriority129 = new AIPriority(AIPriorityType.E_PROGRAM, 24, 1);
                    dataInv.Priorities.Add(aipriority129);
                    aipriority129.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 77, 1, true, 20));
                    aipriority129.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 2, 0, true, 75));
                    AIPriority aipriority130 = new AIPriority(AIPriorityType.E_PROGRAM, 23, 1);
                    dataInv.Priorities.Add(aipriority130);
                    aipriority130.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 94, 0, true, 95));
                    AIPriority aipriority131 = new AIPriority(AIPriorityType.E_PROGRAM, 22, 2);
                    dataInv.Priorities.Add(aipriority131);
                    aipriority131.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_ALERT_LEVEL, 2, 0, false, 50));
                    aipriority131.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 75, 0, true, 100));
                    AIPriority aipriority132 = new AIPriority(AIPriorityType.E_PROGRAM, 21, 0);
                    dataInv.Priorities.Add(aipriority132);
                    aipriority132.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_ALERT_LEVEL, 2, 0, false, 50));
                    aipriority132.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 87, 2, false, 100));
                    AIPriority aipriority133 = new AIPriority(AIPriorityType.E_PROGRAM, 20, 1);
                    dataInv.Priorities.Add(aipriority133);
                    aipriority133.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_ALERT_LEVEL, 0, 0, true, 50));
                    aipriority133.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 6, 0, false, 50));
                    AIPriority aipriority134 = new AIPriority(AIPriorityType.E_PROGRAM, 19, 0);
                    dataInv.Priorities.Add(aipriority134);
                    aipriority134.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_ALERT_LEVEL, 2, 0, false, 50));
                    aipriority134.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 27, 2, false, 1));
                    aipriority134.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 27, 1, false, 0));
                    AIPriority aipriority135 = new AIPriority(AIPriorityType.E_TWEAK, 18, 0);
                    dataInv.Priorities.Add(aipriority135);
                    if (!enemyAI)
                    {
                        aipriority135.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CAPTAINS_ORDER, 2, 0, false, 50));
                    }
                    aipriority135.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 92, 2, true, 50));
                    aipriority135.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 28, 1, false, 0));
                    aipriority135.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 27, 1, false, 0));
                    AIPriority aipriority136 = new AIPriority(AIPriorityType.E_PROGRAM, 18, 1);
                    dataInv.Priorities.Add(aipriority136);
                    aipriority136.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 3, 0, true, 80));
                    aipriority136.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_ALERT_LEVEL, 0, 0, true, 50));
                    aipriority136.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 6, 0, false, 50));
                    AIPriority aipriority137 = new AIPriority(AIPriorityType.E_PROGRAM, 17, 1);
                    dataInv.Priorities.Add(aipriority137);
                    aipriority137.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_ALERT_LEVEL, 0, 0, true, 50));
                    aipriority137.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 6, 0, false, 50));
                    AIPriority aipriority138 = new AIPriority(AIPriorityType.E_PROGRAM, 16, 1);
                    dataInv.Priorities.Add(aipriority138);
                    aipriority138.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_ALERT_LEVEL, 0, 0, true, 50));
                    AIPriority aipriority139 = new AIPriority(AIPriorityType.E_PROGRAM, 15, 1);
                    dataInv.Priorities.Add(aipriority139);
                    aipriority139.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 6, 0, false, 50));
                    aipriority139.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 92, 0, false, 50));
                    aipriority139.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_ALERT_LEVEL, 2, 0, false, 50));
                    AIPriority aipriority140 = new AIPriority(AIPriorityType.E_PROGRAM, 14, 1);
                    dataInv.Priorities.Add(aipriority140);
                    aipriority140.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_ALERT_LEVEL, 0, 0, true, 50));
                    aipriority140.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 1, 0, true, 50));
                    aipriority140.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 7, 0, true, 20));
                    aipriority140.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 6, 0, false, 50));
                    AIPriority aipriority141 = new AIPriority(AIPriorityType.E_PROGRAM, 13, 1);
                    dataInv.Priorities.Add(aipriority141);
                    aipriority141.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 72, 0, true, 5));
                    aipriority141.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_ALERT_LEVEL, 0, 0, true, 50));
                    AIPriority aipriority142 = new AIPriority(AIPriorityType.E_PROGRAM, 12, 1);
                    dataInv.Priorities.Add(aipriority142);
                    aipriority142.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 6, 0, false, 50));
                    AIPriority aipriority143 = new AIPriority(AIPriorityType.E_MAIN, 11, 0);
                    dataInv.Priorities.Add(aipriority143);
                    aipriority143.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 20, 0, false, 50));
                    aipriority143.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 52, 0, false, 50));
                    aipriority143.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 13, 3, true, 30));
                    aipriority143.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 13, 2, true, 55));
                    aipriority143.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 13, 1, true, 80));
                    AIPriority aipriority144 = new AIPriority(AIPriorityType.E_PROGRAM, 11, 1);
                    dataInv.Priorities.Add(aipriority144);
                    aipriority144.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 6, 0, false, 50));
                    AIPriority aipriority145 = new AIPriority(AIPriorityType.E_CLASS_MAIN, 10, 0);
                    dataInv.Priorities.Add(aipriority145);
                    aipriority145.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 20, 0, false, 50));
                    aipriority145.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 83, 0, false, 50));
                    aipriority145.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 17, 4, true, 50));
                    aipriority145.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 18, 3, true, 50));
                    aipriority145.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 19, 3, true, 50));
                    AIPriority aipriority146 = new AIPriority(AIPriorityType.E_PROGRAM, 10, 0);
                    dataInv.Priorities.Add(aipriority146);
                    aipriority146.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_ALERT_LEVEL, 2, 0, false, 50));
                    aipriority146.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 27, 2, false, 1));
                    aipriority146.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 27, 1, false, 0));
                    AIPriority aipriority147 = new AIPriority(AIPriorityType.E_PROGRAM, 9, 1);
                    dataInv.Priorities.Add(aipriority147);
                    aipriority147.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 6, 0, false, 50));
                    aipriority147.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 64, 2, false, 50));
                    AIPriority aipriority148 = new AIPriority(AIPriorityType.E_TWEAK, 8, 0);
                    dataInv.Priorities.Add(aipriority148);
                    AIPriority aipriority149 = new AIPriority(AIPriorityType.E_PROGRAM, 7, 1);
                    dataInv.Priorities.Add(aipriority149);
                    aipriority149.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 32, 1, false, 5));
                    aipriority149.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 32, 0, true, 0));
                    aipriority149.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 7, 0, true, 20));
                    aipriority149.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_ALERT_LEVEL, 0, 0, true, 50));
                    AIPriority aipriority150 = new AIPriority(AIPriorityType.E_PROGRAM, 6, 1);
                    dataInv.Priorities.Add(aipriority150);
                    aipriority150.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_ALERT_LEVEL, 2, 0, false, 50));
                    aipriority150.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 6, 0, false, 50));
                    aipriority150.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 24, 0, true, 20));
                    AIPriority aipriority151 = new AIPriority(AIPriorityType.E_CLASS_MAIN, 6, 0);
                    dataInv.Priorities.Add(aipriority151);
                    aipriority151.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 20, 3, false, 50));
                    aipriority151.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 26, 0, true, 50));
                    aipriority151.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 58, 4, false, 50));
                    if (!enemyAI)
                    {
                        aipriority151.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CAPTAINS_ORDER, 5, 5, false, 50));
                    }
                    if (!enemyAI)
                    {
                        aipriority151.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CAPTAINS_ORDER, 6, 3, false, 50));
                    }
                    AIPriority aipriority152 = new AIPriority(AIPriorityType.E_PROGRAM, 5, 1);
                    dataInv.Priorities.Add(aipriority152);
                    aipriority152.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_ALERT_LEVEL, 0, 0, true, 50));
                    aipriority152.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 7, 0, true, 20));
                    aipriority152.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 1, 0, true, 30));
                    aipriority152.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 0, 0, true, 30));
                    aipriority152.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 5, 0, true, 50));
                    aipriority152.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 10, 0, false, 50));
                    AIPriority aipriority153 = new AIPriority(AIPriorityType.E_PROGRAM, 4, 1);
                    dataInv.Priorities.Add(aipriority153);
                    aipriority153.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 1, 0, true, 20));
                    aipriority153.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_ALERT_LEVEL, 0, 0, true, 50));
                    aipriority153.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 7, 0, true, 20));
                    AIPriority aipriority154 = new AIPriority(AIPriorityType.E_PROGRAM, 3, 0);
                    dataInv.Priorities.Add(aipriority154);
                    aipriority154.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_ALERT_LEVEL, 2, 0, false, 50));
                    aipriority154.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 27, 2, false, 1));
                    aipriority154.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 27, 1, false, 0));
                    AIPriority aipriority155 = new AIPriority(AIPriorityType.E_MAIN, 2, 0);
                    dataInv.Priorities.Add(aipriority155);
                    aipriority155.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 52, 0, false, 50));
                    aipriority155.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 50, 0, true, 50));
                    aipriority155.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 51, 5, false, 50));
                    aipriority155.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 12, 4, false, 2));
                    aipriority155.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 25, 4, false, 1));
                    aipriority155.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 25, 3, false, 0));
                    AIPriority aipriority156 = new AIPriority(AIPriorityType.E_PROGRAM, 2, 0);
                    dataInv.Priorities.Add(aipriority156);
                    aipriority156.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_ALERT_LEVEL, 2, 0, false, 50));
                    aipriority156.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 27, 2, false, 1));
                    aipriority156.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 27, 1, false, 0));
                    AIPriority aipriority157 = new AIPriority(AIPriorityType.E_PROGRAM, 1, 0);
                    dataInv.Priorities.Add(aipriority157);
                    aipriority157.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 80, 2, false, 60));
                    AIPriority aipriority158 = new AIPriority(AIPriorityType.E_MAIN, 1, 0);
                    dataInv.Priorities.Add(aipriority158);
                    aipriority158.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 98, 0, true, 50));
                    aipriority158.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 20, 0, false, 0));
                    aipriority158.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 54, 5, false, 5));
                    aipriority158.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 23, 3, false, 2));
                    aipriority158.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 23, 2, false, 0));
                    AIPriority aipriority159 = new AIPriority(AIPriorityType.E_PROGRAM, 0, 0);
                    dataInv.Priorities.Add(aipriority159);
                    aipriority159.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_ALERT_LEVEL, 2, 0, false, 50));
                    aipriority159.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 27, 2, false, 1));
                    aipriority159.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 27, 1, false, 0));
                    AIPriority aipriority160 = new AIPriority(AIPriorityType.E_MAIN, 0, 0);
                    dataInv.Priorities.Add(aipriority160);
                    aipriority160.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 66, 0, true, 50));
                    aipriority160.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 20, 0, false, 0));
                    aipriority160.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 54, 0, false, 5));
                    aipriority160.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 55, 4, false, 100));
                    aipriority160.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 86, 0, true, 25));
                    aipriority160.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 24, 4, false, 20));
                    aipriority160.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 24, 3, false, 10));
                    break;
                case 3:
                    AIPriority aipriority20 = new AIPriority(AIPriorityType.E_TWEAK, 82, 5);
                    dataInv.Priorities.Add(aipriority20);
                    AIPriority aipriority21 = new AIPriority(AIPriorityType.E_MAIN, 4, 3);
                    dataInv.Priorities.Add(aipriority21);
                    aipriority21.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 20, 0, false, 0));
                    AIPriority aipriority22 = new AIPriority(AIPriorityType.E_TWEAK, 9, 2);
                    dataInv.Priorities.Add(aipriority22);
                    aipriority22.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 68, 3, false, 0));
                    aipriority22.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 75, 3, false, 150));
                    aipriority22.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 75, 1, true, 50));
                    AIPriority aipriority23 = new AIPriority(AIPriorityType.E_MISSILE, 5, 2);
                    dataInv.Priorities.Add(aipriority23);
                    AIPriority aipriority24 = new AIPriority(AIPriorityType.E_MISSILE, 1, 2);
                    dataInv.Priorities.Add(aipriority24);
                    AIPriority aipriority25 = new AIPriority(AIPriorityType.E_MISSILE, 0, 2);
                    dataInv.Priorities.Add(aipriority25);
                    AIPriority aipriority26 = new AIPriority(AIPriorityType.E_MISSILE, 7, 1);
                    dataInv.Priorities.Add(aipriority26);
                    aipriority26.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 15, 2, false, 20));
                    AIPriority aipriority27 = new AIPriority(AIPriorityType.E_MAIN, 5, 1);
                    dataInv.Priorities.Add(aipriority27);
                    if (!enemyAI)
                    {
                        aipriority27.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CAPTAINS_ORDER, 3, 4, false, 50));
                    }
                    aipriority27.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 84, 3, true, 50));
                    aipriority27.Metadata.Add(new PLPriorityMetadata_Float(3, 1, "Range (m)"));
                    AIPriority aipriority28 = new AIPriority(AIPriorityType.E_CLASS_MAIN, 68, 0);
                    dataInv.Priorities.Add(aipriority28);
                    aipriority28.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_ALERT_LEVEL, 0, 0, true, 50));
                    aipriority28.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 6, 0, false, 50));
                    aipriority28.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 50, 0, false, 3));
                    aipriority28.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 20, 0, false, 50));
                    aipriority28.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 73, 0, true, 50));
                    if (!enemyAI)
                    {
                        aipriority28.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CAPTAINS_ORDER, 4, 4, false, 50));
                    }
                    aipriority28.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 75, 4, false, 200));
                    AIPriority aipriority29 = new AIPriority(AIPriorityType.E_CLASS_MAIN, 67, 0);
                    dataInv.Priorities.Add(aipriority29);
                    aipriority29.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_ALERT_LEVEL, 0, 0, true, 50));
                    aipriority29.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 1, 0, true, 50));
                    aipriority29.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 20, 0, false, 50));
                    aipriority29.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 72, 0, true, 8));
                    aipriority29.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 71, 0, true, 2));
                    aipriority29.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 89, 1, false, 45));
                    AIPriority aipriority210 = new AIPriority(AIPriorityType.E_MAIN, 66, 3);
                    dataInv.Priorities.Add(aipriority210);
                    aipriority210.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 69, 0, true, 50));
                    aipriority210.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 70, 0, true, 50));
                    AIPriority aipriority211 = new AIPriority(AIPriorityType.E_MAIN, 65, 0);
                    dataInv.Priorities.Add(aipriority211);
                    aipriority211.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_ALERT_LEVEL, 2, 0, true, 50));
                    if (!enemyAI)
                    {
                        aipriority211.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CAPTAINS_ORDER, 0, 3, false, 50));
                    }
                    AIPriority aipriority212 = new AIPriority(AIPriorityType.E_MAIN, 64, 0);
                    dataInv.Priorities.Add(aipriority212);
                    aipriority212.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 67, 4, false, 50));
                    AIPriority aipriority213 = new AIPriority(AIPriorityType.E_MAIN, 63, 2);
                    dataInv.Priorities.Add(aipriority213);
                    aipriority213.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 65, 0, true, 50));
                    aipriority213.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 66, 0, true, 50));
                    AIPriority aipriority214 = new AIPriority(AIPriorityType.E_MAIN, 60, 3);
                    dataInv.Priorities.Add(aipriority214);
                    aipriority214.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 60, 0, true, 0));
                    aipriority214.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 61, 0, true, 0));
                    aipriority214.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 68, 0, false, 0));
                    aipriority214.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_ALERT_LEVEL, 0, 0, false, 0));
                    AIPriority aipriority215 = new AIPriority(AIPriorityType.E_MAIN, 58, 0);
                    dataInv.Priorities.Add(aipriority215);
                    aipriority215.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 57, 0, true, 50));
                    aipriority215.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 56, 3, false, 50));
                    AIPriority aipriority216 = new AIPriority(AIPriorityType.E_MAIN, 49, 0);
                    dataInv.Priorities.Add(aipriority216);
                    aipriority216.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 52, 0, true, 50));
                    aipriority216.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 51, 5, false, 50));
                    aipriority216.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 12, 4, false, 2));
                    AIPriority aipriority217 = new AIPriority(AIPriorityType.E_MAIN, 11, 0);
                    dataInv.Priorities.Add(aipriority217);
                    aipriority217.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 20, 0, false, 50));
                    aipriority217.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 52, 0, false, 50));
                    aipriority217.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 13, 3, true, 30));
                    aipriority217.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 13, 2, true, 55));
                    aipriority217.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 13, 1, true, 80));
                    AIPriority aipriority218 = new AIPriority(AIPriorityType.E_MISSILE, 10, 0);
                    dataInv.Priorities.Add(aipriority218);
                    aipriority218.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 15, 0, true, 10));
                    aipriority218.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 75, 2, false, 120));
                    AIPriority aipriority219 = new AIPriority(AIPriorityType.E_CLASS_MAIN, 10, 0);
                    dataInv.Priorities.Add(aipriority219);
                    aipriority219.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 20, 0, false, 50));
                    aipriority219.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 83, 0, false, 50));
                    aipriority219.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 17, 3, true, 50));
                    aipriority219.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 18, 2, true, 50));
                    aipriority219.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 19, 2, true, 50));
                    AIPriority aipriority220 = new AIPriority(AIPriorityType.E_MISSILE, 9, 0);
                    dataInv.Priorities.Add(aipriority220);
                    aipriority220.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 15, 3, false, 10));
                    AIPriority aipriority221 = new AIPriority(AIPriorityType.E_TWEAK, 8, 0);
                    dataInv.Priorities.Add(aipriority221);
                    AIPriority aipriority222 = new AIPriority(AIPriorityType.E_MISSILE, 8, 0);
                    dataInv.Priorities.Add(aipriority222);
                    aipriority222.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 15, 0, true, 10));
                    aipriority222.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 87, 0, true, 50));
                    aipriority222.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 87, 3, false, 100));
                    AIPriority aipriority223 = new AIPriority(AIPriorityType.E_CLASS_MAIN, 6, 0);
                    dataInv.Priorities.Add(aipriority223);
                    aipriority223.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 20, 3, false, 50));
                    aipriority223.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 26, 0, true, 50));
                    aipriority223.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 58, 4, false, 50));
                    if (!enemyAI)
                    {
                        aipriority223.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CAPTAINS_ORDER, 5, 5, false, 50));
                    }
                    if (!enemyAI)
                    {
                        aipriority223.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CAPTAINS_ORDER, 6, 5, false, 50));
                    }
                    AIPriority aipriority224 = new AIPriority(AIPriorityType.E_MISSILE, 6, 0);
                    dataInv.Priorities.Add(aipriority224);
                    aipriority224.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 75, 2, false, 75));
                    AIPriority aipriority225 = new AIPriority(AIPriorityType.E_MAIN, 2, 0);
                    dataInv.Priorities.Add(aipriority225);
                    aipriority225.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 52, 0, false, 50));
                    aipriority225.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 50, 0, true, 50));
                    aipriority225.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 51, 5, false, 50));
                    aipriority225.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 12, 4, false, 2));
                    aipriority225.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 25, 4, false, 1));
                    aipriority225.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 25, 3, false, 0));
                    AIPriority aipriority226 = new AIPriority(AIPriorityType.E_MISSILE, 2, 0);
                    dataInv.Priorities.Add(aipriority226);
                    aipriority226.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 15, 3, true, 50));
                    AIPriority aipriority227 = new AIPriority(AIPriorityType.E_MAIN, 1, 0);
                    dataInv.Priorities.Add(aipriority227);
                    aipriority227.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 98, 0, true, 50));
                    aipriority227.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 20, 0, false, 0));
                    aipriority227.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 54, 5, false, 5));
                    aipriority227.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 23, 3, false, 2));
                    aipriority227.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 23, 2, false, 0));
                    AIPriority aipriority228 = new AIPriority(AIPriorityType.E_MAIN, 0, 0);
                    dataInv.Priorities.Add(aipriority228);
                    aipriority228.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 66, 0, true, 50));
                    aipriority228.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 20, 0, false, 0));
                    aipriority228.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 54, 0, false, 5));
                    aipriority228.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 55, 3, false, 100));
                    aipriority228.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 86, 0, true, 60));
                    aipriority228.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 24, 3, false, 30));
                    aipriority228.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 24, 2, false, 20));
                    break;
                case 4:
                    AIPriority aipriority30 = new AIPriority(AIPriorityType.E_TWEAK, 48, 5);
                    dataInv.Priorities.Add(aipriority30);
                    aipriority30.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 96, 1, true, 50));
                    aipriority30.Metadata.Add(new PLPriorityMetadata_Float(15, 1, "Player Override Time"));
                    AIPriority aipriority31 = new AIPriority(AIPriorityType.E_TWEAK, 14, 3);
                    dataInv.Priorities.Add(aipriority31);
                    aipriority31.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 2, 5, true, 60));
                    aipriority31.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 68, 0, false, 0));
                    aipriority31.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 90, 0, false, 50));
                    aipriority31.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 77, 0, true, 10));
                    aipriority31.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 77, 1, true, 20));
                    aipriority31.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 77, 2, true, 40));
                    aipriority31.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 77, 3, true, 60));
                    aipriority31.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 91, 4, true, 50));
                    aipriority31.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 2, 2, false, 80));
                    aipriority31.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 2, 0, false, 90));
                    aipriority31.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_ALERT_LEVEL, 0, 1, false, 50));
                    AIPriority aipriority32 = new AIPriority(AIPriorityType.E_TWEAK, 82, 3);
                    dataInv.Priorities.Add(aipriority32);
                    AIPriority aipriority33 = new AIPriority(AIPriorityType.E_TWEAK, 46, 3);
                    dataInv.Priorities.Add(aipriority33);
                    aipriority33.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 64, 1, false, 50));
                    aipriority33.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 68, 5, false, 0));
                    aipriority33.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 30, 5, false, 50));
                    aipriority33.Metadata.Add(new PLPriorityMetadata_Float(15, 1, "Player Override Time"));
                    AIPriority aipriority34 = new AIPriority(AIPriorityType.E_TWEAK, 45, 3);
                    dataInv.Priorities.Add(aipriority34);
                    aipriority34.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 64, 1, false, 50));
                    aipriority34.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 96, 1, true, 50));
                    aipriority34.Metadata.Add(new PLPriorityMetadata_Float(15, 1, "Player Override Time"));
                    AIPriority aipriority35 = new AIPriority(AIPriorityType.E_CLASS_MAIN, 3, 3);
                    dataInv.Priorities.Add(aipriority35);
                    aipriority35.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 20, 0, false, 50));
                    aipriority35.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 58, 4, false, 50));
                    AIPriority aipriority36 = new AIPriority(AIPriorityType.E_TWEAK, 47, 2);
                    dataInv.Priorities.Add(aipriority36);
                    aipriority36.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 64, 0, false, 50));
                    aipriority36.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 96, 0, true, 50));
                    aipriority36.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 1, 3, false, 75));
                    aipriority36.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 1, 4, false, 50));
                    aipriority36.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 1, 5, false, 25));
                    aipriority36.Metadata.Add(new PLPriorityMetadata_Float(15, 1, "Player Override Time"));
                    AIPriority aipriority37 = new AIPriority(AIPriorityType.E_TWEAK, 78, 1);
                    dataInv.Priorities.Add(aipriority37);
                    aipriority37.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 96, 0, true, 50));
                    aipriority37.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 77, 0, true, 5));
                    AIPriority aipriority38 = new AIPriority(AIPriorityType.E_TWEAK, 76, 1);
                    dataInv.Priorities.Add(aipriority38);
                    aipriority38.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 96, 0, true, 50));
                    aipriority38.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 77, 0, true, 5));
                    AIPriority aipriority39 = new AIPriority(AIPriorityType.E_TWEAK, 75, 1);
                    dataInv.Priorities.Add(aipriority39);
                    aipriority39.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 99, 0, false, 50));
                    aipriority39.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 64, 0, false, 50));
                    aipriority39.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 96, 0, true, 50));
                    aipriority39.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 77, 0, true, 5));
                    AIPriority aipriority310 = new AIPriority(AIPriorityType.E_TWEAK, 74, 1);
                    dataInv.Priorities.Add(aipriority310);
                    aipriority310.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 99, 0, false, 50));
                    aipriority310.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 77, 0, true, 5));
                    AIPriority aipriority311 = new AIPriority(AIPriorityType.E_TWEAK, 73, 1);
                    dataInv.Priorities.Add(aipriority311);
                    aipriority311.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 96, 0, true, 50));
                    aipriority311.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 77, 0, true, 5));
                    AIPriority aipriority312 = new AIPriority(AIPriorityType.E_MAIN, 5, 1);
                    dataInv.Priorities.Add(aipriority312);
                    if (!enemyAI)
                    {
                        aipriority312.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CAPTAINS_ORDER, 3, 4, false, 50));
                    }
                    aipriority312.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 84, 3, true, 50));
                    aipriority312.Metadata.Add(new PLPriorityMetadata_Float(3, 1, "Range (m)"));
                    AIPriority aipriority313 = new AIPriority(AIPriorityType.E_MAIN, 4, 1);
                    dataInv.Priorities.Add(aipriority313);
                    aipriority313.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 20, 0, false, 0));
                    AIPriority aipriority314 = new AIPriority(AIPriorityType.E_CLASS_MAIN, 81, 0);
                    dataInv.Priorities.Add(aipriority314);
                    aipriority314.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 97, 0, false, 50));
                    aipriority314.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 95, 5, false, 50));
                    AIPriority aipriority315 = new AIPriority(AIPriorityType.E_TWEAK, 80, 1);
                    dataInv.Priorities.Add(aipriority315);
                    aipriority315.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 64, 0, false, 50));
                    aipriority315.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 96, 0, true, 50));
                    aipriority315.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 6, 0, false, 50));
                    aipriority315.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 77, 0, true, 5));
                    AIPriority aipriority316 = new AIPriority(AIPriorityType.E_TWEAK, 79, 1);
                    dataInv.Priorities.Add(aipriority316);
                    aipriority316.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 64, 0, false, 50));
                    aipriority316.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 96, 0, true, 50));
                    aipriority316.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 27, 0, true, 0));
                    aipriority316.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 77, 0, true, 5));
                    AIPriority aipriority317 = new AIPriority(AIPriorityType.E_TWEAK, 77, 1);
                    dataInv.Priorities.Add(aipriority317);
                    aipriority317.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 64, 0, false, 50));
                    aipriority317.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 96, 0, true, 50));
                    aipriority317.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 6, 0, false, 50));
                    aipriority317.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 77, 0, true, 5));
                    AIPriority aipriority318 = new AIPriority(AIPriorityType.E_TWEAK, 71, 0);
                    dataInv.Priorities.Add(aipriority318);
                    aipriority318.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_ALERT_LEVEL, 0, 0, true, 50));
                    aipriority318.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 2, 0, false, 80));
                    aipriority318.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 3, 0, true, 70));
                    aipriority318.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 16, 0, false, 25));
                    aipriority318.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 90, 5, false, 50));
                    aipriority318.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 75, 3, false, 125));
                    aipriority318.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 91, 2, true, 50));
                    AIPriority aipriority319 = new AIPriority(AIPriorityType.E_CLASS_MAIN, 67, 0);
                    dataInv.Priorities.Add(aipriority319);
                    aipriority319.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_ALERT_LEVEL, 0, 0, true, 50));
                    aipriority319.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 1, 0, true, 50));
                    aipriority319.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 20, 0, false, 50));
                    aipriority319.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 72, 0, true, 8));
                    aipriority319.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 71, 0, true, 10));
                    aipriority319.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 89, 3, false, 45));
                    AIPriority aipriority320 = new AIPriority(AIPriorityType.E_MAIN, 66, 3);
                    dataInv.Priorities.Add(aipriority320);
                    aipriority320.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 69, 0, true, 50));
                    aipriority320.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 70, 0, true, 50));
                    AIPriority aipriority321 = new AIPriority(AIPriorityType.E_MAIN, 65, 0);
                    dataInv.Priorities.Add(aipriority321);
                    aipriority321.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_ALERT_LEVEL, 2, 0, true, 50));
                    if (!enemyAI)
                    {
                        aipriority321.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CAPTAINS_ORDER, 0, 3, false, 50));
                    }
                    AIPriority aipriority322 = new AIPriority(AIPriorityType.E_MAIN, 64, 0);
                    dataInv.Priorities.Add(aipriority322);
                    aipriority322.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 67, 4, false, 50));
                    AIPriority aipriority323 = new AIPriority(AIPriorityType.E_MAIN, 63, 2);
                    dataInv.Priorities.Add(aipriority323);
                    aipriority323.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 65, 0, true, 50));
                    aipriority323.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 66, 0, true, 50));
                    AIPriority aipriority324 = new AIPriority(AIPriorityType.E_MAIN, 60, 3);
                    dataInv.Priorities.Add(aipriority324);
                    aipriority324.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 60, 0, true, 0));
                    aipriority324.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 61, 0, true, 0));
                    aipriority324.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 68, 0, false, 0));
                    aipriority324.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_ALERT_LEVEL, 0, 0, false, 0));
                    AIPriority aipriority325 = new AIPriority(AIPriorityType.E_MAIN, 58, 0);
                    dataInv.Priorities.Add(aipriority325);
                    aipriority325.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 57, 0, true, 50));
                    aipriority325.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 56, 3, false, 50));
                    AIPriority aipriority326 = new AIPriority(AIPriorityType.E_MAIN, 49, 0);
                    dataInv.Priorities.Add(aipriority326);
                    aipriority326.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 52, 0, true, 50));
                    aipriority326.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 51, 5, false, 50));
                    aipriority326.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 12, 4, false, 2));
                    AIPriority aipriority327 = new AIPriority(AIPriorityType.E_CLASS_MAIN, 31, 0);
                    dataInv.Priorities.Add(aipriority327);
                    aipriority327.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 19, 0, true, 50));
                    aipriority327.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 83, 3, false, 50));
                    AIPriority aipriority328 = new AIPriority(AIPriorityType.E_TWEAK, 17, 0);
                    dataInv.Priorities.Add(aipriority328);
                    aipriority328.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 77, 5, true, 10));
                    aipriority328.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 77, 4, true, 30));
                    aipriority328.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 68, 0, false, 0));
                    aipriority328.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 75, 0, false, 125));
                    aipriority328.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 2, 0, true, 25));
                    aipriority328.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 91, 0, true, 50));
                    aipriority328.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 64, 1, false, 50));
                    aipriority328.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 2, 5, false, 90));
                    aipriority328.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 2, 2, false, 60));
                    aipriority328.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 75, 1, true, 75));
                    aipriority328.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 27, 3, true, 0));
                    AIPriority aipriority329 = new AIPriority(AIPriorityType.E_TWEAK, 13, 3);
                    dataInv.Priorities.Add(aipriority329);
                    aipriority329.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 75, 1, true, 80));
                    aipriority329.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 77, 5, true, 10));
                    aipriority329.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 91, 0, true, 50));
                    aipriority329.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 2, 5, false, 90));
                    aipriority329.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_ALERT_LEVEL, 0, 2, false, 50));
                    aipriority329.Metadata.Add(new PLPriorityMetadata_Float(15, 1, "Player Override Time"));
                    AIPriority aipriority330 = new AIPriority(AIPriorityType.E_MAIN, 11, 0);
                    dataInv.Priorities.Add(aipriority330);
                    aipriority330.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 20, 0, false, 50));
                    aipriority330.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 52, 0, false, 50));
                    aipriority330.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 13, 3, true, 30));
                    aipriority330.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 13, 2, true, 55));
                    aipriority330.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 13, 1, true, 80));
                    AIPriority aipriority331 = new AIPriority(AIPriorityType.E_CLASS_MAIN, 10, 0);
                    dataInv.Priorities.Add(aipriority331);
                    aipriority331.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 20, 0, false, 50));
                    aipriority331.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 17, 5, true, 50));
                    aipriority331.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 18, 4, true, 50));
                    aipriority331.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 83, 0, false, 50));
                    aipriority331.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 19, 4, true, 50));
                    AIPriority aipriority332 = new AIPriority(AIPriorityType.E_TWEAK, 8, 0);
                    dataInv.Priorities.Add(aipriority332);
                    AIPriority aipriority333 = new AIPriority(AIPriorityType.E_CLASS_MAIN, 6, 0);
                    dataInv.Priorities.Add(aipriority333);
                    aipriority333.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 20, 3, false, 50));
                    aipriority333.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 26, 0, true, 50));
                    aipriority333.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 58, 4, false, 50));
                    if (!enemyAI)
                    {
                        aipriority333.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CAPTAINS_ORDER, 5, 5, false, 50));
                    }
                    if (!enemyAI)
                    {
                        aipriority333.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CAPTAINS_ORDER, 6, 3, false, 50));
                    }
                    AIPriority aipriority334 = new AIPriority(AIPriorityType.E_MAIN, 2, 0);
                    dataInv.Priorities.Add(aipriority334);
                    aipriority334.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 52, 0, false, 50));
                    aipriority334.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 50, 0, true, 50));
                    aipriority334.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 51, 5, false, 50));
                    aipriority334.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 12, 4, false, 2));
                    aipriority334.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 25, 4, false, 1));
                    aipriority334.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 25, 3, false, 0));
                    AIPriority aipriority335 = new AIPriority(AIPriorityType.E_MAIN, 1, 0);
                    dataInv.Priorities.Add(aipriority335);
                    aipriority335.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 98, 0, true, 50));
                    aipriority335.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 20, 0, false, 0));
                    aipriority335.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 54, 5, false, 5));
                    aipriority335.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 23, 4, false, 2));
                    aipriority335.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 23, 3, false, 0));
                    AIPriority aipriority336 = new AIPriority(AIPriorityType.E_MAIN, 0, 0);
                    dataInv.Priorities.Add(aipriority336);
                    aipriority336.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 66, 0, true, 50));
                    aipriority336.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 20, 0, false, 0));
                    aipriority336.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 54, 0, false, 5));
                    aipriority336.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 55, 4, false, 100));
                    aipriority336.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 86, 0, true, 60));
                    aipriority336.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 24, 4, false, 20));
                    aipriority336.Overrides.Add(new PLAIPriorityOverride(EPriorityOverrideType.E_CUSTOM, 24, 3, false, 10));
                    break;
            }
        }
    }
}
