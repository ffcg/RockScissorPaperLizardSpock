using System;

namespace FFCG.RSPLS.Domain
{
    public class GameException : Exception
    {
        public GameException(string message)
            : base(message)
        {
        }
    }
}