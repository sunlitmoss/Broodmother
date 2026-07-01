using broodmother.broodmotherCode.Summons;
using Broodmother.broodmotherCode.Summons;
using broodmother.broodmotherCode.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace broodmother.broodmotherCode.Cards;


public class HiveCall() : broodmotherCard(1,
    CardType.Skill, CardRarity.Uncommon,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [];
    
    public override IEnumerable<CardKeyword> CanonicalKeywords =>
        [
            CardKeyword.Exhaust
        ];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        var insects = BroodmotherInsectSlots.Occupants
            .Where(c => c is { Monster: IBroodmotherSummon summon } && summon.Summoner == Owner)
            .ToList();
        
        if  (insects.Count == 0) return;
        
        var randomInsect = CombatState!.RunState.Rng.CombatTargets.NextItem(insects)!;
        
        var summon = (randomInsect.Monster as BroodmotherSummonModel)!;
        await summon.CreateReleaseCard(CombatState!, Owner);
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
}