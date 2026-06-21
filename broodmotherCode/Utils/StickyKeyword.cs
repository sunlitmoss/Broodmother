using BaseLib.Abstracts;
using broodmother.broodmotherCode.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

public class StickyModifier : CardModifier
{
    public int RemainingTurns;

    public override void ModifyDescriptionPost(Creature? target, ref string description)
    {
        description += $"\n[gold]Sticky[/gold] ({RemainingTurns}).";
    }

    public override async Task BeforeSideTurnStart(
        PlayerChoiceContext choiceContext,
        CombatSide side,
        IReadOnlyList<Creature> participants,
        ICombatState combatState)
    {
        if (side != CombatSide.Player) return;
        if (Owner?.Pile?.Type != PileType.Hand) return;

        RemainingTurns--;
        if (RemainingTurns <= 0)
        {
            Owner.RemoveKeyword(CardKeyword.Retain);
            Owner.RemoveKeyword(BroodmotherKeywords.Sticky);
            RemoveModifier(Owner, this);
        }
    }

    public override void StoreSaveData(ModifierSave save)
        => save.IntProperties["RemainingTurns"] = RemainingTurns;

    public override void LoadSaveData(ModifierSave save)
        => RemainingTurns = save.IntProperties.GetValueOrDefault("RemainingTurns", 0);
}