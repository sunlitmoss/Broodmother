using BaseLib.Abstracts;
using JetBrains.Annotations;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace broodmother.broodmotherCode.Utils;

#pragma warning disable CS0618 // Type or member is obsolete
[UsedImplicitly]
public class BiteHelper() : CustomSingletonModel(true, false)
#pragma warning restore CS0618 // Type or member is obsolete
{
    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Card.Keywords.Contains(BroodmotherKeywords.Bite))
        {
            var player = cardPlay.Card.Owner;
            var card = cardPlay.Card;

            var increaseAmt = card.DynamicVars.TryGetValue("BiteAmt", out var amt) ? amt.IntValue : 1;
            
            var allBites =
                player.PlayerCombatState!.AllCards.Where(c => c.Keywords.Contains(BroodmotherKeywords.Bite));

            foreach (var bite in allBites)
                bite.DynamicVars.Damage.BaseValue += increaseAmt;
        }
    }
}