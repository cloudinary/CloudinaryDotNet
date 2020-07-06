using System.Collections.Generic;
using System.Linq;
using CloudinaryDotNet.Actions;
using Newtonsoft.Json;
using NUnit.Framework;

namespace CloudinaryDotNet.Test.Parameters
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
    }
}
