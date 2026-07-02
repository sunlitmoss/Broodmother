// using MegaCrit.Sts2.Core.Commands;
// using MegaCrit.Sts2.Core.Entities.Cards;
// using MegaCrit.Sts2.Core.GameActions.Multiplayer;
// using MegaCrit.Sts2.Core.Localization.DynamicVars;
// using MegaCrit.Sts2.Core.ValueProps;
//
// namespace broodmother.broodmotherCode.Cards.ShiftCards;
//
// public class MetamorphicDefend() : ShiftCard<MetamorphicStrike>
// (1, CardType.Skill, CardRarity.Common,
//     TargetType.Self)
// {
//     protected override IEnumerable<DynamicVar> CanonicalVars =>
//         new List<DynamicVar> { new BlockVar(7m, ValueProp.Move) };
//
//     protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
//     {
//         await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);
//     }
//
//     protected override void OnUpgrade()
//     {
//         DynamicVars.Block.UpgradeValueBy(3m);
//     }
// }