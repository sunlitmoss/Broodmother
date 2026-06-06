using broodmother.broodmotherCode.Summons;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;

namespace broodmother.broodmotherCode.Powers.InsectPowers;

public class ScarabBeetlePower() : broodmotherPower
{
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Single;

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromPower<SwanSongPower>()
    ];

    public override bool ShouldDieLate(Creature creature)
    {
        if (Owner.Monster is not BroodmotherSummonModel summon) return true;
        return creature != summon.Summoner?.Creature;
    }

    public override async Task AfterPreventingDeath(Creature creature)
    {
        if (Owner.Monster is not BroodmotherSummonModel summon) return;
        if (summon.Summoner == null) return;
        await CreatureCmd.Heal(summon.Summoner.Creature, 1m);
        await PowerCmd.Apply<SwanSongPower>(new ThrowingPlayerChoiceContext(), summon.Summoner.Creature, 1m, Owner, null);
        await CreatureCmd.Kill(Owner);
    }
}