using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test
{
    public class Vertex
    {
        public double x;
        public double y;
        public double z;
        public int id;
        
        public Vertex(int id,double x,double y,double z)
        {
            this.id = id;
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public bool judgeEqual(Vertex another)
        {
            if(this.x==another.x && this.y==another.y && this.z==another.z)
            {
                return true;
            }
            else
            {
                return false;
            }
                
        }

    }
}
