namespace TrainTrain.Domain
{
    public static class ThresholdManager
    {
        private const double ThresholdTrainCapacity = 0.70;

        public static double GetMaxRes()
        {
            return ThresholdTrainCapacity;
        }
    }
}