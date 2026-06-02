using Broodmother.broodmotherCode.Powers;
using broodmother.broodmotherCode.Summons;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace broodmother.broodmotherCode.Powers;


public class ShrinkerBeetlePower() : broodmotherPower
{
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Single;

    public override async Task AfterApplied(Creature? applier, CardModel? cardSource)
    {
        var target = (Owner.Monster as ShrinkerBeetle)!.Target;
        if (target == null) return;
        await PowerCmd.Apply<ShrinkerBeetleShrinkPower>(new ThrowingPlayerChoiceContext(),
            target,
            1,
            Owner,
            null);
    }
}