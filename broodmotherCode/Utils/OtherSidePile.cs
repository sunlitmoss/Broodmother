using BaseLib.Abstracts;
using BaseLib.Patches.Content;
using Godot;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;

namespace broodmother.broodmotherCode.Utils;

public class OtherSidePile : CustomPile
{
    [CustomEnum] public static PileType OtherSide;
    
    public OtherSidePile() : base(OtherSide) { }

    public override bool CardShouldBeVisible(CardModel card) => false;

    public override Vector2 GetTargetPosition(CardModel model, Vector2 size) =>
        new Vector2(-99999, -9999);
}