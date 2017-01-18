using SabberStone.Actions;
using SabberStone.Model;

namespace SabberStone.Tasks.SimpleTasks
{
    public class DrawOpTask : SimpleTask
    {
        public DrawOpTask(Card card = null, bool toStack = false)
        {
            Card = card;
            ToStack = toStack;
        }

        public Card Card { get; set; }

        public bool ToStack { get; set; }

        public override TaskState Process()
        {
            var drawedCard = Card != null ? Generic.DrawCardBlock.Invoke(Controller.Opponent, Card) : Generic.Draw(Controller.Opponent);
            if (ToStack && drawedCard != null)
            {
                Playables.Add(drawedCard);
            }
            return TaskState.COMPLETE;
        }

        public override ISimpleTask Clone()
        {
            var clone = new DrawOpTask(Card, ToStack);
            clone.Copy(this);
            return clone;
        }
    }
}