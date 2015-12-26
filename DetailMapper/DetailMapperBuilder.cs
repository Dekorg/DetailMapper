using DetailMapper.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DetailMapper
{
    /// <summary>
    /// Creates mapping of Details
    /// </summary>
    public static class DetailMapperBuilder
    {
        /// <summary>
        /// Initialize the builder with a MasterDTO and Master entity
        /// </summary>
        /// <typeparam name="TMasterDTO">MasterDTO Type.</typeparam>
        /// <typeparam name="TMaster">Master Type.</typeparam>
        /// <returns></returns>
        public static IDetailMapperBuilder<TMasterDTO, TMaster> Create<TMasterDTO, TMaster>()
        {
            return new DetailMapperBuilder<TMasterDTO, TMaster>();
        }
    }

}
