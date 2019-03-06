namespace TrainTrain.Domain
{
    public static class CapacityThreshold
    {
        private const double TrainCapacityThreshold = 0.70;

        public static double ForTrain => TrainCapacityThreshold;
    }
}