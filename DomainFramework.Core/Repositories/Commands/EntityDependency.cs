using System;
using System.Collections.Generic;

namespace DomainFramework.Core
{
    public class EntityDependency
    {
        /// <summary>
        /// The entity of the dependency
        /// </summary>
        public IEntity Entity { get; set; }

        /// <summary>
        /// In self reference relationships with multiple association ends, 
        /// a selector is needed to determine which instance of the entity in the dependencies to use
        /// </summary>
        public string Selector { get; set; }

        // Static empty function to get dependencies
        public static Func<IEnumerable<EntityDependency>> EmptyEntityDependencies { get; set; } = () => { return new EntityDependency[] { }; };
    }
}