using Broodmother.broodmotherCode.Summons;
using broodmother.broodmotherCode.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace broodmother.broodmotherCode.Cards;

public class BugSwarm() : broodmotherCard
(1, CardType.Attack, CardRarity.Basic,
    TargetType.AllEnemies)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar>
    {
        new DamageVar(3m, ValueProp.Move),
        new RepeatVar(3)
    };

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);

        await DamageCmd.Attack(DynamicVars.Damage.BaseValue).WithHitCount(DynamicVars.Repeat.IntValue).FromCard(this)
            .TargetingRandomOpponents(CombatState!)
            .Execute(choiceContext);

        var insects = BroodmotherInsectSlots.Occupants
            .Where(c => c is { Monster: IBroodmotherSummon summon } && summon.Summoner == Owner)
            .ToList();
        
        foreach (var insect in  insects)
            await CreatureCmd.Heal(insect!, 1m);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Repeat.UpgradeValueBy(1m);
    }
}