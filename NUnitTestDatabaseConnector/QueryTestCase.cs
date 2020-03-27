using NUnit.Framework;
using DatabaseConnector;
using System.Collections.Generic;

namespace NUnitTestDatabaseConnector
{
    public class Tests
    {
        private DatabaseObject dbDriver;

        [SetUp]
        public void SetUp()
        {
            string host = ;
            string port = "8080";
            string serviceName = "OracleDB";
            string userId = ;
            string password = ;
            dbDriver = new OracleDatabase(host, port, serviceName, userID, password);
        }
        /// <summary>
        /// The QueryExistingTestCaseWithOneRow.
        /// </summary>
        [Test]
        public void QueryExistingTestCaseWithOneRow()
        {
            string collection = "AutomationFramework";
            string testcase = "AutomationDatabaseTestOneRow";
            string release = "1";
            string query = "SELECT T.TESTCASE, T.TESTSTEPDESCRIPTION, T.STEPNUM, T.ACTIONONOBJECT, T.OBJECT, T.VALUE, T.COMMENTS, T.RELEASE, T.LOCAL_ATTEMPTS, T.LOCAL_TIMEOUT, T.CONTROL, T.COLLECTION, T.TEST_STEP_TYPE_ID, T.GOTOSTEP FROM QA_AUTOMATION.TESTCASE T WHERE T.TESTCASE = '" + testcase + "' AND T.COLLECTION = '" + collection + "' AND T.RELEASE = '" + release + "' ORDER BY T.STEPNUM";
            List<List<object>> table = dbDriver.ExecuteQuery(query);
            List<object> row = table[0];
            string dbtestcase = row[0]?.ToString() ?? string.Empty;   // TESTCASE
            string testStepDesc = row[1]?.ToString() ?? string.Empty;   // TESTCASEDESCRIPTION
            string action = row[3]?.ToString() ?? string.Empty;         // ACTIONONOBJECT (test action)
            string attribValue = row[4]?.ToString() ?? string.Empty;    // OBJECT
            string value = row[5]?.ToString() ?? string.Empty;          // VALUE (of the control/field)
            string attribute = row[6]?.ToString() ?? string.Empty;      // COMMENTS (selected attribute)
            string dbrelease = row[7]?.ToString() ?? string.Empty;        // RELEASE
            string stLocAttempts = row[8]?.ToString() ?? "0"; // LOCAL_ATTEMPTS
            string stLocTimeout = row[9]?.ToString() ?? "0";  // LOCAL_TIMEOUT
            string control = row[10]?.ToString() ?? string.Empty;       // CONTROL
            string dbcollection = row[11]?.ToString() ?? string.Empty;   // COLLECTION
            string testStepType = row[12]?.ToString() ?? "1"; // TESTSTEPTYPE (formerly SEVERITY)
            string goToStep = row[13]?.ToString() ?? string.Empty;      // GOTOSTEP

            Assert.AreEqual("AutomationDatabaseTestOneRow", dbtestcase, "Checking the testcase");
            Assert.AreEqual("AutomationDatabaseTestOneRow", testStepDesc, "Checking the test step description");
            Assert.AreEqual("Comment", action, "Checking the action");
            Assert.AreEqual("Test", value, "Checking the value");
            Assert.AreEqual("Test", attribute, "Checking the comments");
            Assert.AreEqual("1", dbrelease, "Checking the release");
            Assert.AreEqual(string.Empty, stLocAttempts, "Checking the local attempts");
            Assert.AreEqual(string.Empty, stLocTimeout, "Checking the lcoal timeout");
            Assert.AreEqual(string.Empty, control, "checking the control");
            Assert.AreEqual("AutomationFramework", dbcollection, "Checking the collection");
            Assert.AreEqual(string.Empty, testStepType, "Checking the test step type.");
            Assert.AreEqual(string.Empty, goToStep, "Checking the goTOStep.");
        }

        /// <summary>
        /// The QueryExistingTestCaseWithMoreThanOneRow.
        /// </summary>
        [Test]
        public void QueryExistingTestCaseWithMoreThanOneRow()
        {
            string collection = "AutomationFramework";
            string testcase = "AutomationDatabaseTestMultiRow";
            string release = "1";
            string query = "SELECT T.TESTCASE, T.TESTSTEPDESCRIPTION, T.STEPNUM, T.ACTIONONOBJECT, T.OBJECT, T.VALUE, T.COMMENTS, T.RELEASE, T.LOCAL_ATTEMPTS, T.LOCAL_TIMEOUT, T.CONTROL, T.COLLECTION, T.TEST_STEP_TYPE_ID, T.GOTOSTEP FROM QA_AUTOMATION.TESTCASE T WHERE T.TESTCASE = '" + testcase + "' AND T.COLLECTION = '" + collection + "' AND T.RELEASE = '" + release + "' ORDER BY T.STEPNUM";
            List<List<object>> table = dbDriver.ExecuteQuery(query);
            Assert.AreEqual(2, table.Count, "Count of multiple rows");
        }

        /// <summary>
        /// The QueryNonExistingTestCase.
        /// </summary>
        [Test]
        public void QueryNonExistingTestCase()
        {
            string collection = "AutomationFramework";
            string testcase = "AutomationDatabaseNonExistingTestCase";
            string release = "1"; 
            string query = "SELECT T.TESTCASE, T.TESTSTEPDESCRIPTION, T.STEPNUM, T.ACTIONONOBJECT, T.OBJECT, T.VALUE, T.COMMENTS, T.RELEASE, T.LOCAL_ATTEMPTS, T.LOCAL_TIMEOUT, T.CONTROL, T.COLLECTION, T.TEST_STEP_TYPE_ID, T.GOTOSTEP FROM QA_AUTOMATION.TESTCASE T WHERE T.TESTCASE = '" + testcase + "' AND T.COLLECTION = '" + collection + "' AND T.RELEASE = '" + release + "' ORDER BY T.STEPNUM";
            dbDriver.ExecuteQuery(query);
        }
    }
}