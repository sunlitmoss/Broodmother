using Broodmother.broodmotherCode.Summons;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;


namespace broodmother.broodmotherCode.Powers;
public class ResistancePower() : broodmotherPower
{
    public override PowerType Type =>
        PowerType.None;

    public override PowerStackType StackType =>
        PowerStackType.Counter;
}