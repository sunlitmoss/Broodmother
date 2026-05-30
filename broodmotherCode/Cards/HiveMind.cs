using broodmother.broodmotherCode.Summons;
using broodmother.broodmotherCode.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace broodmother.broodmotherCode.Cards;

public class HiveMind() : broodmotherCard(1,
    CardType.Skill, CardRarity.Uncommon,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DynamicVar("Times", 1m)
    ];

    private Creature? _insect;
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        List<Creature?> insects = BroodmotherInsectSlots.Occupants
            .Where(c => c != null)
            .Select(c => c)
            .ToList();
        
        if (insects.Count == 0) return;
        
        for (int i = 1; i <= DynamicVars["Times"].IntValue; i++)
        {
            Creature insect = CombatState.RunState.Rng.CombatTargets.NextItem(insects)!;
            if (insect.Monster is BroodmotherSummonModel summon) await summon.OnPassive(CombatState);
        }
        
    }

    protected override void OnUpgrade()
    {
        DynamicVars["Times"].UpgradeValueBy(1m);
    }
}