﻿using NeoServer.Game.Common.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeoServer.Game.Contracts.Creatures
{
    public interface IVocation
    {
        string AttackSpeed { get; set; }
        ushort BaseSpeed { get; set; }
        string Clientid { get; set; }
        string Description { get; set; }
        IVocationFormula Formula { get; set; }
        string FromVoc { get; set; }
        ushort GainCap { get; set; }
        ushort GainHp { get; set; }
        ushort GainHpAmount { get; set; }
        ushort GainManaAmount { get; set; }
        byte GainHpTicks { get; set; }
        ushort GainMana { get; set; }
        byte GainManaTicks { get; set; }
        string GainSoulTicks { get; set; }
        string Id { get; set; }
        string ManaMultiplier { get; set; }
        string Name { get; set; }
        List<IVocationSkill> Skill { get; set; }
        byte SoulMax { get; set; }
        VocationType VocationType { get; }
    }

}
