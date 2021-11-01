﻿using System.Collections.Generic;

namespace MMR.Enemizer.Models
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