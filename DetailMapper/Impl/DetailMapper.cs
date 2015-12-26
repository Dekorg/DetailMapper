using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DetailMapper.Impl
{
    public class DetailMapper<TMasterDTO, TMaster, TDetailDTO, TDetail, TDependencies> : IDetailMapper<TMasterDTO, TMaster, TDetailDTO, TDetail, TDependencies>
    {
        private readonly IDetailBuilderProperties<TMasterDTO, TMaster, TDetailDTO, TDetail, TDependencies> _baseBuilder;
        public DetailMapper(IDetailBuilderProperties<TMasterDTO, TMaster, TDetailDTO, TDetail, TDependencies> baseBuilder)
        {
            if (baseBuilder == null)
                throw new ArgumentNullException("baseBuilder");
            _baseBuilder = baseBuilder;
        }

        public virtual void Map(TMasterDTO masterDTO, TMaster master, TDependencies dependencies, Action<TDetailDTO, TDetail> mapper = null)
        {
            if (masterDTO == null)
                throw new ArgumentNullException("masterDTO");
            if (master == null)
                throw new ArgumentNullException("master");
            if(_baseBuilder.RequiresDependency && dependencies == null)
                throw new ArgumentNullException("dependencies");

            var detailCollection = _baseBuilder.DetailCollection(master);
            var detailDTOCollection = _baseBuilder.DetailDTOCollection(masterDTO);

            if (detailCollection == null)
                throw new Exception("DetailCollection can not be null");
            if (detailDTOCollection == null)
                throw new Exception("detailDTOCollection can not be null");

            var deleteAction = _baseBuilder.Delete;
            var addAction = _baseBuilder.Add;
            var updateAction = _baseBuilder.Update;
            var create = _baseBuilder.Create;
            var equals = _baseBuilder.AreEquals;

            var viewModelContext = new ViewModelContext<TMasterDTO, TMaster, TDependencies>(masterDTO, master, dependencies);

            // Borra los details que ya no existen
            detailCollection.Where(detail => !detailDTOCollection.Any(detailDTO => equals(detailDTO, detail)))
                .ToList().ForEach(deleted => deleteAction(viewModelContext, deleted));

            // Actualiza o inserta los demás details
            foreach (var detailViewModel in detailDTOCollection)
            {
                // Buscar que exista el detail
                var detail = detailCollection.FirstOrDefault(auxDetail => equals(detailViewModel, auxDetail));
                if (detail == null)
                {
                    detail = create(viewModelContext);
                    // Agregarlo a la lista

                    addAction(viewModelContext, detail);
                }
                else
                {
                    // Mapeo extra
                    if (updateAction != null)
                        updateAction(viewModelContext, detail);
                }

                // Mapeo de datos
                if (mapper != null)
                    mapper(detailViewModel, detail);
            }
        }
    }
}
