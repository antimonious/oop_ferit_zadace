using System;

namespace Weather_library
{
    public interface IRandomGenerator
    {
        double GenerateDouble(double minValue, double maxValue);
    }
}
