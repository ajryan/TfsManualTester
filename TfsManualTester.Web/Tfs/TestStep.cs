namespace TfsManualTester.Web.Tfs
{
    public class TestStep
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Expected { get; set; }

        // TODO: track whether action is ITestStep 
    }
}