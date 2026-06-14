using broodmother.broodmotherCode.Summons;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace broodmother.broodmotherCode.Cards;


public class SwarmSacrifice() : broodmotherCard(2,
    CardType.Skill, CardRarity.Rare,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        foreach (var c in CombatState!.Creatures.Where(c => c.Monster is BroodmotherSummonModel).ToList())
        {
            var insect = c.Monster as BroodmotherSummonModel;
            if (insect!.Summoner == Owner) await CreatureCmd.Kill(c);
        }
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
}