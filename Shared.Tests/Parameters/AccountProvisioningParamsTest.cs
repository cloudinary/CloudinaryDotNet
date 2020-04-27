using System;
using System.Collections.Generic;
using CloudinaryDotNet.Actions;
using NUnit.Framework;

namespace CloudinaryDotNet.Test.Parameters
{
    public class AccountProvisioningParamsTest
    {
        private const string prefix = "test_prefix";
        private const string testName = "sub_account_name";
        private const string cloudName = "cloud_name";
        private const string testId = "base_id";
        private const string email = "some@email.com";
        private const Role role = Role.Admin;
        private readonly List<string> idList = new List<string> {"id1", "id2"};
        private readonly StringDictionary customAttributes = new StringDictionary("key1=value1", "key2=value2");

        [Test]
        public void TestCreateSubAccountParamsCheck()
        {
            var parameters = new CreateSubAccountParams(testName);
            parameters.Name = string.Empty;
            AssertCheck(parameters, "Name must not be empty");
        }

        [Test]
        public void TestUpdateSubAccountParamsCheck()
        {
            var parameters = new UpdateSubAccountParams(testId);
            parameters.SubAccountId = string.Empty;
            AssertCheck(parameters, "SubAccountId must not be empty");
        }

        [Test]
        public void TestCreateUserParamsCheck()
        {
            var parameters = new CreateUserParams(testName, email, role);

            parameters.Name = string.Empty;
            AssertCheck(parameters, "Name must not be empty");

            parameters.Name = testName;
            parameters.Email = null;
            AssertCheck(parameters, "Email must not be empty");

            parameters.Email = email;
            parameters.Role = null;
            AssertCheck(parameters, "Role must not be empty");
        }

        [Test]
        public void TestUpdateUserParamsCheck()
        {
            var parameters = new UpdateUserParams(testId);
            parameters.UserId = string.Empty;
            AssertCheck(parameters, "UserId must not be empty");
        }

        [Test]
        public void TestCreateUserGroupParamsCheck()
        {
            var parameters = new CreateUserGroupParams(testName);
            parameters.Name = string.Empty;
            AssertCheck(parameters, "Name must not be empty");
        }

        [Test]
        public void TestUpdateUserGroupParamsCheck()
        {
            var parameters = new UpdateUserGroupParams(testId, testName);

            parameters.Name = string.Empty;
            AssertCheck(parameters, "Name must not be empty");

            parameters.Name = testName;
            parameters.UserGroupId = null;
            AssertCheck(parameters, "UserGroupId must not be empty");
        }

        [Test]
        public void TestListSubAccountsParamsDictionary()
        {
            var parameters = new ListSubAccountsParams
            {
                Enabled = true,
                Prefix = prefix,
                Ids = idList
            };
            Assert.DoesNotThrow(() => parameters.Check());

            var dictionary = parameters.ToParamsDictionary();
            Assert.AreEqual("true", dictionary["enabled"]);
            Assert.AreEqual(prefix, dictionary["prefix"]);
            Assert.AreEqual(idList, dictionary["ids"]);
        }

        [Test]
        public void TestCreateSubAccountParamsDictionary()
        {
            var parameters = new CreateSubAccountParams(testName)
            {
                CloudName = cloudName,
                BaseSubAccountId = testId,
                CustomAttributes = customAttributes
            };
            Assert.DoesNotThrow(() => parameters.Check());

            var dictionary = parameters.ToParamsDictionary();
            Assert.AreEqual("true", dictionary["enabled"]);
            Assert.AreEqual(testName, dictionary["name"]);
            Assert.AreEqual(cloudName, dictionary["cloud_name"]);
            Assert.AreEqual(testId, dictionary["base_sub_account_id"]);
            Assert.AreEqual("key1=value1|key2=value2", dictionary["custom_attributes"]);
        }

        [Test]
        public void TestUpdateSubAccountParamsDictionary()
        {
            var parameters = new UpdateSubAccountParams(testId)
            {
                Name = testName,
                CloudName = cloudName,
                Enabled = false,
                CustomAttributes = customAttributes
            };
            Assert.DoesNotThrow(() => parameters.Check());

            var dictionary = parameters.ToParamsDictionary();
            Assert.AreEqual("false", dictionary["enabled"]);
            Assert.AreEqual(testName, dictionary["name"]);
            Assert.AreEqual(cloudName, dictionary["cloud_name"]);
            Assert.AreEqual("key1=value1|key2=value2", dictionary["custom_attributes"]);
        }

        [Test]
        public void TestListUsersParamsDictionary()
        {
            var parameters = new ListUsersParams
            {
                Pending = true,
                Prefix = prefix,
                UserIds = idList,
                SubAccountId = testId
            };
            Assert.DoesNotThrow(() => parameters.Check());

            var dictionary = parameters.ToParamsDictionary();
            Assert.AreEqual("true", dictionary["pending"]);
            Assert.AreEqual(prefix, dictionary["prefix"]);
            Assert.AreEqual(idList, dictionary["ids"]);
            Assert.AreEqual(testId, dictionary["sub_account_id"]);
        }

        [Test]
        public void TestCreateUserParamsDictionary()
        {
            var parameters = new CreateUserParams(testName, email, role)
            {
                SubAccountIds = idList
            };
            Assert.DoesNotThrow(() => parameters.Check());

            var dictionary = parameters.ToParamsDictionary();
            Assert.AreEqual(testName, dictionary["name"]);
            Assert.AreEqual(email, dictionary["email"]);
            Assert.AreEqual("admin", dictionary["role"]);
            Assert.AreEqual(idList, dictionary["sub_account_ids"]);
        }

        [Test]
        public void TestUpdateUserParamsDictionary()
        {
            var parameters = new UpdateUserParams(testId)
            {
                Name = testName,
                Email = email,
                Role = role,
                SubAccountIds = idList
            };
            Assert.DoesNotThrow(() => parameters.Check());

            var dictionary = parameters.ToParamsDictionary();
            Assert.AreEqual(testName, dictionary["name"]);
            Assert.AreEqual(email, dictionary["email"]);
            Assert.AreEqual("admin", dictionary["role"]);
            Assert.AreEqual(idList, dictionary["sub_account_ids"]);
        }

        [Test]
        public void TestCreateUserGroupParamsDictionary()
        {
            var parameters = new CreateUserGroupParams(testName);
            Assert.DoesNotThrow(() => parameters.Check());

            var dictionary = parameters.ToParamsDictionary();
            Assert.AreEqual(testName, dictionary["name"]);
        }

        [Test]
        public void TestUpdateUserGroupParamsDictionary()
        {
            var parameters = new UpdateUserGroupParams(testId, testName);
            Assert.DoesNotThrow(() => parameters.Check());

            var dictionary = parameters.ToParamsDictionary();
            Assert.AreEqual(testName, dictionary["name"]);
        }

        private static void AssertCheck<T>(T parameters, string expectedMessage)
            where T : BaseParams
        {
            Assert.Throws<ArgumentException>(parameters.Check, expectedMessage);
        }
    }
}
