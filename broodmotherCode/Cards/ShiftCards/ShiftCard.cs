using BaseLib.Utils;
using broodmother.broodmotherCode.Character;
using broodmother.broodmotherCode.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;

namespace broodmother.broodmotherCode.Cards.ShiftCards;

[Pool(typeof(broodmotherCardPool))]
public abstract class ShiftCard<TAlt>(int cost, CardType type, CardRarity rarity, TargetType target)
    : broodmotherCard(cost, type, rarity, target), IShiftCard
    where TAlt : CardModel
{
    public sealed override IEnumerable<CardKeyword> CanonicalKeywords => 
        new List<CardKeyword> { BroodmotherKeywords.Shift }
            .Concat(AdditionalKeywords);

    protected virtual IEnumerable<IHoverTip> AdditionalHoverTips =>
        Enumerable.Empty<IHoverTip>();

    protected virtual IEnumerable<CardKeyword> AdditionalKeywords =>
        Enumerable.Empty<CardKeyword>();
    protected sealed override IEnumerable<IHoverTip> ExtraHoverTips =>
        new List<IHoverTip> { HoverTipFactory.FromCard<TAlt>() }
            .Concat(AdditionalHoverTips);

    public Type AlternateCardType => typeof(TAlt);

    public CardModel GetAlternateCard()
    {
        return CombatState!.CreateCard<TAlt>(Owner);
    }
}