using System;
using System.Collections;
using System.Linq;
using System.Runtime.Serialization;
using CloudinaryDotNet.Actions;
using Newtonsoft.Json;
using NUnit.Framework;

namespace CloudinaryDotNet.Tests.Parameters
{
    public class DeserializationTests
    {
        [Test]
        public void TestListUploadPresetsResult()
        {
            var presets = new object[] {"1", 1, true, "true", "0", 0, false, "false", ""}
                .Select(v => new { settings = new { live = v } })
                .ToList();
            var serializedObject = JsonConvert.SerializeObject(new {Presets = presets});

            var deserializedObject = JsonConvert.DeserializeObject<ListUploadPresetsResult>(serializedObject);

            Assert.True(deserializedObject.Presets.Take(4).All(result => result.Settings.Live));
            Assert.True(deserializedObject.Presets.Skip(4).All(result => !result.Settings.Live));
        }

        [Test]
        public void TestIfAllResultPropertiesHavePublicSetter()
        {
            typeof(BaseResult).Assembly.GetTypes()
                .Where(type => !type.IsAbstract && type.IsPublic && (typeof(BaseResult).IsAssignableFrom(type)
                            || Attribute.GetCustomAttribute(type, typeof(DataContractAttribute)) != null
                            || type.GetProperties().Any(propInfo => propInfo.CustomAttributes.Any(ca => ca.AttributeType == typeof(JsonPropertyAttribute)))
                       ))
                .ToList()
                .ForEach (type =>
                {
                    type.GetProperties()
                    .Where(
                        p => (!p.PropertyType.IsAbstract || typeof(IEnumerable).IsAssignableFrom(p.PropertyType))
                            && p.CanWrite
                            && p.Name != "JsonObj"
                    )
                    .ToList()
                    .ForEach(propInfo =>
                    {
                        if (!(propInfo.GetSetMethod()?.IsPublic ?? false))
                        {
                            Assert.Fail($"Setter of {type.FullName}.{propInfo.Name} should be public.");
                        }
                    });
                });
        }
    }
}
