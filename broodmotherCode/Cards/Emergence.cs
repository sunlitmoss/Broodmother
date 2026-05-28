using broodmother.broodmotherCode.Cards;
using broodmother.broodmotherCode.Powers;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace broodmother.broodmotherCode.Cards;

public class Emergence() : broodmotherCard(1,
    CardType.Skill, CardRarity.Rare,
    TargetType.Self)
{
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new EnergyVar(2),
        new CardsVar(2)
    ];

    private new bool CanPlay
    {
        get
        {
            int num = CombatManager.Instance.History.CardPlaysFinished.Count((CardPlayFinishedEntry e) => e.HappenedThisTurn(base.CombatState) && e.CardPlay.Card.Owner == base.Owner);
            return num == 0;
        }
    }

    
    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);

        if (CanPlay)
        {
            await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.IntValue, Owner);
        }
        else
        {
            await PlayerCmd.GainEnergy(DynamicVars.Energy.IntValue, Owner);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Cards.UpgradeValueBy(1m);
        DynamicVars.Energy.UpgradeValueBy(1m);
    }
}