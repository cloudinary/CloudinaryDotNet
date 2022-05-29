using System;
using System.Collections.Generic;
using CloudinaryDotNet.Actions;
using NUnit.Framework;

namespace CloudinaryDotNet.Tests.Provisioning.Parameters
{
    public class AccountProvisioningParamsTest
    {
        private const string Prefix = "test_prefix";
        private const string TestName = "sub_account_name";
        private const string CloudName = "cloud_name";
        private const string TestId = "base_id";
        private const string Email = "some@email.com";
        private const Role Role = Actions.Role.Admin;
        private readonly List<string> idList = new List<string> {"id1", "id2"};
        private readonly StringDictionary customAttributes = new StringDictionary("key1=value1", "key2=value2");

        [Test]
        public void TestCreateSubAccountParamsCheck()
        {
            var parameters = new CreateSubAccountParams(TestName);
            parameters.Name = string.Empty;
            AssertCheck(parameters, "Name must not be empty");
        }

        [Test]
        public void TestUpdateSubAccountParamsCheck()
        {
            var parameters = new UpdateSubAccountParams(TestId);
            parameters.SubAccountId = string.Empty;
            AssertCheck(parameters, "SubAccountId must not be empty");
        }

        [Test]
        public void TestCreateUserParamsCheck()
        {
            var parameters = new CreateUserParams(TestName, Email, Role);

            parameters.Name = string.Empty;
            AssertCheck(parameters, "Name must not be empty");

            parameters.Name = TestName;
            parameters.Email = null;
            AssertCheck(parameters, "Email must not be empty");

            parameters.Email = Email;
            parameters.Role = null;
            AssertCheck(parameters, "Role must not be empty");
        }

        [Test]
        public void TestUpdateUserParamsCheck()
        {
            var parameters = new UpdateUserParams(TestId);
            parameters.UserId = string.Empty;
            AssertCheck(parameters, "UserId must not be empty");
        }

        [Test]
        public void TestCreateUserGroupParamsCheck()
        {
            var parameters = new CreateUserGroupParams(TestName);
            parameters.Name = string.Empty;
            AssertCheck(parameters, "Name must not be empty");
        }

        [Test]
        public void TestUpdateUserGroupParamsCheck()
        {
            var parameters = new UpdateUserGroupParams(TestId, TestName);

            parameters.Name = string.Empty;
            AssertCheck(parameters, "Name must not be empty");

            parameters.Name = TestName;
            parameters.UserGroupId = null;
            AssertCheck(parameters, "UserGroupId must not be empty");
        }

        [Test]
        public void TestListSubAccountsParamsDictionary()
        {
            var parameters = new ListSubAccountsParams
            {
                Enabled = true,
                Prefix = Prefix,
                Ids = idList
            };
            Assert.DoesNotThrow(() => parameters.Check());

            var dictionary = parameters.ToParamsDictionary();
            Assert.AreEqual("true", dictionary["enabled"]);
            Assert.AreEqual(Prefix, dictionary["prefix"]);
            Assert.AreEqual(idList, dictionary["ids"]);
        }

        [Test]
        public void TestCreateSubAccountParamsDictionary()
        {
            var parameters = new CreateSubAccountParams(TestName)
            {
                CloudName = CloudName,
                BaseSubAccountId = TestId,
                CustomAttributes = customAttributes
            };
            Assert.DoesNotThrow(() => parameters.Check());

            var dictionary = parameters.ToParamsDictionary();
            Assert.AreEqual("true", dictionary["enabled"]);
            Assert.AreEqual(TestName, dictionary["name"]);
            Assert.AreEqual(CloudName, dictionary["cloud_name"]);
            Assert.AreEqual(TestId, dictionary["base_sub_account_id"]);
            Assert.AreEqual("key1=value1|key2=value2", dictionary["custom_attributes"]);
        }

        [Test]
        public void TestUpdateSubAccountParamsDictionary()
        {
            var parameters = new UpdateSubAccountParams(TestId)
            {
                Name = TestName,
                CloudName = CloudName,
                Enabled = false,
                CustomAttributes = customAttributes
            };
            Assert.DoesNotThrow(() => parameters.Check());

            var dictionary = parameters.ToParamsDictionary();
            Assert.AreEqual("false", dictionary["enabled"]);
            Assert.AreEqual(TestName, dictionary["name"]);
            Assert.AreEqual(CloudName, dictionary["cloud_name"]);
            Assert.AreEqual("key1=value1|key2=value2", dictionary["custom_attributes"]);
        }

        [Test]
        public void TestListUsersParamsDictionary()
        {
            var parameters = new ListUsersParams
            {
                Pending = true,
                Prefix = Prefix,
                UserIds = idList,
                SubAccountId = TestId,
                LastLogin = true,
                From = new DateTime(2022, 01, 10),
                To = new DateTime(2022, 02, 13),
            };
            Assert.DoesNotThrow(() => parameters.Check());

            var dictionary = parameters.ToParamsDictionary();
            Assert.AreEqual("true", dictionary["pending"]);
            Assert.AreEqual(Prefix, dictionary["prefix"]);
            Assert.AreEqual(idList, dictionary["ids"]);
            Assert.AreEqual(TestId, dictionary["sub_account_id"]);
            Assert.AreEqual("true", dictionary["last_login"]);
            Assert.AreEqual("2022-01-10T00:00:00", dictionary["from"]);
            Assert.AreEqual("2022-02-13T00:00:00", dictionary["to"]);
        }

        [Test]
        public void TestCreateUserParamsDictionary()
        {
            var parameters = new CreateUserParams(TestName, Email, Role)
            {
                SubAccountIds = idList
            };
            Assert.DoesNotThrow(() => parameters.Check());

            var dictionary = parameters.ToParamsDictionary();
            Assert.AreEqual(TestName, dictionary["name"]);
            Assert.AreEqual(Email, dictionary["email"]);
            Assert.AreEqual("admin", dictionary["role"]);
            Assert.AreEqual(idList, dictionary["sub_account_ids"]);
        }

        [Test]
        public void TestUpdateUserParamsDictionary()
        {
            var parameters = new UpdateUserParams(TestId)
            {
                Name = TestName,
                Email = Email,
                Role = Role,
                SubAccountIds = idList
            };
            Assert.DoesNotThrow(() => parameters.Check());

            var dictionary = parameters.ToParamsDictionary();
            Assert.AreEqual(TestName, dictionary["name"]);
            Assert.AreEqual(Email, dictionary["email"]);
            Assert.AreEqual("admin", dictionary["role"]);
            Assert.AreEqual(idList, dictionary["sub_account_ids"]);
        }

        [Test]
        public void TestCreateUserGroupParamsDictionary()
        {
            var parameters = new CreateUserGroupParams(TestName);
            Assert.DoesNotThrow(() => parameters.Check());

            var dictionary = parameters.ToParamsDictionary();
            Assert.AreEqual(TestName, dictionary["name"]);
        }

        [Test]
        public void TestUpdateUserGroupParamsDictionary()
        {
            var parameters = new UpdateUserGroupParams(TestId, TestName);
            Assert.DoesNotThrow(() => parameters.Check());

            var dictionary = parameters.ToParamsDictionary();
            Assert.AreEqual(TestName, dictionary["name"]);
        }

        private static void AssertCheck<T>(T parameters, string expectedMessage)
            where T : BaseParams
        {
            Assert.Throws<ArgumentException>(parameters.Check, expectedMessage);
        }
    }
}
