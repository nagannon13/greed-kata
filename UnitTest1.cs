using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace greed_kata
{

    public class GreedScore
    {
        private Dictionary<int,int> _dieCounts = new Dictionary<int, int>();

        public int Calculate(params int[] dieValues)
        {
            PopulateCount(dieValues);


            int score = 0;
            
            score += ScoreTripleOnes();
            score += ScoreTripleOthers();
            score += ScoreSingleDie(1, 100);
            score += ScoreSingleDie(5, 50);

            return score;
        }

        private void PopulateCount(int[] dieValues)
        {
            for(int i =1; i<=6;i++)
            {
                _dieCounts.Add(i, dieValues.Count(d => d == i));
            }
        }

        private int ScoreTripleOnes()
        {
            if(_dieCounts[1] >= 3) 
            {
                _dieCounts[1] -= 3;
                return 1000;
            }
            return 0;
        }
        private int ScoreTripleOthers()
        {
            for(int i=2;i<=6;i++)
            {
                if(_dieCounts[i] >= 3)
                {
                    _dieCounts[i] -= 3;
                    return i * 100;
                }
            }

            return 0;
        }
        private int ScoreSingleDie(int dieValue, int dieScore)
        {
            if(dieValue == 1) return _dieCounts[1] * dieScore;
            if(dieValue == 5) return _dieCounts[5] * dieScore;
            return 0;
        }
    }

    public class GreedScore_Calculate
    {
        [Fact]
        public void Returns0GivenNoDice()
        {
            var scorer = new GreedScore();

            var result = scorer.Calculate(new List<int>().ToArray());

            Assert.Equal(0, result);
        }
        [Theory]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(6)]
        public void Returns0GivenAWorthlessDie(int dieValue)
        {
            var scorer = new GreedScore();

            var result = scorer.Calculate((new List<int>() { dieValue }).ToArray());

            Assert.Equal(0, result);
        }

        [Theory]
        [InlineData(1, 100)]
        [InlineData(5, 50)]
        public void Returns100GivenASingleOne(int dieValue, int expectedScore)
        {
            var scorer = new GreedScore();

            var result = scorer.Calculate((new List<int>() { dieValue }).ToArray());

            Assert.Equal(expectedScore, result);
        }

        [Theory]
        [InlineData(1, 200)]
        [InlineData(5, 100)]
        public void ReturnsExpectedValueGivenTwoOnesOrFives(int dieValue, int expectedScore)
        {
            var scorer = new GreedScore();

            var result = scorer.Calculate((new List<int>() { dieValue, dieValue }).ToArray());

            Assert.Equal(expectedScore, result);
        }

        [Theory]
        [InlineData(1, 5)]
        [InlineData(5, 1)]
        [InlineData(5, 1, 2, 3)]
        [InlineData(5, 1, 4, 6)]
        public void Returns150GivenOneAndFiveAndWhateverElse(params int[] dieValues)
        {
            var scorer = new GreedScore();

            var result = scorer.Calculate(dieValues);

            Assert.Equal(150, result);
        }

        [Theory]
        [InlineData(1, 1, 1)]
        public void Returns1000GivenThreeOnes(params int[] dieValues)
        {
            var scorer = new GreedScore();

            var result = scorer.Calculate(dieValues);

            Assert.Equal(1000, result);
        }
        [Theory]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        [InlineData(6)]
        public void ReturnsExpectedAmountGivenThreeNonOnes(params int[] dieValues)
        {
            var scorer = new GreedScore();

            var result = scorer.Calculate(dieValues[0], dieValues[0], dieValues[0]);

            Assert.Equal(100*dieValues[0], result);
        }

    }
}
