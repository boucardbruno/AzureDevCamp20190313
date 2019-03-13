namespace TrainTrain.Domain
{
    public static class CapacityThresholdPolicy
    {
        private const double TrainCapacityThreshold = 0.70;
        private const double CoachCapacityThreshold = 0.70;

        public static double ForTrain => TrainCapacityThreshold;
        public static double ForCoach => CoachCapacityThreshold;
    }
}