using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using System.IO;
using JsonTransformer;
using System.Linq;

namespace JSONTransformer.Test
{
    [TestClass]
    public class JsonTransformertest
    {
        
        private string _mapperJson;
        private JObject _inputObject;
        private JObject _outputObject;
        private JObject _mapperObject;

        [TestInitialize]
        public void Initialize()
        {
            var inputagreementJson = File.ReadAllText("Input.json");
            var outputagreementJson = File.ReadAllText("Output.json");
            //_mapperJson = File.ReadAllText("Mapper.json");
            _inputObject = JObject.Parse(inputagreementJson);
            _outputObject = JObject.Parse(outputagreementJson);
           // _mapperObject = JObject.Parse(_mapperJson);
        }

        [TestMethod]
        public void DirectMapping_ShouldReturnCorrectNode()
        {
            var jsonMapper = new JsonMapper();
            var mapperObject = JObject.Parse("{ 'AgreementNumber': '{AgreementNumber}'}");

            var output = jsonMapper.Transform(_inputObject, mapperObject);
            Assert.AreEqual(output, _outputObject["AgreementNumber"]);

        }


        [TestMethod]
        public void DirectNestedMapping_ShouldReturnCorrectNode()
        {
            var jsonMapper = new JsonMapper();
            var mapperObject = JObject.Parse("{ 'NestedObject': 'Parent.{NestedObject}'}");

            var output = jsonMapper.Transform(_inputObject, mapperObject);
            Assert.AreEqual(Convert.ToString(output), "Hello");

        }

        [TestMethod]
        public void MappingFromArrayObject_ShouldReturnCorrectNode()
        {
            var jsonMapper = new JsonMapper();
            var mapperObject = JObject.Parse(@"{ 'CompanyAdministratorAddressLine1': 'Contacts[Type = \'CA\'].{AddressLine1}' }");

            var output = jsonMapper.Transform(_inputObject, mapperObject);
            Assert.AreEqual(output, "Address Line 1 Admin");
        }


        [TestMethod]
        public void MappingFromArrayObjectWithMultipleFilterinArray_ShouldReturnCorrectNode()
        {
            var jsonMapper = new JsonMapper();
            var mapperObject = JObject.Parse(@"{ 'GPAP1Notification': 'Contacts[NameCode=\'GPA\',Type = \'P1\' ].{ContactName}' }");

            var output = jsonMapper.Transform(_inputObject, mapperObject);
            Assert.AreEqual(output, "P1 ContactName");
        }

        [TestMethod]
        public void MappingArray_ShouldReturnArray()
        {
            var jsonMapper = new JsonMapper();
            var mapperObject = JObject.Parse(@"{'PAXOBillingContact': 'SubAgreements[NameCode=\'PAXO\'].Contacts[Type=\'BP\'].{AddressLine1, AddressLine2, City, PostalCode, State}'}");

            var output = jsonMapper.Transform(_inputObject, mapperObject);
            Assert.AreEqual<string>(output.ToString(), _outputObject["PAXOBillingContact"].ToString());
        }

    }
}
