using BaseLib.Abstracts;
using BaseLib.Utils;
using broodmother.broodmotherCode.Summons;
using Broodmother.broodmotherCode.Summons;
using broodmother.broodmotherCode.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.ValueProps;

namespace broodmother.broodmotherCode.Cards.InsectCards;

[Pool(typeof(TokenCardPool))]
public abstract class BroodmotherInsectCard : CustomCardModel
{
    public override CardPoolModel VisualCardPool => ModelDb.CardPool<TokenCardPool>();

    protected BroodmotherInsectCard(TargetType target = TargetType.Self)
        : base(0, CardType.Skill, CardRarity.Token, target)
    {
    }

    public override IEnumerable<CardKeyword> CanonicalKeywords => new List<CardKeyword> { CardKeyword.Exhaust };

    protected abstract IHoverTip InsectPowerTip { get; }

    protected virtual IEnumerable<IHoverTip> AdditionalHoverTips =>
        Enumerable.Empty<IHoverTip>();

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        new List<IHoverTip> { InsectPowerTip }
            .Concat(AdditionalHoverTips);
    
    protected override bool IsPlayable => BroodmotherInsectSlots.GetNextSlot() != -1;

    protected virtual Task ApplySummonPowers(PlayerChoiceContext choiceContext, Creature creature)
        => Task.CompletedTask;
    
    public async Task<Creature?> SummonInsect<TMonster>(PlayerChoiceContext choiceContext)
        where TMonster : MonsterModel, IBroodmotherSummon
    {
        var c = await CreatureCmd.Add<TMonster>(CombatState);
        var slot = BroodmotherInsectSlots.GetNextSlot();
        BroodmotherInsectSlots.OccupySlot(slot, c);
        (c.Monster as IBroodmotherSummon)!.SlotIndex = slot;
        var node = NCombatRoom.Instance?.GetCreatureNode(c);
        if (node != null) node.Position = BroodmotherInsectSlots.ActiveSlots[slot];
        await PowerCmd.Apply<MinionPower>(choiceContext, c, 1m, null, null);
        ApplySummonPowers(choiceContext, c);
        if (c.Monster is BroodmotherSummonModel summon)
        {
            await summon.OnPassive(CombatState);
            summon.Summoner = Owner;
        }
        return c;
    }

    protected async Task ApplyToRandomEnemy<TPower>(PlayerChoiceContext choiceContext, decimal amount)
        where TPower : PowerModel
    {
        var target = CombatState.RunState.Rng.CombatTargets.NextItem(
            CombatState.HittableEnemies.Where(c => !(c.Monster is IBroodmotherSummon)));
        if (target != null)
            await PowerCmd.Apply<TPower>(choiceContext, target, amount, Owner.Creature, this);
    }

    protected async Task DamageRandomEnemy(PlayerChoiceContext choiceContext, decimal amount, Creature? dealer = null)
    {
        var _damage = new DamageVar(amount, ValueProp.Move);
        var target = CombatState.RunState.Rng.CombatTargets.NextItem(
            CombatState.HittableEnemies.Where(c => !(c.Monster is IBroodmotherSummon)));
        if (target != null)
            await CreatureCmd.Damage(new ThrowingPlayerChoiceContext(), target, _damage, dealer, null);
    }

    public static async Task<CardModel?> CreateInHand<T>(Player owner, ICombatState combatState)
        where T : BroodmotherInsectCard
    {
        return (await CreateInHand<T>(owner, 1, combatState)).FirstOrDefault();
    }

    public static async Task<IEnumerable<CardModel>> CreateInHand<T>(Player owner, int count, ICombatState combatState)
        where T : BroodmotherInsectCard
    {
        if (count == 0) return Array.Empty<CardModel>();
        if (CombatManager.Instance.IsOverOrEnding) return Array.Empty<CardModel>();

        var cards = new List<CardModel>();
        for (var i = 0; i < count; i++)
            cards.Add(combatState.CreateCard<T>(owner));

        await CardPileCmd.AddGeneratedCardsToCombat(cards, PileType.Hand, owner);
        return cards;
    }
}