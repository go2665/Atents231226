using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _01_Console
{
    internal class Enemy : Character
    {
        public Enemy(string _name) : base(_name)
        { 
        }

        protected override float OnSkill()
        {
            Console.WriteLine("적은 도끼를 던진다.");
            return attackPower * 3.0f;
        }
    }
}
