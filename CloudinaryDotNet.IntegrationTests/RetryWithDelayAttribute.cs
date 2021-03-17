using System;
using System.Threading;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using NUnit.Framework.Internal.Commands;

namespace CloudinaryDotNet.IntegrationTest
{
    /// <summary>
    /// <see cref="RetryWithDelayAttribute" /> is used on a test method to specify that it should
    /// be rerun if it fails, up to a maximum number of times with a specified delay in seconds.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public class RetryWithDelayAttribute : PropertyAttribute, IWrapSetUpTearDown
    {
        private readonly int m_retryCount;
        private readonly int m_delaySeconds;

        /// <summary>
        /// Construct a <see cref="RetryWithDelayAttribute" />
        /// </summary>
        /// <param name="retryCount">The maximum number of retries. Default value: 3</param>
        /// <param name="delaySeconds">The delay between retries in seconds. Default value: 3 seconds</param>
        public RetryWithDelayAttribute(int retryCount = 3, int delaySeconds = 3) : base(retryCount)
        {
            this.m_retryCount = retryCount;
            this.m_delaySeconds = delaySeconds;
        }

        /// <summary>
        /// Wrap a command and return the result.
        /// </summary>
        /// <param name="command">The command to be wrapped</param>
        /// <returns>The wrapped command</returns>
        public TestCommand Wrap(TestCommand command)
        {
            return new CustomRetryCommand(command, m_retryCount, m_delaySeconds);
        }

        /// <summary>
        /// The test command for the <see cref="RetryWithDelayAttribute"/>
        /// </summary>
        public class CustomRetryCommand : DelegatingTestCommand
        {
            private readonly int m_retryCount;
            private readonly int m_delaySeconds;

            /// <summary>
            /// Initializes a new instance of the <see cref="CustomRetryCommand"/> class.
            /// </summary>
            /// <param name="innerCommand">The inner command.</param>
            /// <param name="retryCount">The maximum number of retries.</param>
            /// <param name="delaySeconds">The delay between retries in seconds.</param>
            public CustomRetryCommand(TestCommand innerCommand, int retryCount, int delaySeconds)
                : base(innerCommand)
            {
                this.m_retryCount = retryCount;
                this.m_delaySeconds = delaySeconds;
            }

            /// <summary>
            /// Runs the test, saving a TestResult in the supplied TestExecutionContext.
            /// </summary>
            /// <param name="context">The context in which the test should run.</param>
            /// <returns>A TestResult</returns>
            public override TestResult Execute(TestExecutionContext context)
            {
                var count = m_retryCount;

                while (count-- > 0)
                {
                    try
                    {
                        context.CurrentResult = innerCommand.Execute(context);
                    }
                    catch (Exception ex)
                    {
                        if (context.CurrentResult == null) context.CurrentResult = context.CurrentTest.MakeTestResult();
                        context.CurrentResult.RecordException(ex);
                    }

                    if (context.CurrentResult.ResultState != ResultState.Failure)
                        break;

                    if (count <= 0) continue;

                    context.CurrentResult = context.CurrentTest.MakeTestResult();
                    context.CurrentRepeatCount++;
                    Thread.Sleep(TimeSpan.FromSeconds(m_delaySeconds));
                }

                return context.CurrentResult;
            }
        }
    }
}
