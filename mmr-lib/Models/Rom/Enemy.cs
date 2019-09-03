using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MMRando.Models.Rom
{
    public class Enemy
    {
        public int Actor;
        public int Object;
        public int ObjectSize;
        public List<int> Variables = new List<int>();
        public int Type;
        public int Stationary;
        public List<int> SceneExclude = new List<int>();
    }
}
