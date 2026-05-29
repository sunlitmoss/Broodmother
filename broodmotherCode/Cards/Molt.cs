using broodmother.broodmotherCode.Powers;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace broodmother.broodmotherCode.Cards;

public class Molt() : broodmotherCard
    (0, CardType.Skill, CardRarity.Rare, TargetType.Self)
{
    private new bool CanPlay
    {
        get
        {
            var num = CombatManager.Instance.History.CardPlaysFinished.Count((CardPlayFinishedEntry e) =>
                e.HappenedThisTurn(CombatState) && e.CardPlay.Card.Owner == Owner);
            return num == 0;
        }
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar>
    {
        new PowerVar<StrengthPower>(-1m),
        new PowerVar<ThornsPower>(3m),
        new BlockVar(5m, ValueProp.Move)
    };

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        new List<IHoverTip> { HoverTipFactory.FromPower<StrengthPower>(), HoverTipFactory.FromPower<ThornsPower>() };


    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (CanPlay)
        {
            await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
            await PowerCmd.Apply<MoltThornsPower>(choiceContext, Owner.Creature,
                DynamicVars["ThornsPower"].BaseValue, Owner.Creature, this);
            await PowerCmd.Apply<MoltStrengthPower>(choiceContext, Owner.Creature,
                DynamicVars.Strength.BaseValue, Owner.Creature, this);
        }
        else
        {
            await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars["ThornsPower"].UpgradeValueBy(2m);
    }
}