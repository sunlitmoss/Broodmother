using broodmother.broodmotherCode.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace broodmother.broodmotherCode.Cards;

public class Cocoon() : broodmotherCard(1,
    CardType.Skill, CardRarity.Uncommon,
    TargetType.Self)
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        new List<IHoverTip> { HoverTipFactory.FromKeyword(BroodmotherKeywords.Shift) };

    protected override IEnumerable<DynamicVar> CanonicalVars =>
        new List<DynamicVar>
        {
            new BlockVar(7m, ValueProp.Move)
        };

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, play);

        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        var validCards = PileType.Hand.GetPile(Owner).Cards
            .Where(c => !c.Keywords.Contains(BroodmotherKeywords.Shift) &&
                        c.Type != CardType.Power &&
                        !c.Keywords.Contains(CardKeyword.Ethereal))
            .ToList();

        if (validCards.Count > 2)
        {
            var list = (await CardSelectCmd.FromHand(
                    choiceContext,
                    Owner,
                    new CardSelectorPrefs(new LocString("gameplay_ui", "CHOOSE_CARD_HEADER"), 2),
                    (CardModel c) => !c.Keywords.Contains(BroodmotherKeywords.Shift) &&
                                     c.Type != CardType.Power &&
                                     !c.Keywords.Contains(CardKeyword.Ethereal),
                    this))
                .ToList();
            var card1 = list[0];
            card1.AddKeyword(BroodmotherKeywords.Shift);
            var card2 = list[1];
            card2.AddKeyword(BroodmotherKeywords.Shift);
            ShiftRegistries.RegisterCombatPairs(card1, card2);
            await CardPileCmd.RemoveFromCombat(card2);
        }

        if (validCards.Count == 2)
        {
            var card1 = (await CardSelectCmd.FromHand(choiceContext, Owner,
                new CardSelectorPrefs(new LocString("gameplay_ui", "CHOOSE_CARD_HEADER"), 1),
                (CardModel c) => !c.Keywords.Contains(BroodmotherKeywords.Shift) &&
                                 c.Type != CardType.Power &&
                                 !c.Keywords.Contains(CardKeyword.Ethereal), this)).FirstOrDefault()!;
            validCards.Remove(card1);
            card1.AddKeyword(BroodmotherKeywords.Shift);
            var card2 = validCards.FirstOrDefault()!;
            card2.AddKeyword(BroodmotherKeywords.Shift);
            ShiftRegistries.RegisterCombatPairs(card1, card2);
            await CardPileCmd.RemoveFromCombat(card2);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(3m);
    }
}