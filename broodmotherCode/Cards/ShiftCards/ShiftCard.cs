using BaseLib.Abstracts;
using BaseLib.Utils;
using broodmother.broodmotherCode.Character;
using broodmother.broodmotherCode.Powers;
using broodmother.broodmotherCode.Summons;
using Broodmother.broodmotherCode.Summons;
using broodmother.broodmotherCode.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.ValueProps;

namespace broodmother.broodmotherCode.Cards.ShiftCards;

[Pool(typeof(broodmotherCardPool))]
public abstract class ShiftCard<TAlt>(int cost, CardType type, CardRarity rarity, TargetType target)
    : CustomCardModel(cost, type, rarity, target), IShiftCard
        where TAlt : CardModel
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => new List<CardKeyword> { BroodmotherKeywords.Shift };

    protected virtual IEnumerable<IHoverTip> AdditionalHoverTips => 
        Enumerable.Empty<IHoverTip>();

    protected override IEnumerable<IHoverTip> ExtraHoverTips => 
        new List<IHoverTip> { HoverTipFactory.FromCard<TAlt>() }
            .Concat(AdditionalHoverTips);
    
    public Type AlternateCardType => typeof(TAlt);

    public CardModel GetAlternateCard() => CombatState.CreateCard<TAlt>(Owner);

}