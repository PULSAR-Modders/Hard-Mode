using System;
using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;
using CodeStage.AntiCheat.ObscuredTypes;

namespace Hard_Mode
{
    [HarmonyPatch(typeof(PLLiarsDiceGame), "AI_TakeTurns")]
    class LiarsDiceAI
    {
        static bool Prefix(PLLiarsDiceGame __instance, ref ObscuredInt ___LastNewPlayerTimeMs, ref float ___LastAIUpdateTime, ref ObscuredBool ___CallBluffOverTime_InProgress, ref ObscuredBool ___Game_AllowTurnsToAdvance, ref ObscuredInt ___CurrentTurn_PlayerID, ref int ___PrevTurn_PlayerID, ref float ___GameIsActive_Time, ref List<ObscuredInt> ___CurrentlyPlaying_PlayerIDs, ref ObscuredByte ___CurrentTurn_LastDieFace, ref ObscuredByte ___CurrentTurn_LastDieCount)
        {
            if (PhotonNetwork.isMasterClient && !__instance.IsCurrentlyChallenging() && !__instance.IsCurrentlyRolling() && PLServer.Instance.GetEstimatedServerMs() - ___LastNewPlayerTimeMs > 3000 && Time.time - ___LastAIUpdateTime > 1f && !___CallBluffOverTime_InProgress && ___Game_AllowTurnsToAdvance)
            {
                ___LastAIUpdateTime = Time.time;
                PLPlayer CurrentPlayer = PLServer.Instance.GetPlayerFromPlayerID(___CurrentTurn_PlayerID);
                if (CurrentPlayer != null && (CurrentPlayer.IsBot || (___GameIsActive_Time > 20f && PLServer.Instance.GetEstimatedServerMs() - ___LastNewPlayerTimeMs > 60000)))
                {
                    Dictionary<int, int> MyDices = new Dictionary<int, int>();
                    foreach (Byte value in CurrentPlayer.LocalGame_MyDice) //Gets my Current Hand
                    {
                        int num = (int)value;
                        if (!MyDices.ContainsKey(num))
                        {
                            MyDices.Add(num, 1);
                        }
                        else
                        {
                            MyDices[num]++;
                        }
                    }
                    int Players = ___CurrentlyPlaying_PlayerIDs.Count;
                    Byte CurrentFace = ___CurrentTurn_LastDieFace;
                    Byte CurrentBid = ___CurrentTurn_LastDieCount;
                    int BetFace = 0;
                    int BetValue = 0;
                    double ChanceOfTruth = 0;
                    if (CurrentPlayer.LiarsDice_DieCountOfFace(CurrentFace) >= CurrentBid) //If I have more dices of that type than the bid I know its true
                    {
                        ChanceOfTruth = 100;
                    }
                    else
                    {
                        for (int i = CurrentBid; i <= Players * 5; i++) //This calculates the chance of bid being true
                        {
                            ChanceOfTruth += (Factorial(Players * 5) / (Factorial(i) * Factorial(Players * 5 - i))) * Math.Pow(1f / 6f, i) * Math.Pow(5f / 6f, Players * 5 - i);
                        }
                    }
                    ChanceOfTruth *= 100;
                    foreach(Byte Face in MyDices.Keys) //Gets the highest value Dice in My Hand
                    { 
                        if(MyDices.GetValueSafe(Face) > BetValue) 
                        {
                            BetFace = Face;
                            BetValue = MyDices.GetValueSafe(Face);
                        }
                    }
                    if (BetFace == CurrentFace)
                    {
                        BetValue += CurrentBid + UnityEngine.Random.Range(-2, 1);
                    }
                    BetValue += (int)(BetValue + UnityEngine.Random.Range(-2,2)); //Increases the bet a little
                    if (BetValue >= Players * 5 * 0.4 || BetValue > 20) //Should help avoiding stupid bets
                    {
                        BetValue = (int)(Players * 5 * 0.4);
                    }
                    if (BetValue <= CurrentBid) BetValue = ++CurrentBid;//Just to be sure AI WILL incriase the bet, so it follows the rules of the game
                    if (UnityEngine.Random.Range(0, 100) > ChanceOfTruth) // Challanges if my random number is bigger than the chance of failure
                    {
                        __instance.CallBluff();
                    }
                    else 
                    {
                        __instance.Raise((Byte)BetFace, (Byte)BetValue);
                    }
                }
            }
            return false;
        }
        public static double Factorial(double num) 
        { 
            if(num == 0) 
            {
                return 1;
            }
            for(int i = (int)num - 1; i > 0; i--) 
            {
                num *= i;
            }
            return num;
        }
    }
}
