using System;
using System.Collections.Generic;
using CloudinaryDotNet.Actions;
using NUnit.Framework;

namespace CloudinaryDotNet.Test.Parameters
{
    public class MetadataFieldUpdateParamsTest
    {
        [Test]
        public void TestMetadataUpdateParamsCheck()
        {
            var parameters = new MetadataUpdateParams();

            Assert.Throws<ArgumentException>(parameters.Check, "List of public ids should not be empty");
        }

        [Test]
        public void TestMetadataUpdateParamsDictionary()
        {
            var parameters = new MetadataUpdateParams
            {
                PublicIds = new List<string> { "test_id_1", "test_id_2" }
            };
            parameters.Metadata.Add("metadata_color", "red");
            parameters.Metadata.Add("metadata_shape", "");
            
            Assert.DoesNotThrow(parameters.Check);
            var dictionary = parameters.ToParamsDictionary();

            Assert.AreEqual(parameters.Type, dictionary["type"]);
            Assert.AreEqual(parameters.PublicIds, dictionary["public_ids"]);
            Assert.AreEqual("metadata_color=red|metadata_shape", dictionary["metadata"]);
        }
    }
}