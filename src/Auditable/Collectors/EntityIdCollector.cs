﻿namespace Auditable.Collectors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public class EntityIdCollector : IEntityIdCollector
    {
        public string Extract(object instance)
        {
            var type = instance.GetType();

            var fields = AllFields(AllTypes(type));


            var idFieldPatterns = new[]
            {
                "_id",
                GetPropertyBackingFieldName("Id"),
                GetPropertyBackingFieldName($"{type.Name}Id"),
                "id",
                "Id"
            }.ToList();

            var idField = fields.FirstOrDefault(x => idFieldPatterns.Contains(x.Name));

            if (idField == null)
            {
                throw new NoIdAttributeException(instance.GetType());
            }

            var id = idField.GetValue(instance)?.ToString();
            //if (string.IsNullOrEmpty(id))
            //{
            //    throw new NoIdValueException(instance.GetType(), idField.Name);
            //}

            return id;

        }

        private IEnumerable<FieldInfo> AllFields(IEnumerable<Type> typeTree)
        {
            return typeTree.SelectMany(x => x.GetTypeInfo().DeclaredFields);
        }

        private IEnumerable<Type> AllTypes(Type currentType)
        {
            if (currentType.BaseType != null)
            {
                foreach (Type type in AllTypes(currentType.BaseType))
                {
                    yield return type;
                }
            }

            yield return currentType;
        }
        
        private string GetPropertyBackingFieldName(string propertyName)
        {
            return $"<{propertyName}>k__BackingField";
        }

    }
}