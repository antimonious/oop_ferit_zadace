using System;

namespace WeatherLibrary
{
    public interface IRandomGenerator
    {
        double GenerateDouble(double minValue, double maxValue);
    }
}
