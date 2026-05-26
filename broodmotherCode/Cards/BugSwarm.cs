using broodmother.broodmotherCode.Cards;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace broodmother.broodmotherCode.Cards;

public class BugSwarm() : broodmotherCard
    (1,CardType.Attack, CardRarity.Basic,
    TargetType.AllEnemies)
{
    
    protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar>
    {
        new DamageVar(2m, ValueProp.Move),
        new DynamicVar("Times", 2m)
    };

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Attack", Owner.Character.AttackAnimDelay);
        for (int i = 0; i < base.DynamicVars["Times"].IntValue; i++)
        {
            await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                        .FromCard(this)
                        .TargetingAllOpponents(CombatState)
                        .Execute(choiceContext);
        }
        
    }

    protected override void OnUpgrade() =>
        base.DynamicVars["Times"].UpgradeValueBy(1m);
}