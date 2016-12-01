using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Operations;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;

namespace MagicNumbers
{
    internal class TestQuickInfoSource : IQuickInfoSource
    {
        private TestQuickInfoSourceProvider m_provider;
        private ITextBuffer m_subjectBuffer;
        private Dictionary<string, string> m_dictionary;
        private Dictionary<string, Dictionary<string, string>> fileTooltips;

        public TestQuickInfoSource(TestQuickInfoSourceProvider provider, ITextBuffer subjectBuffer)
        {
            m_provider = provider;
            m_subjectBuffer = subjectBuffer;

            fileTooltips = new Dictionary<string, Dictionary<string, string>>();
            var firstFile = new Dictionary<string, string>();
            firstFile.Add("add", "my first test");
            fileTooltips.Add("C:\\Users\\tomas\\Desktop\\test.txt", firstFile);
            var secondFile = new Dictionary<string, string>();
            secondFile.Add("add", "int add(int firstInt, int secondInt)\nAdds one integer to another.");
            secondFile.Add("subtract", "int subtract(int firstInt, int secondInt)\nSubtracts one integer from another.");
            secondFile.Add("multiply", "int multiply(int firstInt, int secondInt)\nMultiplies one integer by another.");
            secondFile.Add("divide", "int divide(int firstInt, int secondInt)\nDivides one integer by another.");
            fileTooltips.Add("C:\\Users\\tomas\\Desktop\\test2.txt", secondFile);

            //these are the method names and their descriptions
            m_dictionary = new Dictionary<string, string>();
            m_dictionary.Add("add", "int add(int firstInt, int secondInt)\nAdds one integer to another.");
            m_dictionary.Add("subtract", "int subtract(int firstInt, int secondInt)\nSubtracts one integer from another.");
            m_dictionary.Add("multiply", "int multiply(int firstInt, int secondInt)\nMultiplies one integer by another.");
            m_dictionary.Add("divide", "int divide(int firstInt, int secondInt)\nDivides one integer by another.");
        }

        public void AugmentQuickInfoSession(IQuickInfoSession session, IList<object> qiContent, out ITrackingSpan applicableToSpan)
        {
            // Map the trigger point down to our buffer.
            SnapshotPoint? subjectTriggerPoint = session.GetTriggerPoint(m_subjectBuffer.CurrentSnapshot);
            if (!subjectTriggerPoint.HasValue)
            {
                applicableToSpan = null;
                return;
            }

            var fileSpecificTooltips = GetFileSpecificTooltips();

            ITextSnapshot currentSnapshot = subjectTriggerPoint.Value.Snapshot;
            SnapshotSpan querySpan = new SnapshotSpan(subjectTriggerPoint.Value, 0);

            //look for occurrences of our QuickInfo words in the span
            ITextStructureNavigator navigator = m_provider.NavigatorService.GetTextStructureNavigator(m_subjectBuffer);
            TextExtent extent = navigator.GetExtentOfWord(subjectTriggerPoint.Value);
            string searchText = extent.Span.GetText();

            foreach (string key in fileSpecificTooltips.Keys)
            {
                int foundIndex = searchText.IndexOf(key, StringComparison.CurrentCultureIgnoreCase);
                if (foundIndex > -1)
                {
                    applicableToSpan = currentSnapshot.CreateTrackingSpan
                        (
                                                //querySpan.Start.Add(foundIndex).Position, 9, SpanTrackingMode.EdgeInclusive
                                                extent.Span.Start + foundIndex, key.Length, SpanTrackingMode.EdgeInclusive
                        );

                    string value;
                    fileSpecificTooltips.TryGetValue(key, out value);
                    if (value != null)
                        qiContent.Add(value);
                    else
                        qiContent.Add("");

                    return;
                }
            }

            applicableToSpan = null;
        }

        private Dictionary<string, string> GetFileSpecificTooltips()
        {
            var dte2 = (DTE2)Package.GetGlobalService(typeof(Microsoft.VisualStudio.Shell.Interop.SDTE));
            var activeFileName = dte2.ActiveDocument.FullName;

            Dictionary<string, string> result;

            if (fileTooltips.TryGetValue(activeFileName, out result))
            {
                return result;
            }

            return new Dictionary<string, string>();
        }

        private bool m_isDisposed;
        public void Dispose()
        {
            if (!m_isDisposed)
            {
                GC.SuppressFinalize(this);
                m_isDisposed = true;
            }
        }
    }
}
