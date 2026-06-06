using broodmother.broodmotherCode.Powers;
using broodmother.broodmotherCode.Powers.InsectPowers;
using broodmother.broodmotherCode.Summons;
using Broodmother.broodmotherCode.Summons;
using broodmother.broodmotherCode.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Rooms;

namespace broodmother.broodmotherCode.Relics;

public class HeartOfTheHive() : broodmotherRelic
{
    public override RelicRarity Rarity =>
        RelicRarity.Starter;

    private async Task<Creature?> SummonInsect<TMonster, TPower>(PlayerChoiceContext choiceContext,
        ICombatState combatState)
        where TMonster : MonsterModel, IBroodmotherSummon
        where TPower : broodmotherPower
    {
        var c = await CreatureCmd.Add<TMonster>(combatState);
        var slot = BroodmotherInsectSlots.GetNextSlot();
        BroodmotherInsectSlots.OccupySlot(slot, c);
        (c.Monster as IBroodmotherSummon)!.SlotIndex = slot;
        var node = NCombatRoom.Instance?.GetCreatureNode(c);
        if (node != null) node.Position = BroodmotherInsectSlots.ActiveSlots[slot];
        await PowerCmd.Apply<MinionPower>(choiceContext, c, 1m, null, null);
        await PowerCmd.Apply<TPower>(choiceContext, c, 1m, null, null);
        if (c.Monster is BroodmotherSummonModel summon) summon.Summoner = Owner;
        return c;
    }

    public override async Task BeforeSideTurnStart(PlayerChoiceContext choiceContext, CombatSide side,
        IReadOnlyList<Creature> participants, ICombatState combatState)
    {
        if (participants.Contains(Owner.Creature) && Owner.PlayerCombatState!.TurnNumber <= 1)
            await SummonInsect<WaspNest, WaspNestPower>(choiceContext, combatState);
    }
}