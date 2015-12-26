using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DetailMapper.Impl
{
    /// <summary>
    /// Context passed in a Detail mapping operation
    /// </summary>
    /// <typeparam name="TMasterDTO">MasterDTO Type.</typeparam>
    /// <typeparam name="TMaster">Master Type.</typeparam>
    /// <typeparam name="TDependencies">Dependency Type.</typeparam>
    public class ViewModelContext<TMasterDTO, TMaster, TDependencies> : IViewModelContext<TMasterDTO, TMaster, TDependencies>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModelContext{TMasterDTO, TMaster, TDependencies}"/> class.
        /// </summary>
        /// <param name="masterDTO">master DTO.</param>
        /// <param name="master">master.</param>
        /// <param name="dependencies">dependencies.</param>
        public ViewModelContext(TMasterDTO masterDTO, TMaster master, TDependencies dependencies)
        {
            MasterDTO = masterDTO;
            Master = master;
            Dependencies = dependencies;
        }

        /// <summary>
        /// Dependent service, repository or composite object
        /// </summary>
        /// <value>
        /// The dependencies.
        /// </value>
        public virtual TDependencies Dependencies
        {
            get;
            protected set;
        }

        /// <summary>
        /// Master DTO
        /// </summary>
        /// <value>
        /// The master dto.
        /// </value>
        public virtual TMasterDTO MasterDTO
        {
            get;
            protected set;
        }

        /// <summary>
        /// Master entity
        /// </summary>
        /// <value>
        /// The master.
        /// </value>
        public virtual TMaster Master
        {
            get;
            protected set;
        }
    }
}
