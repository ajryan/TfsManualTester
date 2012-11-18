using System.Collections.Generic;
using Microsoft.TeamFoundation.TestManagement.Client;

namespace TfsManualTester.Web.Tfs
{
    public class TestCase
    {
        public string TeamProject { get; set; }
        public int? TestCaseId { get; set; }
        public List<TestStep> TestSteps { get; private set; }

        public TestCase(string teamProject)
        {
            TeamProject = teamProject;
            TestCaseId = null;
            TestSteps = new List<TestStep>();
        }

        public TestCase(ITestCase tfsTestCase)
        {
            TeamProject = tfsTestCase.Project.WitProject.Name;
            TestCaseId = tfsTestCase.Id;

            TestSteps = new List<TestStep>();
            foreach (var action in tfsTestCase.Actions)
            {
                if (action is ITestStep)
                {
                    var testStep = (ITestStep)action;

                    // TODO: html steps
                    //newSteps.IsHtml = newSteps.IsHtml || (testStep.Title.ToString().IndexOf("<HTML>", StringComparison.OrdinalIgnoreCase) != -1);

                    TestSteps.Add(
                        new TestStep
                        {
                            Id = testStep.Id,
                            Title = testStep.Title.ToPlainText(),
                            Expected = testStep.ExpectedResult.ToPlainText()
                        });
                }
                else if (action is ISharedStepReference)
                {
                    var sharedStep = (ISharedStepReference)action;
                    TestSteps.Add(
                        new TestStep { Id = sharedStep.Id, Title = "Shared step ID " + sharedStep.SharedStepId });
                }
                else
                {
                    TestSteps.Add(
                        new TestStep { Id = action.Id, Title = "Unknown action " + action.Id });
                }
            }
        }
    }
}