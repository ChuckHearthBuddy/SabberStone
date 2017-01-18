﻿using System;
using System.Reflection;
using log4net;
using SabberStone.Model;
using SabberStone.Tasks;

namespace SabberStone.Enchants
{
    public enum EnchantmentActivation
    {
        BATTLECRY,
        DEATHRATTLE,
        BOARD,
        HAND,
        DECK,
        SECRET,
        SPELL,
        WEAPON,
        NONE
    }

    public enum EnchantmentArea
    {
        TARGET,

        HERO,
        OP_HERO,
        HEROES,

        BOARD,
        OP_BOARD,
        BOARDS,

        HAND,
        OP_HAND,
        HANDS,

        NONE,
        WEAPON,
        SELF,

        GAME,

        CONTROLLER,
        OP_CONTROLLER,
        SECRET
    }

    public class Enchantment
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public EnchantmentArea Area { get; set; } = EnchantmentArea.NONE;

        public EnchantmentActivation Activation { get; set; } = EnchantmentActivation.NONE;

        public Enchant Enchant { get; set; }

        public Trigger Trigger { get; set; }

        public ISimpleTask SingleTask { get; set; }

        public void Activate(Controller controller, IPlayable source, IPlayable target = null)
        {
            // execute task straight over
            if (SingleTask != null)
            {
                // clone task here
                var clone = SingleTask.Clone();
                clone.Game = controller.Game;
                clone.Controller = controller;
                clone.Source = source;
                clone.Target = target;

                controller.Game.TaskQueue.Enqueue(clone);
            }

            // only apply enchant and triggers if there is ...
            if (Enchant == null && Trigger == null)
            {
                return;
            }

            switch (Area)
            {
                case EnchantmentArea.TARGET:
                    if (target != null)
                    {
                        Enchant?.Activate(source.Card.Id, target.Enchants, target);
                        Trigger?.Activate(source.Card.Id, target.Triggers, target);
                    }
                    break;
                case EnchantmentArea.BOARD:
                    Enchant?.Activate(source.Card.Id, controller.Board.Enchants, source);
                    Trigger?.Activate(source.Card.Id, controller.Board.Triggers, source);
                    break;
                case EnchantmentArea.OP_BOARD:
                    Enchant?.Activate(source.Card.Id, controller.Opponent.Board.Enchants, source);
                    Trigger?.Activate(source.Card.Id, controller.Opponent.Board.Triggers, source);
                    break;
                case EnchantmentArea.BOARDS:
                    Enchant?.Activate(source.Card.Id, controller.Board.Enchants, source);
                    Trigger?.Activate(source.Card.Id, controller.Board.Triggers, source);
                    Enchant?.Activate(source.Card.Id, controller.Opponent.Board.Enchants, source);
                    Trigger?.Activate(source.Card.Id, controller.Opponent.Board.Triggers, source);
                    break;
                case EnchantmentArea.SECRET:
                    Enchant?.Activate(source.Card.Id, controller.Secrets.Enchants, source);
                    Trigger?.Activate(source.Card.Id, controller.Secrets.Triggers, source);
                    break;
                case EnchantmentArea.HERO:
                    Enchant?.Activate(source.Card.Id, controller.Hero.Enchants, source);
                    Trigger?.Activate(source.Card.Id, controller.Hero.Triggers, source);
                    break;
                case EnchantmentArea.SELF:
                    Enchant?.Activate(source.Card.Id, source.Enchants, source);
                    Trigger?.Activate(source.Card.Id, source.Triggers, source);
                    break;
                case EnchantmentArea.CONTROLLER:
                    Enchant?.Activate(source.Card.Id, controller.Enchants, source);
                    Trigger?.Activate(source.Card.Id, controller.Triggers, source);
                    break;
                case EnchantmentArea.GAME:
                    Enchant?.Activate(source.Card.Id, controller.Game.Enchants, source);
                    Trigger?.Activate(source.Card.Id, controller.Game.Triggers, source);
                    break;

                default:
                    throw new NotImplementedException();
            }

        }
    }
}