// INNOVT TECNOLOGIA 2014-2021
// Author: Michel Magalhães
// Project: Innovt.Core
// Solution: Innovt.Platform
// Date: 2021-06-02
// Contact: michel@innovt.com.br or michelmob@gmail.com

using Innovt.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Innovt.Core.Serialization
{
    public class DeserializerFactory
    {
        private static DeserializerFactory instance;

        private readonly Dictionary<string, Type> mapping;

        private DeserializerFactory()
        {
            mapping = new Dictionary<string, Type>();
        }

        public static DeserializerFactory Instance => instance ??= new DeserializerFactory();

        private string GetGenericFullName<T>()
        {
            return typeof(T).FullName;
        }

        public DeserializerFactory AddMapping<T>(string key = null)
        {
            var typeKey = key ?? GetGenericFullName<T>();

            mapping.TryAdd(typeKey, typeof(T));
            return instance;
        }


        public object Deserialize([NotNull] string key, string content)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));

            if (content.IsNullOrEmpty())
                return default;

            if (!mapping.TryGetValue(key, out var typeValue))
                return default;

            var deserialized = System.Text.Json.JsonSerializer.Deserialize(content, typeValue);

            return deserialized;
        }


        public T Deserialize<T>([NotNull] string key, string content)
        {
            return (T)Deserialize(key, content);
        }
    }
}