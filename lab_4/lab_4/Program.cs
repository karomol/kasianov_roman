using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab4test
{
    public abstract class Forest
    {
        public abstract void Plant();
        public abstract void Uproot();
    }
    public class Tree : Forest
    {
        public string variety { get; set; }
        private List<Tree> Trees = new List<Tree>();
        public Tree(string variety)
        {
            this.variety = variety;
        }
        public void AddToList(Tree tree)
        {
            Trees.Add(tree);
            Plant();
        }
        public void DeleteFromList(Tree tree)
        {
            Trees.Remove(tree);
            Uproot();
        }
        public override void Plant()
        {
            Console.WriteLine($"A new tree was planted");
        }
        public override void Uproot()
        {
            Console.WriteLine($"One tree was uprooted");
        }
        public List<Tree> GetList()
        {
            Console.WriteLine($"Here are the list of {variety} trees:");
            return Trees;
        }
    }
    public class FurTree : Tree
    {
        public string name { get; set; }
        public FurTree(string name, string variety) : base(variety)
        {
            this.name = name;
        }
    }
    public class Bush : Forest
    {
        public string variety { get; set; }
        private List<Bush> Bushes = new List<Bush>();
        public Bush(string variety)
        {
            this.variety = variety;
        }
        public void AddToList(Bush bush)
        {
            Bushes.Add(bush);
            Plant();
        }
        public void DeleteFromList(Bush bush)
        {
            Bushes.Remove(bush);
            Uproot();
        }
        public override void Plant()
        {
            Console.WriteLine($"New bush was planted");
        }
        public override void Uproot()
        {
            Console.WriteLine($"One bush was uprooted");
        }
        public List<Bush> GetList()
        {
            return Bushes;
        }
    }
    public class Berry
    {
        public void PickBerries(List<Bush> bushes)
        {
            int count = 0;
            foreach (RoseBush b in bushes)
            {
                count += b.berry_number;
                b.berry_number = 0;
            }
            Console.WriteLine($"{count} berries were picked");
        }
        public void BerriesCount(List<Bush> bushes)
        {
            int count = 0;
            foreach (RoseBush b in bushes)
            {
                count += b.berry_number;
            }
            Console.WriteLine($"current count of berries - {count}");
        }

    }
    public class RoseBush : Bush
    {
        public string name { get; set; }
        public int berry_number;
        public int Berry_number
        {
            set
            {
                if (value > -1)
                    berry_number = value;
                else
                    berry_number = 0;
            }
            get
            {
                return berry_number;
            }
        }

        public RoseBush(string name, string variety, int Berry_number) : base(variety)
        {
            this.name = name;
            this.Berry_number = Berry_number;
        }
        public int GetNumberOfBerries()
        {
            return berry_number;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Tree coniferous = new Tree("coniferous trees");
            Tree furtree1 = new FurTree("furtree1", "coniferous");
            Tree furtree2 = new FurTree("furtree2", "coniferous");

            coniferous.AddToList(furtree1);
            coniferous.AddToList(furtree2);

            foreach (FurTree furtree in coniferous.GetList())
                Console.WriteLine(furtree.name);
            Console.WriteLine();

            coniferous.DeleteFromList(furtree2);

            foreach (FurTree furtree in coniferous.GetList())
                Console.WriteLine(furtree.name);
            Console.WriteLine();

            Bush berry_bearing = new Bush("berry-bearing");
            Bush rose_bush1 = new RoseBush("rose-bush1", "berry-bearing", 3);
            Bush rose_bush2 = new RoseBush("rose-bush2", "berry-bearing", 5);
            Bush rose_bush3 = new RoseBush("rose-bush3", "berry-bearing", -3);

            berry_bearing.AddToList(rose_bush1);
            berry_bearing.AddToList(rose_bush2);
            berry_bearing.AddToList(rose_bush3);

            Berry b = new Berry();
            foreach (RoseBush rosebush in berry_bearing.GetList())
                Console.WriteLine($"{rosebush.name} - {rosebush.berry_number} berries");

            b.BerriesCount(berry_bearing.GetList());

            berry_bearing.DeleteFromList(rose_bush2);
            Console.WriteLine();

            foreach (RoseBush rosebush in berry_bearing.GetList())
                Console.WriteLine($"{rosebush.name} - {rosebush.berry_number} berries");

            b.PickBerries(berry_bearing.GetList());

            foreach (RoseBush rosebush in berry_bearing.GetList())
                Console.WriteLine($"{rosebush.name} - {rosebush.berry_number} berries");
            Console.ReadKey();
        }
    }
}