using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Altinn.ApiClients.Dan.Interfaces;
using Altinn.ApiClients.Dan.Models;
using Altinn.ApiClients.Dan.Models.Enums;
using Altinn.ApiClients.Dan.Services;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Tests.Services
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class DanClientTest
    {

        [Test]
        public async Task DeserializeTypedToSuppliedField_Ok()
        {
            // Setup
            var danApi = new Mock<IDanApi>();
            danApi
                .Setup(x => x.GetDirectharvest(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<Dictionary<string, string>>(),
                    It.IsAny<bool>(),
                    It.IsAny<bool>(),
                    It.IsAny<string>()))
                .ReturnsAsync(new DataSet
                {
                    Values = new List<DataSetValue>
                    {
                        new()
                        {
                            Name = "SomeString",
                            Value = "Bar",
                            ValueType = DataSetValueType.String
                        },
                        new()
                        {
                            Name = "SomeJson",
                            Value = "{\"SomeString\":\"Bar\",\"SomeNumber\":123,\"SomeDateTime\":\"2021-12-12T04:56:12\"}",
                            ValueType = DataSetValueType.JsonSchema
                        }

                    }
                });

            var danClient = new DanClient(danApi.Object);

            // Act
            MyModel result = await danClient.GetDataSet<MyModel>("a", "a", deserializeField: "SomeJson");

            // Verify
            Assert.IsNotNull(result);
            Assert.IsFalse(string.IsNullOrEmpty(result.SomeString));
            Assert.AreEqual(result.SomeNumber, (decimal)123);
            Assert.IsNotNull(result.SomeDateTime);
            Assert.IsFalse(result.SomeDateTime.Equals(DateTime.MinValue));
        }

        [Test]
        public async Task DeserializeTypedToSuppliedFieldJsonNet_Ok()
        {
            // Setup
            var danApi = new Mock<IDanApi>();
            danApi
                .Setup(x => x.GetDirectharvest(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<Dictionary<string, string>>(),
                    It.IsAny<bool>(),
                    It.IsAny<bool>(),
                    It.IsAny<string>()))
                .ReturnsAsync(new DataSet
                {
                    Values = new List<DataSetValue>
                    {
                        new()
                        {
                            Name = "SomeString",
                            Value = "Bar",
                            ValueType = DataSetValueType.String
                        },
                        new()
                        {
                            Name = "SomeJson",
                            Value = "{\"SomeString\":\"Bar\",\"SomeNumber\":123,\"SomeDateTime\":\"2035_06_12\"}",
                            ValueType = DataSetValueType.JsonSchema
                        }

                    }
                });

            var danClient = new DanClient(danApi.Object)
            {
                Configuration = new DanConfiguration
                {
                    Deserializer = new JsonNetDeserializer
                    {
                        SerializerSettings = new JsonSerializerSettings()
                        {
                            DateFormatString = "yyyy_MM_dd"
                        }
                    }
                }
            };

            // Act
            MyModel result = await danClient.GetDataSet<MyModel>("a", "a", deserializeField: "SomeJson");

            // Verify
            Assert.IsNotNull(result);
            Assert.IsFalse(string.IsNullOrEmpty(result.SomeString));
            Assert.AreEqual(result.SomeNumber, (decimal)123);
            Assert.IsNotNull(result.SomeDateTime);
            Assert.IsFalse(result.SomeDateTime.Equals(DateTime.MinValue));
        }

        [Test]
        public async Task DeserializeTypedToSuppliedFieldJsonNetDefault_Ok()
        {
            // Setup
            var danApi = new Mock<IDanApi>();
            danApi
                .Setup(x => x.GetDirectharvestUnenveloped(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<Dictionary<string, string>>(),
                    It.IsAny<bool>(),
                    It.IsAny<bool>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                .ReturnsAsync("{\"SomeString\":\"Bar\",\"SomeNumber\":123,\"SomeDateTime\":\"2035_06_12\"}");

            var danClient = new DanClient(danApi.Object)
            {
                Configuration = new DanConfiguration
                {
                    Deserializer = new JsonNetDeserializer
                    {
                        SerializerSettings = new JsonSerializerSettings()
                        {
                            DateFormatString = "yyyy_MM_dd"
                        }
                    }
                }
            };

            // Act
            MyModel result = await danClient.GetDataSet<MyModel>("a", "a");

            // Verify
            Assert.IsNotNull(result);
            Assert.IsFalse(string.IsNullOrEmpty(result.SomeString));
            Assert.AreEqual(result.SomeNumber, (decimal)123);
            Assert.IsNotNull(result.SomeDateTime);
            Assert.IsFalse(result.SomeDateTime.Equals(DateTime.MinValue));

        }

        [Test]
        public void DeserializeTypedToSuppliedField_FailedMissing()
        {
            // Setup
            var danApi = new Mock<IDanApi>();
            danApi
                .Setup(x => x.GetDirectharvest(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<Dictionary<string, string>>(),
                    It.IsAny<bool>(),
                    It.IsAny<bool>(),
                    It.IsAny<string>()))
                .ReturnsAsync(new DataSet
                {
                    Values = new List<DataSetValue>
                    {
                        new()
                        {
                            Name = "SomeString",
                            Value = "Bar",
                            ValueType = DataSetValueType.String
                        },
                        new()
                        {
                            Name = "SomeJson",
                            Value = "{\"SomeString\":\"Bar\",\"SomeNumber\":123,\"SomeDateTime\":\"2021-12-12T04:56:12\"}",
                            ValueType = DataSetValueType.JsonSchema
                        }

                    }
                });

            var danClient = new DanClient(danApi.Object);

            // Act / verify
            Assert.ThrowsAsync<DanException>(async () =>
                await danClient.GetDataSet<MyModel>("a", "a", deserializeField: "SomeJsonThatDoesNotExist"));

        }

        [Test]
        public async Task DeserializeTypedWithoutDeserializeField_Ok()
        {
            // Setup
            var danApi = new Mock<IDanApi>();
            danApi
                .Setup(x => x.PostDirectharvestUnenveloped(
                    It.IsAny<DirectHarvestPostBody>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<Dictionary<string, string>>(),
                    It.IsAny<bool>(),
                    It.IsAny<bool>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                .ReturnsAsync("{\"SomeString\":\"Bar\",\"SomeNumber\":123,\"SomeDateTime\":\"2021-12-12T04:56:12\"}");

            var danClient = new DanClient(danApi.Object);

            // Act
            MyModel result = await danClient.GetDataSet<MyModel>("a", "12345678910");

            // Verify
            Assert.IsNotNull(result);
            Assert.IsFalse(string.IsNullOrEmpty(result.SomeString));
            Assert.AreEqual(result.SomeNumber, (decimal)123);
            Assert.IsNotNull(result.SomeDateTime);
            Assert.IsFalse(result.SomeDateTime.Equals(DateTime.MinValue));

        }

        [Test]
        public void DeserializeTypedSuppliedFieldNotJsonType_Fail()
        {
            // Setup
            var danApi = new Mock<IDanApi>();
            danApi
                .Setup(x => x.GetDirectharvest(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<Dictionary<string, string>>(),
                    It.IsAny<bool>(),
                    It.IsAny<bool>(),
                    It.IsAny<string>()))
                .ReturnsAsync(new DataSet
                {
                    Values = new List<DataSetValue>
                    {
                        new()
                        {
                            Name = "SomeString",
                            Value = "{}",
                            ValueType = DataSetValueType.String
                        }
                    }
                });

            var danClient = new DanClient(danApi.Object);

            // Act / verify
            Assert.ThrowsAsync<DanException>(async () =>
                await danClient.GetDataSet<MyModel>("a", "a", deserializeField: "SomeString"));
        }

        [Test]
        public void DeserializeTypedSuppliedFieldInvalidJson_Fail()
        {
            // Setup
            var danApi = new Mock<IDanApi>();
            danApi
                .Setup(x => x.GetDirectharvest(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<Dictionary<string, string>>(),
                    It.IsAny<bool>(),
                    It.IsAny<bool>(),
                    It.IsAny<string>()))
                .ReturnsAsync(new DataSet
                {
                    Values = new List<DataSetValue>
                    {
                        new()
                        {
                            Name = "SomeJson",
                            Value = "notjson",
                            ValueType = DataSetValueType.JsonSchema
                        }
                    }
                });

            var danClient = new DanClient(danApi.Object);

            // Act / verify
            Assert.ThrowsAsync<DanException>(async () =>
                await danClient.GetDataSet<MyModel>("a", "a", deserializeField: "SomeJson"));
        }

        private DataSet GetDataSet()
        {
            return new DataSet
            {
                Values = new List<DataSetValue>
                {
                    new()
                    {
                        Name = "SomeString",
                        Value = "Bar",
                        ValueType = DataSetValueType.String
                    },
                    new()
                    {
                        Name = "SomeNumber",
                        Value = "123",
                        ValueType = DataSetValueType.String
                    },
                    new()
                    {
                        Name = "SomeDateTime",
                        Value = "2021-12-12T04:56:12",
                        ValueType = DataSetValueType.String
                    },
                }
            };
        }
    }

    internal class MyModel
    {
        public string SomeString { get; set; }
        public decimal SomeNumber { get; set; }
        public DateTime SomeDateTime { get; set; }
    }
}
