using broodmother.broodmotherCode.Cards;
using broodmother.broodmotherCode.Powers;
using Broodmother.broodmotherCode.Summons;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace broodmother.broodmotherCode;

public class Blight() : broodmotherCard(1,
    CardType.Skill, CardRarity.Uncommon,
    TargetType.AllEnemies)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => new List<CardKeyword> { CardKeyword.Exhaust, CardKeyword.Innate };
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DynamicVar("InfestationAmount", 3m)
    ];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        await PowerCmd.Apply<InfestationPower>(choiceContext,
            CombatState.HittableEnemies.Where(c => c is not IBroodmotherSummon),
            DynamicVars["InfestationAmount"].BaseValue,
            Owner.Creature,
            this);
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
}