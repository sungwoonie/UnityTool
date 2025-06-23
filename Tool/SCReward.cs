using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StarCloudgamesLibrary
{
    [Serializable]
    public class SCReward
    {
        public string rewardName;
        public RewardType rewardType;
        public RewardID rewardID;
        public double amount;

        public SCReward(string rewardName, RewardType rewardType, RewardID rewardID, double amount)
        {
            this.rewardName = rewardName;
            this.rewardType = rewardType;
            this.rewardID = rewardID;
            this.amount = amount;
        }

        public SCReward(string rewardName, int rewardType, int rewardID, double amount)
        {
            this.rewardName = rewardName;
            this.rewardType = (RewardType)rewardType;
            this.rewardID = (RewardID)rewardID;
            this.amount = amount;
        }

        public SCReward(string rewardType, string rewardID, string amount, string rewardName)
        {
            this.rewardType = (RewardType)int.Parse(rewardType);
            this.rewardID = (RewardID)int.Parse(rewardID);
            this.amount = double.Parse(amount);
            this.rewardName = rewardName;
        }
    }

    /// <summary>
    /// enum for database
    /// </summary>
    public enum RewardType
    {
        None = 0,
        Item = 1,
        Package = 2,
        UserData = 3
    }

    /// <summary>
    /// 1 ~ 99 = currency
    /// 100 = paidCach
    /// 1000 ~ 1099 = direct currency
    /// </summary>
    public enum RewardID
    {
        paidCash = 100,

        None = 0,
        gold = 1,
        freeCash = 2,
        eventCurrency = 3,

        beyondStone = 10,
        relicStone = 11,
        enhanceStone = 12,
        levelStone = 13,
        awakeningStone = 14,
        skillStone = 15,
        dice = 16,
        magicStone = 17,
        magicPower = 18,

        lowPetFood = 20,
        middlePetFood = 21,
        highPetFood = 22,

        relicDungeonKey = 30,
        equipmentDungeonKey = 31,
        awakeningDungeonKey = 32,
        skillDungeonKey = 33,
        magicPowerDungeonKey = 34,
        limitBreakTicket = 35,
        pandaTicket = 36,
        advancementTicket = 37,

        equipmentGachaCoupon = 40,
        skillGachaCoupon = 41,
        relicGachaCoupon = 42,
        petGachaCoupon = 43,
        patrolSkipCoupon = 44,
        rouletteTicket = 45,
        
        mastery = 50,
        mileague = 51,
        vipPoint = 52,
        guildSkillPoint = 53,

        randomDungeonKeyDirect = 1001,
        AdRemove = 1002,

        directEquipmentGacha = 1010,
        directRelicGacha = 1011,
        directSkillGacha = 1012,

        sTierEquipmentDirectGacha = 1020,
        ssTierEquipmentDirectGacha = 1021,
        lTierEquipmentDirectGacha = 1022,
        llTierEquipmentDirectGacha = 1023,
        sTierSkillDirectGacha = 1024,
        ssTierSkillDirectGacha = 1025,
        lTierSkillDirectGacha = 1026,
        sTierRelicDirectGacha = 1027,
        ssTierRelicDirectGacha = 1028,
        lTierRelicDirectGacha = 1029,
        llTierRelicDirectGacha = 1030,
    }
}