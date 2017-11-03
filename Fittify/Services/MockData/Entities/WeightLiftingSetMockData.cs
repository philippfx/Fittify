using System;
using System.Collections.Generic;
using Fittify.Entities;

namespace Fittify.Services.MockData.Entities
{
    public class WeightLiftingSetMockData : IData<WeightLiftingSet>
    {
        private List<WeightLiftingSet> _sets;

        public WeightLiftingSetMockData()
        {
            _sets = new List<WeightLiftingSet>();
            _sets.Add(new WeightLiftingSet()
            {
                Id = 1,
                DateTimeStart = DateTime.Now.AddHours(-5),
                DateTimeEnd = DateTime.Now.AddHours(-4),
                WeightFull = 40,
                RepetitionsFull = 12,
                WeightReduced = 30,
                RepetitionsReduced = 4,
                WeightBurn = 20,
                MachineAdjustableType1 = MachineAdjustableType.SeatPosition,
                MachineAdjustableSetting1 = 3
            });
            _sets.Add(new WeightLiftingSet()
            {
                Id = 2,
                DateTimeStart = DateTime.Now.AddHours(-5),
                DateTimeEnd = DateTime.Now.AddHours(-4),
                WeightFull = 40,
                RepetitionsFull = 10,
                WeightReduced = 30,
                RepetitionsReduced = 3,
                WeightBurn = 20,
                MachineAdjustableType1 = MachineAdjustableType.SeatPosition,
                MachineAdjustableSetting1 = 3
            });
            _sets.Add(new WeightLiftingSet()
            {
                Id = 3,
                DateTimeStart = DateTime.Now.AddHours(-5),
                DateTimeEnd = DateTime.Now.AddHours(-4),
                WeightFull = 40,
                RepetitionsFull = 8,
                WeightReduced = 30,
                RepetitionsReduced = 2,
                WeightBurn = 20,
                MachineAdjustableType1 = MachineAdjustableType.SeatPosition,
                MachineAdjustableSetting1 = 3
            });
        }
        public ICollection<WeightLiftingSet> GetAll()
        {
            return _sets;
        }

        public WeightLiftingSet Get(int id)
        {
            throw new NotImplementedException();
        }
    }
}
