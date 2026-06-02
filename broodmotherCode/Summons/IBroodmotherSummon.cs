using MegaCrit.Sts2.Core.Entities.Players;

namespace Broodmother.broodmotherCode.Summons;

public interface IBroodmotherSummon
{
    int SlotIndex { get; set; }
    public Player? Summoner { get; set; }
}