using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Block
{
    public class Block 
    {
        private bool[3,3] block = new bool[3,3];
        public bool[3,3] Block => block;
        public Block()
        {
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    block[i, j] = false;
        }
        public void Create_L_Block()
        {
            block[0, 0] = true;
            block[1, 0] = true;
            block[2, 0] = true;
            block[2, 1] = true;
        }

    }
}
