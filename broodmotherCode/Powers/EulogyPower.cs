using broodmother.broodmotherCode.Summons;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace broodmother.broodmotherCode.Powers;

public class EulogyPower() : broodmotherPower
{
    public override PowerType Type =>
        PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;

    public override async Task AfterDeath(PlayerChoiceContext choiceContext, Creature creature, bool wasRemovalPrevented, float deathAnimLength)
    {
        if (creature is { IsMonster: true, Monster: BroodmotherSummonModel insect } && insect.Summoner == Owner.Player)
        {
            for (int i = 0; i < Amount; i++)
            {
                await CardPileCmd.Draw(choiceContext, Owner.Player!);
            }
        }
    }
}