// using broodmother.broodmotherCode.Powers;
// using MegaCrit.Sts2.Core.Commands;
// using MegaCrit.Sts2.Core.Entities.Cards;
// using MegaCrit.Sts2.Core.GameActions.Multiplayer;
// using MegaCrit.Sts2.Core.HoverTips;
// using MegaCrit.Sts2.Core.Localization.DynamicVars;
//
// namespace broodmother.broodmotherCode.Cards;
//
// public class WillingSacrifice() : broodmotherCard(0,
//     CardType.Skill, CardRarity.Uncommon,
//     BroodmotherTargetTypes.AnyInsect)
// {
//     protected override IEnumerable<DynamicVar> CanonicalVars => [];
//     
//     protected override IEnumerable<IHoverTip> ExtraHoverTips =>
//         new List<IHoverTip> {HoverTipFactory.FromPower<SacrificePower>() };
//
//     public override IEnumerable<CardKeyword> CanonicalKeywords => new List<CardKeyword> { CardKeyword.Exhaust };
//     protected override async Task OnPlay(
//         PlayerChoiceContext choiceContext,
//         CardPlay play)
//     {
//         await PowerCmd.Apply<SacrificePower>(choiceContext, play.Target!, 1m, Owner.Creature, this);
//     }
//
//     protected override void OnUpgrade()
//     {
//         RemoveKeyword(CardKeyword.Exhaust);
//     }
// }