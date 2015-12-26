using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DetailMapper.Impl
{
    /// <summary>
    /// Base properties of a Detail Builder
    /// </summary>
    /// <typeparam name="TMasterDTO">MasterDTO Type.</typeparam>
    /// <typeparam name="TMaster">Master Type.</typeparam>
    /// <typeparam name="TDetailDTO">DetailDTO Type.</typeparam>
    /// <typeparam name="TDetail">Detail Type.</typeparam>
    /// <typeparam name="TDependencies">Dependency Type.</typeparam>
    public interface IDetailBuilderProperties<TMasterDTO, TMaster, TDetailDTO, TDetail, TDependencies>
    {
        /// <summary>
        /// Gets a value indicating whether Map requires a dependency to be not null.
        /// </summary>
        /// <value>
        ///   <c>true</c> if requires dependency; otherwise, <c>false</c>.
        /// </value>
        bool RequiresDependency { get; }

        Func<TMasterDTO, ICollection<TDetailDTO>> DetailDTOCollection { get; }

        Func<TMaster, ICollection<TDetail>> DetailCollection { get; }

        Action<IViewModelContext<TMasterDTO, TMaster, TDependencies>, TDetail> Add { get; }

        Action<IViewModelContext<TMasterDTO, TMaster, TDependencies>, TDetail> Update { get; }

        Action<IViewModelContext<TMasterDTO, TMaster, TDependencies>, TDetail> Delete { get; }

        Func<IViewModelContext<TMasterDTO, TMaster, TDependencies>, TDetail> Create { get; }

        Func<TDetailDTO, TDetail, bool> AreEquals { get; }
    }
}
