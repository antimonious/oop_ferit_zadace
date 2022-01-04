﻿using System;

namespace Weather_library
{
    public class UniformGenerator : IRandomGenerator
    {
        private Random generator;
        public UniformGenerator(Random generator)
        {
            this.generator = generator;
        }
        public double GenerateDouble(double minValue, double maxValue)
        {
            return generator.NextDouble() * (maxValue - minValue) + minValue;
        }
    }
}
