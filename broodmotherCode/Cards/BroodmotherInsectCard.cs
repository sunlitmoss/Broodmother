using BaseLib.Abstracts;
using BaseLib.Utils;
using broodmother.broodmotherCode.Character;
using broodmother.broodmotherCode.Powers;
using broodmother.broodmotherCode.Summons;
using Broodmother.broodmotherCode.Summons;
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
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.ValueProps;

namespace broodmother.broodmotherCode.Cards;

[Pool(typeof(TokenCardPool))]
public abstract class BroodmotherInsectCard : CustomCardModel
{
    public override CardPoolModel VisualCardPool => ModelDb.CardPool<TokenCardPool>();
    protected BroodmotherInsectCard(int cost) 
        : base(cost, CardType.Skill, CardRarity.Token, TargetType.Self)
    {
    }

    public override IEnumerable<CardKeyword> CanonicalKeywords => new List<CardKeyword> { CardKeyword.Exhaust };

    protected abstract IHoverTip InsectPowerTip { get; }
    protected virtual IHoverTip OtherTip { get; }

    protected override IEnumerable<IHoverTip> ExtraHoverTips => 
        new List<IHoverTip> { InsectPowerTip, OtherTip };
    
    protected override bool IsPlayable => BroodmotherInsectSlots.GetNextSlot() != -1;
    
    protected async Task<Creature?> SummonInsect<TMonster, TPower>(PlayerChoiceContext choiceContext)
        where TMonster : MonsterModel, IBroodmotherSummon
        where TPower : broodmotherPower
    {
        Creature c = await CreatureCmd.Add<TMonster>(base.CombatState); 
        int slot = BroodmotherInsectSlots.GetNextSlot();
        BroodmotherInsectSlots.OccupySlot(slot, c);
        (c.Monster as IBroodmotherSummon)!.SlotIndex = slot;
        var node = NCombatRoom.Instance?.GetCreatureNode(c);
        if (node != null) node.Position = BroodmotherInsectSlots.InsectSlots[slot];
        await PowerCmd.Apply<TPower>(choiceContext, c, 1m, null, null);
        await PowerCmd.Apply<MinionPower>(choiceContext, c, 1m, null, null);
        if (c.Monster is BroodmotherSummonModel summon)
            await summon.OnPassive(base.CombatState);
        return c;
    }
    
    protected async Task ApplyToRandomEnemy<TPower>(PlayerChoiceContext choiceContext, decimal amount)
        where TPower : PowerModel
    {
        Creature? target = base.CombatState.RunState.Rng.CombatTargets.NextItem(
            base.CombatState.HittableEnemies.Where(c => !(c.Monster is IBroodmotherSummon)));
        if (target != null)
            await PowerCmd.Apply<TPower>(choiceContext, target, amount, base.Owner.Creature, this);
    }

    protected async Task DamageRandomEnemy(PlayerChoiceContext choiceContext, decimal amount, Creature? dealer = null)
    {
        DamageVar _damage = new DamageVar(amount, ValueProp.Move);
        Creature? target = base.CombatState.RunState.Rng.CombatTargets.NextItem(
            base.CombatState.HittableEnemies.Where(c => !(c.Monster is IBroodmotherSummon)));
        if (target != null)
            await CreatureCmd.Damage(new ThrowingPlayerChoiceContext(), target, _damage, dealer, null);
    }
    public static async Task<CardModel?> CreateInHand<T>(Player owner, ICombatState combatState) where T : BroodmotherInsectCard
    {
        return (await CreateInHand<T>(owner, 1, combatState)).FirstOrDefault();
    }

    public static async Task<IEnumerable<CardModel>> CreateInHand<T>(Player owner, int count, ICombatState combatState) where T : BroodmotherInsectCard
    {
        if (count == 0) return Array.Empty<CardModel>();
        if (CombatManager.Instance.IsOverOrEnding) return Array.Empty<CardModel>();
        
        List<CardModel> cards = new List<CardModel>();
        for (int i = 0; i < count; i++)
            cards.Add(combatState.CreateCard<T>(owner));
        
        await CardPileCmd.AddGeneratedCardsToCombat(cards, PileType.Hand, owner);
        return cards;
    }

}