using System;
using SQLite;

namespace GameOfChallengers.Models
{
    public class LevelingUp
    {

        public int XP { get; set; }// 0-355000
        public int Level { get; set; }// 1-20
        public int Attack { get; set; }// creature's base attack stat
        public int Defense { get; set; }// creature's base defense stat
        public int Speed { get; set; }// creature's base speed stat

        public LevelingUp()
        {
           // Id = null;
            XP = 0;
            Level = 1;
            Attack = 1;
            Defense = 1;
            Speed = 1;
        }
    }
}
