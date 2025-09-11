using System.Threading.Tasks;

namespace TrainTrain.Domain.Port
{
    public interface IProvideTrainTopology
    {
        Task<Train> GetTrainBy(TrainId trainId);
    }
}