using GameOfChallengers.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameOfChallengers.Controllers
{
    class TurnController
    {
        //there will be one controller per type and the specific creature will be passed in to the controller methods
        MonsterController mc = new MonsterController() ;
        CharacterController cc = new CharacterController(); 


        public Creature[,] Move(Creature creature, int loc, Creature[,] gameBoard)
        {
            //move the creature to a new spot on the game board
            return gameBoard;
        }



        public int Attack(Creature creature1, Creature creature2)
        {
            
            int score1, score2 = 0;
            //bool succeeded = false;
            int hit = 0;
            int dateSeed = DateTime.Now.Millisecond;
            Random roll = new Random(dateSeed);
            //run the attack of creature1 on creature2, return if it succeeded or not
            //attack is successful(true) if creature1 score > creature2 score
            int rollValue = roll.Next(1, 21);

            if (GameGlobals.DisableRandomNumbers)
            {
                rollValue = 19;
            }
            if (GameGlobals.RollValue >= 0)
            {

                rollValue = GameGlobals.RollValue;
            }
            if (rollValue == 1)
            {
                if (GameGlobals.EnableCriticalMiss == true)
                {
                    return -1;
                }
                return 0;
            }
            if(rollValue == 20)
            {
                if (GameGlobals.EnableCriticalHit == true)
                {
                    return 2;
                }
                return 1;
            }
            
            if (creature1.Type == 0)
            {
                score1 = rollValue + creature1.Level + cc.GetBaseAttack(creature1);
                score2 = mc.GetBaseDefense(creature2) + creature2.Level;
            }
            else
            {
                score1 = rollValue + creature2.Level + mc.GetBaseAttack(creature2);
                score2 = cc.GetBaseDefense(creature1) + creature1.Level;
            }

            if (score1 > score2)
            {
                hit = 1;

            }
            return hit;
        }
            
           
        public int DamageToDo(Creature creature)
        {
            //get the 
            int finalDamage = 0;
            if (creature.Type == 0)
            {
                finalDamage = cc.GetBaseDamage(creature) + (int)Math.Ceiling((double)creature.Level / 4);
            }
            else
            {
                finalDamage = mc.GetBaseDamage(creature) + (int)Math.Ceiling((double)creature.Level / 4);
            }
            return finalDamage;
        }

    }
}
