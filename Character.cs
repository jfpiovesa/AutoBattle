using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using static AutoBattle.Types;

namespace AutoBattle
{
    public class Character
    {
        public string Name { get; set; }
        public float Health { get; set; }
        public float BaseDamage { get; set; }
        public float DamageMultiplier { get; set; }

        public bool CanWalk = true;
        public bool isDead = false;

        public CharacterClassSpecific CharacterClassSpecific;
        public GridBox currentBox;
        public int PlayerIndex;
        public Character Target { get; set; }
        public Character(CharacterClass characterClass)
        {
            switch (characterClass)
            {

                case CharacterClass.Paladin:
                    CharacterClassSpecific.characterClass = CharacterClass.Paladin;
                    CharacterClassSpecific.hpModifier = 150;
                    CharacterClassSpecific.ClassDamage = 20;

                    break;
                case CharacterClass.Archer:
                    CharacterClassSpecific.characterClass = CharacterClass.Archer;
                    CharacterClassSpecific.hpModifier = 100;
                    CharacterClassSpecific.ClassDamage = 25;
                    CharacterClassSpecific.skills = null;

                    break;
                case CharacterClass.Cleric:
                    CharacterClassSpecific.characterClass = CharacterClass.Cleric;
                    CharacterClassSpecific.hpModifier = 130;
                    CharacterClassSpecific.ClassDamage = 15;
                    CharacterClassSpecific.skills = null;

                    break;
                case CharacterClass.Warrior:
                    CharacterClassSpecific.characterClass = CharacterClass.Warrior;
                    CharacterClassSpecific.hpModifier = 120;
                    CharacterClassSpecific.ClassDamage = 17;
                    CharacterClassSpecific.skills = null;
                    break;
            }

            Health = CharacterClassSpecific.hpModifier;
            BaseDamage = CharacterClassSpecific.ClassDamage;

        }
        public bool TakeDamage(float amount)
        {
            Health -= amount;
            if (Health <= 0)
            {
                return true;
            }
            return false;
        }

        public void Die(Character target)
        {
            //TODO >> maybe kill him?
            Console.WriteLine($"Player {target.Name} is died \n");
        }

        public void WalkTO(Grid battlefield)
        {
            // if there is no target close enough, calculates in wich direction this character should move to be closer to a possible target
            if (this.currentBox.xIndex > Target.currentBox.xIndex)
            {
                if ((battlefield.grids.Exists(x => x.Index == currentBox.Index - 1)))
                {
                    currentBox.ocupied = false;
                    battlefield.grids[currentBox.Index] = currentBox;
                    currentBox = (battlefield.grids.Find(x => x.Index == currentBox.Index - 1));
                    currentBox.ocupied = true;
                    battlefield.grids[currentBox.Index] = currentBox;
                    Console.WriteLine($"Player {Name} walked left\n");
                    battlefield.drawBattlefield();

                    return;
                }
            }
            else if (currentBox.xIndex < Target.currentBox.xIndex)
            {
                currentBox.ocupied = false;
                battlefield.grids[currentBox.Index] = currentBox;
                currentBox = (battlefield.grids.Find(x => x.Index == currentBox.Index + 1));
                currentBox.ocupied = true;

                battlefield.grids[currentBox.Index] = currentBox;
                Console.WriteLine($"Player {Name} walked right\n");
                battlefield.drawBattlefield();
                return;
            }

            if (this.currentBox.yIndex > Target.currentBox.yIndex)
            {
                if ((battlefield.grids.Exists(x => x.Index == currentBox.Index - 1)))
                {
                    this.currentBox.ocupied = false;
                    battlefield.grids[currentBox.Index] = currentBox;
                    this.currentBox = (battlefield.grids.Find(x => x.Index == currentBox.Index - battlefield.xLenght));
                    this.currentBox.ocupied = true;
                    battlefield.grids[currentBox.Index] = currentBox;
                    Console.WriteLine($"Player {Name} walked up\n");
                    battlefield.drawBattlefield();
                    return;
                }
            }
            else if (this.currentBox.yIndex < Target.currentBox.yIndex)
            {
                this.currentBox.ocupied = false;
                battlefield.grids[currentBox.Index] = this.currentBox;
                this.currentBox = (battlefield.grids.Find(x => x.Index == currentBox.Index + battlefield.xLenght));
                this.currentBox.ocupied = true;
                battlefield.grids[currentBox.Index] = currentBox;
                Console.WriteLine($"Player {Name} walked down\n");
                battlefield.drawBattlefield();

                return;
            }
        }

        public void StartTurn(Grid battlefield)
        {
            if (isDead | Target.isDead) return;

            if (CheckCloseTargets(battlefield))
            {
                Attack(Target);


                return;
            }
            else
            {
                if (CanWalk)
                {
                    WalkTO(battlefield);
                }

            }
        }

        // Check in x and y directions if there is any character close enough to be a target.
        bool CheckCloseTargets(Grid battlefield)
        {
            bool left = (battlefield.grids.Find(x => x.Index == currentBox.Index - 1).ocupied);
            bool right = (battlefield.grids.Find(x => x.Index == currentBox.Index + 1).ocupied);
            bool up = (battlefield.grids.Find(x => x.Index == currentBox.Index + battlefield.xLenght).ocupied);
            bool down = (battlefield.grids.Find(x => x.Index == currentBox.Index - battlefield.xLenght).ocupied);

            if (left || right || up || down)
            {
                return true;
            }
            return false;
        }

        public void Attack(Character target)
        {
            var rand = new Random();
            int takeDamage = rand.Next(0, (int)BaseDamage);
            isDead = target.TakeDamage(takeDamage);
            if (!isDead)
            {
                Console.WriteLine($"Player {Name} is attacking the player {target.Name} and did {takeDamage} damage and has {target.Health} life left \n");
            }
            else
            {
                Die(target);
            }
        }
    }
}
