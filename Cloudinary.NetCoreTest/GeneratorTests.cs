using NUnit.Framework;
using FizzWare.NBuilder;
using CloudinaryDotNet.Actions;
using System.Collections;
using System;
using System.Reflection;
using Newtonsoft.Json;
using System.Linq;
using AutoFixture;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace CloudinaryDotNet.Test
{
    public class GeneratorTests
    {
        [TestCaseSource(typeof(TypeGenerator), nameof(TypeGenerator.Types))]
        public void Type_should_deserialize_all_non_public_props(Type type)
        {
            var r = GenerateRandomObject(type);
            var json1 = SerializeToJson(r);
            AssertAllPropertiesSerialized(json1);

            var o = DeserializeFromJson(type, json1);

            var json2 = SerializeToJson(o);
            Assert.That(json2, Is.EqualTo(json1));
        }

        private static void AssertAllPropertiesSerialized(string json) => Assert.That(json, Is.Not.Contain("null"));

        private static object DeserializeFromJson(Type type, string json)
        {
            var jobject = JObject.Parse(json);

            var o = Activator.CreateInstance(type);
            if (type == typeof(ListResourceTypesResult))
            {
                type
                    .GetField("m_resourceTypes", BindingFlags.NonPublic | BindingFlags.Instance)
                    .SetValue(o, new string[0]);
            }

            (o as BaseResult).JsonObj = jobject;

            return o;
        }

        private static string SerializeToJson(object r)
        {   
            return JsonConvert
                .SerializeObject(r,
                    new JsonSerializerSettings
                    {
                        ContractResolver = new DefaultContractResolver
                        {
                            NamingStrategy = new SnakeCaseNamingStrategy()
                        }
                    })
                .Replace("h_100,w_100", ""); // TODO: this is temp fix for transfomation
        }

        private static object GenerateWithAutofixture(Type type)
        {
            var fixture = new Fixture();

            return typeof(SpecimenFactory)
                .GetMethods().Where(_ => _.Name == "Create").Skip(1).First()
                .MakeGenericMethod(type)
                .Invoke(null, new object[] { fixture });
        }

        private static object GenerateWithNBuilder(Type type)
        {
            var genericBase = typeof(Builder<>);
            var concreteType = genericBase.MakeGenericType(type);
            var foo1 = concreteType.InvokeMember(nameof(Builder<Foo>.CreateNew), BindingFlags.Static | BindingFlags.Public | BindingFlags.InvokeMethod, null, null, null);
            return foo1.GetType().InvokeMember("Build", BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.Public, null, foo1, null);
        }

        private static object GenerateRandomObject(Type type)
        {
            if (type == typeof(JToken))
            {
                return JToken.Parse(@"{""colour"":""yellow"",""size"":""medium""}");
            }
            if (type == typeof(Transformation))
            {
                return new Transformation().Width(100).Height(100);
            }
            if (type == typeof(DateTime))
            {
                return GenerateWithAutofixture(typeof(DateTime));
            }
            if (Nullable.GetUnderlyingType(type) != null)
            {
                var nullableType = type.GetGenericArguments().First();
                return GenerateRandomObject(nullableType);
            }
            if (type.IsPrimitive || type == typeof(string) || type == typeof(Uri) || type.IsEnum)
            {
                return GenerateWithAutofixture(type);
            }
            if (type.IsArray)
            {
                var arrayType = type.GetElementType();
                var result = Array.CreateInstance(arrayType, 1);
                result.SetValue(GenerateRandomObject(arrayType), 0);
                return result;
            }
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Dictionary<,>))
            {
                var newDict = Activator.CreateInstance(type);
                var genArguments = type.GetGenericArguments();
                var key = GenerateRandomObject(genArguments[0]);
                var value = GenerateRandomObject(genArguments[1]);
                type.GetMethod("Add").Invoke(newDict, new object[] { key, value });
                return newDict;
            }
            if (type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(List<>) || typeof(IEnumerable).IsAssignableFrom(type)))
            {
                var listType = type.GetGenericArguments().First();
                var result = Activator.CreateInstance(typeof(List<>).MakeGenericType(listType)) as IList;
                result.Add(GenerateRandomObject(listType));
                return result;
            }
            var r = Activator.CreateInstance(type);
            type.GetProperties()
                .Where(
                    p => (!p.PropertyType.IsAbstract || typeof(IEnumerable).IsAssignableFrom(p.PropertyType)) 
                        && p.CanWrite
                        && p.Name != "JsonObj"
                )
                .ToList()
                .ForEach(p =>
                {
                    var value = GenerateRandomObject(p.PropertyType);
                    // Console.WriteLine(new {type, p.Name, p.PropertyType, value});
                    p.SetValue(r, value);
                });
            type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                .ToList()
                .ForEach(f =>
                {
                    if (f.Name == "m_resourceType")
                    {
                        f.SetValue(r, "image");
                    }
                    else if(f.Name == "m_resourceTypes")
                    {
                        f.SetValue(r, new string[]{"image", "video"});
                    }
                    else
                    {
                        var value = GenerateRandomObject(f.FieldType);
                        f.SetValue(r, value);
                    }
                });
            return r;
        }
    }

    public class Foo
    {
        public string Name { get; set; }
    }

    public static class TypeGenerator
    {
        public static IEnumerable Types => 
            typeof(BaseResult).Assembly
                .GetTypes()
                .Where(_ => typeof(BaseResult).IsAssignableFrom(_) && !_.IsAbstract)
                .Where(_ => _ != typeof(UploadMappingResults)); // TODO: not sure how to generate it according to the one coming from API
    }
}