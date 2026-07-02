// using broodmother.broodmotherCode.Utils;
// using MegaCrit.Sts2.Core.Entities.Cards;
// using MegaCrit.Sts2.Core.GameActions.Multiplayer;
// using MegaCrit.Sts2.Core.Localization.DynamicVars;
//
// namespace broodmother.broodmotherCode.Cards.ShiftCards;
//
// public class Lunge() : ShiftCard<Recoil>(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
// {
//     protected override IEnumerable<DynamicVar> CanonicalVars => [];
//
//     protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
//     {
//         
//     }
//
//     protected override void OnUpgrade()
//     {
//     }
// }
//
// public class Recoil() : ShiftCard<Lunge>(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
// {
//     protected override IEnumerable<DynamicVar> CanonicalVars => [];
//
//     protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
//     {
//     }
//
//     protected override void OnUpgrade()
//     {
//     }
// }