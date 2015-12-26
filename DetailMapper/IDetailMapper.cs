using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DetailMapper
{
    /// <summary>
    /// Maps a detail of a DTO to an Entity
    /// </summary>
    /// <typeparam name="TMasterDTO">The type of the master dto.</typeparam>
    /// <typeparam name="TMaster">The type of the master.</typeparam>
    /// <typeparam name="TDetailDTO">The type of the detail dto.</typeparam>
    /// <typeparam name="TDetail">The type of the detail.</typeparam>
    /// <typeparam name="TDependencies">The type of the dependencies.</typeparam>
    public interface IDetailMapper<TMasterDTO, TMaster, TDetailDTO, TDetail, TDependencies>
    {
        /// <summary>
        /// Maps a detail of a DTO to an Entity
        /// </summary>
        /// <param name="masterDTO">The master dto.</param>
        /// <param name="master">The master entity.</param>
        /// <param name="dependencies">The main dependency needed in the mapping.</param>
        /// <param name="mapper">Maps properties from <typeparamref name="TDetailDTO"/> to <typeparamref name="TDetail"/>.</param>
        void Map(TMasterDTO masterDTO, TMaster master, TDependencies dependencies, Action<TDetailDTO, TDetail> mapper = null);
    }
}
