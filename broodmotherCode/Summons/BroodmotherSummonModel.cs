using BaseLib.Abstracts;
using Broodmother.broodmotherCode.Summons;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;

namespace broodmother.broodmotherCode.Summons;

public abstract class BroodmotherSummonModel : CustomMonsterModel, IBroodmotherSummon
{
    public int SlotIndex { get; set; } = -1;
    public override LocString Title => new LocString("monsters", "BROODMOTHER-" + GetType().Name.ToUpper() + ".name");
}