using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace broodmother.broodmotherCode.Powers;

public abstract class TemporaryThornsPower : PowerModel, ITemporaryPower
{
    private bool _shouldIgnoreNextInstance;

    public override PowerType Type
    {
        get
        {
            if (!IsPositive) return PowerType.Debuff;
            return PowerType.Buff;
        }
    }

    public override PowerStackType StackType => PowerStackType.Counter;

    public abstract AbstractModel OriginModel { get; }

    public PowerModel InternallyAppliedPower => ModelDb.Power<ThornsPower>();

    protected virtual bool IsPositive => true;

    private int Sign
    {
        get
        {
            if (!IsPositive) return -1;
            return 1;
        }
    }

    public override LocString Title
    {
        get
        {
            var originModel = OriginModel;
            if (!(originModel is CardModel cardModel))
            {
                if (!(originModel is PotionModel potionModel))
                {
                    if (originModel is RelicModel relicModel) return relicModel.Title;
                    throw new InvalidOperationException();
                }

                return potionModel.Title;
            }

            return cardModel.TitleLocString;
        }
    }

    public override LocString Description => new("powers",
        IsPositive ? "TEMPORARY_THORNS_POWER.description" : "TEMPORARY_THORNS_DOWN.description");

    protected override string SmartDescriptionLocKey => "BROODMOTHER-TEMPORARY_THORNS_POWER.smartDescription";

    protected override IEnumerable<IHoverTip> ExtraHoverTips
    {
        get
        {
            var list = new List<IHoverTip>();
            var list2 = list;
            var originModel = OriginModel;
            IEnumerable<IHoverTip> collection;
            if (!(originModel is CardModel card))
            {
                if (!(originModel is PotionModel model))
                {
                    if (!(originModel is RelicModel relic)) throw new InvalidOperationException();
                    collection = HoverTipFactory.FromRelic(relic);
                }
                else
                {
                    collection = new List<IHoverTip> { HoverTipFactory.FromPotion(model) };
                }
            }
            else
            {
                collection = new List<IHoverTip> { HoverTipFactory.FromCard(card) };
            }

            list2.AddRange(collection);
            list.Add(HoverTipFactory.FromPower<ThornsPower>());
            return new List<IHoverTip>(list);
        }
    }

    public void IgnoreNextInstance()
    {
        _shouldIgnoreNextInstance = true;
    }

    public override async Task BeforeApplied(Creature target, decimal amount, Creature? applier, CardModel? cardSource)
    {
        if (_shouldIgnoreNextInstance)
            _shouldIgnoreNextInstance = false;
        else
            await PowerCmd.Apply<ThornsPower>(new ThrowingPlayerChoiceContext(), target, (decimal)Sign * amount,
                applier, cardSource, true);
    }

    public override async Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power,
        decimal amount, Creature? applier, CardModel? cardSource)
    {
        if (!(amount == (decimal)Amount) && power == this)
        {
            if (_shouldIgnoreNextInstance)
                _shouldIgnoreNextInstance = false;
            else
                await PowerCmd.Apply<ThornsPower>(choiceContext, Owner, (decimal)Sign * amount, applier, cardSource,
                    true);
        }
    }

    public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side,
        IEnumerable<Creature> participants)
    {
        if (side == CombatSide.Enemy)
        {
            Flash();
            await PowerCmd.Remove(this);
            await PowerCmd.Apply<ThornsPower>(choiceContext, Owner, -Sign * Amount, Owner, null);
        }
    }
}