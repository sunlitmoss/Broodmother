using broodmother.broodmotherCode.Cards;
using broodmother.broodmotherCode.Powers;
using broodmother.broodmotherCode.Summons;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace broodmother.broodmotherCode;

public class Blight() : broodmotherCard(1,
    CardType.Skill, CardRarity.Uncommon,
    TargetType.AllEnemies)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => new List<CardKeyword> { CardKeyword.Exhaust, CardKeyword.Innate };
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DynamicVar("InfestationAmount", 3m),
        new DynamicVar("WeakAmount", 1m)
    ];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        await PowerCmd.Apply<InfestationPower>(choiceContext,
            CombatState!.HittableEnemies.Where(c => c.Monster is not BroodmotherSummonModel),
            DynamicVars["InfestationAmount"].BaseValue,
            Owner.Creature,
            this);
        await PowerCmd.Apply<WeakPower>(choiceContext,
            CombatState.HittableEnemies.Where(c => c.Monster is not BroodmotherSummonModel),
            DynamicVars["WeakAmount"].BaseValue,
            Owner.Creature,
            this);
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
}