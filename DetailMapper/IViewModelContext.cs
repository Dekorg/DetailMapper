using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DetailMapper
{
    /// <summary>
    /// Context passed in a Detail mapping operation
    /// </summary>
    /// <typeparam name="TMasterDTO">MasterDTO Type.</typeparam>
    /// <typeparam name="TMaster">Master Type.</typeparam>
    /// <typeparam name="TDependencies">Dependency Type.</typeparam>
    public interface IViewModelContext<TMasterDTO, TMaster, TDependency>
    {
        /// <summary>
        /// Master DTO
        /// </summary>
        /// <value>
        /// The master dto.
        /// </value>
        TMasterDTO MasterDTO { get; }

        /// <summary>
        /// Master entity
        /// </summary>
        /// <value>
        /// The master.
        /// </value>
        TMaster Master { get; }

        /// <summary>
        /// Dependent service, repository or composite object
        /// </summary>
        /// <returns></returns>
        TDependency Dependencies { get; }
    }
}
