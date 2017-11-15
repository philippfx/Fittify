using System;
using System.Collections.Generic;
using System.Text;
using DataModels.Models.Sport;

namespace DataModelRepositories.Repository.Sport
{
    public class CardioSetRepository : Crud<CardioSet, int>
    {
        public CardioSetRepository()
        {
            
        }

        public CardioSetRepository(FittifyContext fittifyContext) : base(fittifyContext)
        {
            
        }
    }
}
