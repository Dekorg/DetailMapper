using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DetailMapper
{
    /// <summary>
    /// Builds a DetailMapper
    /// </summary>
    /// <typeparam name="TMasterDTO">MasterDTO Type.</typeparam>
    /// <typeparam name="TMaster">Master Type.</typeparam>
    public interface IDetailMapperBuilder<TMasterDTO, TMaster>
    {
        /// <summary>
        /// Initialize the building of a Detail Mapping
        /// </summary>
        /// <typeparam name="TDetailDTO">DetailDTO Type.</typeparam>
        /// <typeparam name="TDetail">Detail Type.</typepara
        /// <param name="detailDTOCollection">DetailDTO collection.</param>
        /// <param name="detailCollection">Detail collection.</param>
        /// <returns></returns>
        IDetailMapperBuilder<TMasterDTO, TMaster, TDetailDTO, TDetail> Detail<TDetailDTO, TDetail>(
           Func<TMasterDTO, ICollection<TDetailDTO>> detailDTOCollection,
           Func<TMaster, ICollection<TDetail>> detailCollection);
    }

    /// <summary>
    /// Builds a DetailMapper
    /// </summary>
    /// <typeparam name="TMasterDTO">MasterDTO Type.</typeparam>
    /// <typeparam name="TMaster">Master Type.</typeparam>
    /// <typeparam name="TDetailDTO">DetailDTO Type.</typeparam>
    /// <typeparam name="TDetail">Detail Type.</typeparam>
    public interface IDetailMapperBuilder<TMasterDTO, TMaster, TDetailDTO, TDetail>
    {
        /// <summary>
        /// Set the Main dependency
        /// </summary>
        /// <typeparam name="TDependencies">Dependency Type.</typeparam>
        /// <returns></returns>
        IDetailMapperBuilder<TMasterDTO, TMaster, TDetailDTO, TDetail, TDependencies> WithDependencies<TDependencies>();
    }

    /// <summary>
    /// Builds a DetailMapper
    /// </summary>
    /// <typeparam name="TMasterDTO">MasterDTO Type.</typeparam>
    /// <typeparam name="TMaster">Master Type.</typeparam>
    /// <typeparam name="TDetailDTO">DetailDTO Type.</typeparam>
    /// <typeparam name="TDetail">Detail Type.</typeparam>
    /// <typeparam name="TDependencies">Dependency Type.</typeparam>
    public interface IDetailMapperBuilder<TMasterDTO, TMaster, TDetailDTO, TDetail, TDependencies>
    {
        IDetailMapperBuilder<TMasterDTO, TMaster, TDetailDTO, TDetail, TDependencies> AddAction(Action<IViewModelContext<TMasterDTO, TMaster, TDependencies>, TDetail> addDetail);
        IDetailMapperBuilder<TMasterDTO, TMaster, TDetailDTO, TDetail, TDependencies> UpdateAction(Action<IViewModelContext<TMasterDTO, TMaster, TDependencies>, TDetail> updateDetail);
        IDetailMapperBuilder<TMasterDTO, TMaster, TDetailDTO, TDetail, TDependencies> DeleteAction(Action<IViewModelContext<TMasterDTO, TMaster, TDependencies>, TDetail> deleteDetail);
        IDetailMapperBuilder<TMasterDTO, TMaster, TDetailDTO, TDetail, TDependencies> CreateFunc(Func<IViewModelContext<TMasterDTO, TMaster, TDependencies>, TDetail> createFunc);
        IDetailMapperBuilder<TMasterDTO, TMaster, TDetailDTO, TDetail, TDependencies> EqualsFunc(Func<TDetailDTO, TDetail, bool> equalsFunc);
        IDetailMapper<TMasterDTO, TMaster, TDetailDTO, TDetail, TDependencies> Build();
    }
}
