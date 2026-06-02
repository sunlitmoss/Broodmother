// using broodmother.broodmotherCode.Cards.InsectCards;
// using MegaCrit.Sts2.Core.Commands;
// using MegaCrit.Sts2.Core.Entities.Cards;
// using MegaCrit.Sts2.Core.GameActions.Multiplayer;
// using MegaCrit.Sts2.Core.Localization.DynamicVars;
// using MegaCrit.Sts2.Core.ValueProps;
//
// namespace broodmother.broodmotherCode.Cards.ShiftCards;
//
// public class PocketHive() : ShiftCard<>
// (1, CardType.Skill, CardRarity.Uncommon,
//     TargetType.Self)
// {
//     protected override IEnumerable<DynamicVar> CanonicalVars =>
//     [
//         new DynamicVar("Count", 2)
//     ];
//
//     protected override async Task OnPlay(
//         PlayerChoiceContext choiceContext,
//         CardPlay play)
//     {
//         for (int i = 0; i < DynamicVars.Count; i++)
//         {
//             await ReleaseWaspNest.CreateInHand(Owner, CombatState!);
//         }
//     }
//
//     protected override void OnUpgrade()
//     {
//         DynamicVars["Count"].UpgradeValueBy(1m);
//     }
// }