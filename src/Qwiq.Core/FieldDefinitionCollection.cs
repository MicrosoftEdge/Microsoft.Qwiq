﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Qwiq
{
    /// <summary>
    ///     A facade over <see cref="Microsoft.TeamFoundation.WorkItemTracking.Client.FieldDefinitionCollection" />.
    /// </summary>
    public abstract class FieldDefinitionCollection : IFieldDefinitionCollection
    {
        private readonly Dictionary<int, int> _fieldUsagesById;

        private readonly Dictionary<string, int> _fieldUsagesByName;

        private readonly List<IFieldDefinition> _list;

        protected FieldDefinitionCollection()
        {
        }

        protected FieldDefinitionCollection(IEnumerable<IFieldDefinition> fieldDefinitions)
        {
            _list = new List<IFieldDefinition>();
            _fieldUsagesByName = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            _fieldUsagesById = new Dictionary<int, int>();

            foreach (var field in fieldDefinitions) Add(field);
        }

        public virtual int Count => _list.Count;

        public virtual IFieldDefinition this[string name]
        {
            get
            {
                if (string.IsNullOrEmpty(name))
                    throw new ArgumentException("Value cannot be null or empty.", nameof(name));

                if (_fieldUsagesByName.TryGetValue(name, out int index)) return _list[index];

                throw new FieldDefinitionNotExistException(name);
            }
        }

        public virtual bool Contains(string fieldName)
        {
            if (string.IsNullOrEmpty(fieldName))
                throw new ArgumentException("Value cannot be null or empty.", nameof(fieldName));
            return _fieldUsagesByName.ContainsKey(fieldName);
        }

        public virtual IEnumerator<IFieldDefinition> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public virtual IFieldDefinition TryGetById(int id)
        {
            int index;
            if (_fieldUsagesById.TryGetValue(id, out index)) return _list[index];
            return null;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj)) return true;
            if (ReferenceEquals(obj, null)) return false;
            var fdc = obj as IFieldDefinitionCollection;
            if (fdc == null) return false;

            return this.All(p => fdc.Contains(p, FieldDefinitionComparer.Instance));
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return this.Aggregate(27, (current, f) => (13 * current) ^ f.GetHashCode());
            }
        }

        private void Add(IFieldDefinition p)
        {
            var count = _list.Count;
            try
            {
                _fieldUsagesByName.Add(p.Name, count);
            }
            catch (ArgumentException e)
            {
                throw new ArgumentException($"A field with the name {p.Name} already exists.", e);
            }
            try
            {
                _fieldUsagesByName.Add(p.ReferenceName, count);
            }
            catch (ArgumentException e)
            {
                throw new ArgumentException($"A field with the name {p.ReferenceName} already exists.", e);
            }
            try
            {
                _fieldUsagesById.Add(p.Id, count);
            }
            catch (ArgumentException e)
            {
                throw new ArgumentException($"A field with the ID {p.Id} already exists.", e);
            }
            _list.Add(p);
        }
    }
}