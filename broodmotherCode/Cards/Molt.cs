using broodmother.broodmotherCode.Powers;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Encounters;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Screens.DailyRun;
using MegaCrit.Sts2.Core.ValueProps;

namespace broodmother.broodmotherCode.Cards;

public class Molt() : broodmother.broodmotherCode.Cards.BroodmotherCard
    (0, CardType.Skill, CardRarity.Rare, TargetType.Self){
    
    private new bool CanPlay
    {
        get
        {
            int num = CombatManager.Instance.History.CardPlaysFinished.Count((CardPlayFinishedEntry e) => e.HappenedThisTurn(base.CombatState) && e.CardPlay.Card.Owner == base.Owner);
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
            await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
            await PowerCmd.Apply<MoltThornsPower>(choiceContext, base.Owner.Creature,
                base.DynamicVars["ThornsPower"].BaseValue, base.Owner.Creature, this);
            await PowerCmd.Apply<MoltStrengthPower>(choiceContext, base.Owner.Creature,
                base.DynamicVars.Strength.BaseValue, base.Owner.Creature, this);
        }
        else
        {
            await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.Block, cardPlay);
        }
    }
    protected override void OnUpgrade()
    {
        base.DynamicVars["ThornsPower"].UpgradeValueBy(2m);
    }
}