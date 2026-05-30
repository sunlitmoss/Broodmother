using broodmother.broodmotherCode.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.CommonUi;

namespace broodmother.broodmotherCode.Cards;


public class Instar() : broodmotherCard(1,
    CardType.Skill, CardRarity.Rare,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        foreach (var card in PileType.Hand.GetPile(Owner).Cards.Where(c => c.Keywords.Contains(BroodmotherKeywords.Shift)))
        {
            if (ShiftRegistries.ShiftPairs.TryGetValue(card.GetType(),
                    out var altType))
            {
                var modelDbCard = typeof(ModelDb).GetMethod("Card",
                        System.Type.EmptyTypes)!
                    .MakeGenericMethod(altType)
                    .Invoke(null,
                        null) as CardModel;
                var alt = card.CardScope!.CreateCard(modelDbCard!,
                    card.Owner);
                await CardCmd.Transform(new CardTransformation(card,
                        alt).Yield(),
                    null,
                    CardPreviewStyle.None);
                if (card.IsUpgraded && alt.IsUpgradable)
                    CardCmd.Upgrade(alt);
                return;
            }


            if (ShiftRegistries.CombatPairs.TryGetValue(card.GetHashCode(),
                    out (Type altTypeC, bool wasUpgraded) tuple))
            {
                var modelDbCard = typeof(ModelDb).GetMethod("Card",
                        System.Type.EmptyTypes)!
                    .MakeGenericMethod(tuple.altTypeC)
                    .Invoke(null,
                        null) as CardModel;
                
                var alt = card.CardScope!.CreateCard(modelDbCard!,
                    card.Owner);
                alt.AddKeyword(BroodmotherKeywords.Shift);
                if (tuple.wasUpgraded)
                    CardCmd.Upgrade(alt);
                await CardCmd.Transform(new CardTransformation(card,
                        alt).Yield(),
                    null,
                    CardPreviewStyle.None);
                ShiftRegistries.CombatPairs.Remove(card.GetHashCode());
                ShiftRegistries.CombatPairs[alt.GetHashCode()] = (card.GetType(), card.IsUpgraded);
                return;
            }
        }
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
}