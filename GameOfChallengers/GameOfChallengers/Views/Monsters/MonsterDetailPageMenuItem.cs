﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfChallengers.Views.Monsters
{

    public class MonsterDetailPageMenuItem
    {
        public MonsterDetailPageMenuItem()
        {
            TargetType = typeof(MonsterDetailPageDetail);
        }
        public int Id { get; set; }
        public string Title { get; set; }

        public Type TargetType { get; set; }
    }
}