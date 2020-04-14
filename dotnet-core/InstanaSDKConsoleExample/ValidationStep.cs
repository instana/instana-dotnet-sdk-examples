namespace InstanaSDKConsoleExample
{
    using Instana.Tracing.Sdk.Spans;
    using System;
    using System.Collections.Generic;

    internal class ValidationStep : IStep
    {
        public string Execute(string input, Dictionary<string, object> protocol)
        {
            using (var span = CustomSpan.CreateEntry(this, CorrelationUtil.GetCorrelationFromCurrentContext))
            {
                span.WrapAction(() =>
                {
                    span.SetTag(new string[] { "Input" }, input);
                    if (string.IsNullOrEmpty(input))
                    {
                        protocol["Validation-Result"] = "Invalid";
                        throw new ArgumentException("The input for this job was invalid!");
                    }
                    protocol["Validation-Result"] =  "Valid";
                }, true);
                return input;
            }
        }
    }
}
