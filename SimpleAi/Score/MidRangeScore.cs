namespace SimpleAi.Score
{
    public class MidRangeScore : Score
    {
        public override int Rate()
        {
            if (OpHeroHp < 1)
                return int.MaxValue;

            if (HeroHp < 1)
                return int.MinValue;

            var result = 0;

            if (OpBoard.Count == 0 && Board.Count > 0)
                result += 5000;

            result += (Board.Count - OpBoard.Count) * 5;

            if (OpMinionTotHealthTaunt > 0)
                result += OpMinionTotHealthTaunt * -1000;

            result += MinionTotAtk;

            result += (HeroHp - OpHeroHp) * 10;

            result += (MinionTotHealth - OpMinionTotHealth) * 10;

            result += (MinionTotAtk - OpMinionTotAtk) * 10;

            return result;
        }
    }
}