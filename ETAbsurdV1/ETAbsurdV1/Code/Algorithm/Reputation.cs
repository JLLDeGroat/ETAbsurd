using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ETAbsurdV1.Models;

namespace ETAbsurdV1.Code.Algorithm
{
    public class Reputation
    {       

        public static decimal NewReputation(decimal Rep, int? UpVote, int? DownVote, int? TotalVote, string Vote)
        {

            decimal UpVotes = Convert.ToDecimal(UpVote) + 1;
            decimal DownVotes = Convert.ToDecimal(TotalVote) + 1;
            decimal Average = UpVotes / DownVotes / 100;

            //Make Maximum Number
            if (Average >= Convert.ToDecimal(0.10))
            {
                Average = Convert.ToDecimal(0.09);
            }
            if(Vote == "up")
            {
                Average = Rep + Average;
            }
            if(Vote == "down")
            {
                Average = Rep - Average;
            } 
            return Average;
        }

        public static decimal CutReputation(decimal? Reputation)
        {
            int? ReputationInteger = Convert.ToInt32(Reputation);
            //Depending on how many digits, it will cut down to be readable.
            if (ReputationInteger >= 10 | ReputationInteger <= -10)
            {
                Reputation = Convert.ToDecimal(Reputation.ToString().Substring(0, 6));
            }
            else if (ReputationInteger >= 100 | ReputationInteger <= -100)
            {
                Reputation = Convert.ToDecimal(Reputation.ToString().Substring(0, 7));
            }
            else
            {
                Reputation = Convert.ToDecimal(Reputation.ToString().Substring(0, 5));
            }
            return Convert.ToDecimal(Reputation);
        }


        public static int NewKarma(int? PosKarma, int? NegKarma)
        {
            decimal Total = Convert.ToDecimal(PosKarma + NegKarma);

            decimal Average = Convert.ToDecimal(PosKarma / Total) * 100;

            return Convert.ToInt32(Average);
        }

    }
}