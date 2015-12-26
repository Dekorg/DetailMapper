using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DetailMapper.Tests.Utils
{
    public interface ICollectionBuilder<TObject>
    {
        IEnumerable<TObject> BuildCollection(int length);
        IEnumerable<TObject> BuildCollection(int length, Action<TObject, int> setProperties);
    }

    public interface IBuilder<TObject>
    {
        TObject Build();
    }

    public interface IBuilderWithSetters<TBuilder, TObject>
    : IBuilder<TObject>
    where TBuilder : IBuilderWithSetters<TBuilder, TObject>
    {
        TBuilder With(Action<TObject> setProperties);
    }

    public abstract class AbstractTemplateBuilder<TBuilder, TObject>
            : IBuilderWithSetters<TBuilder, TObject>,
                ICollectionBuilder<TObject>
        where TBuilder : AbstractTemplateBuilder<TBuilder, TObject>
        where TObject : class, new()
    {
        protected readonly IList<Action<TObject>> actions;
        protected AbstractTemplateBuilder()
        {
            actions = new List<Action<TObject>>();
        }

        public virtual TBuilder With(Action<TObject> setProperties)
        {
            if (setProperties == null)
                throw new ArgumentNullException("setProperties");
            actions.Add(setProperties);
            return (TBuilder)this;
        }

        public virtual TObject Build()
        {
            TObject toBuild = new TObject();
            foreach (Action<TObject> action in actions)
            {
                if (action != null)
                    action(toBuild);
            }
            return toBuild;
        }

        public virtual IEnumerable<TObject> BuildCollection(int length)
        {
            TObject toBuild = Build();
            for (int i = 0; i < length; i++)
            {
                yield return toBuild;
            }
        }

        public virtual IEnumerable<TObject> BuildCollection(int length, Action<TObject, int> setProperties)
        {
            for (int i = 0; i < length; i++)
            {
                TObject toBuild = Build();
                if (setProperties != null)
                    setProperties(toBuild, i);
                yield return toBuild;
            }
        }
    }
}
