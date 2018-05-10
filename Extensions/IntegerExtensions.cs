using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static OWLMiddleware.Models.Enumerations;

namespace OWLMiddleware.Extensions
{
    public static class IntegerExtensions
    {
        private static int[] AllTeamIds = { 4523, 4524, 4525, 4402, 4403, 4404, 4405, 4406, 4407, 4408, 4409, 4410 };
        public static bool IsTeamId(this int check)
        {
            return AllTeamIds.Contains(check);
        }

    }
}
