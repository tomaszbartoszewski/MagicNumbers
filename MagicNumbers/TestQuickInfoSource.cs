using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Operations;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;

namespace MagicNumbers
{
    internal class TestQuickInfoSource : IQuickInfoSource
    {
        private TestQuickInfoSourceProvider m_provider;
        private ITextBuffer m_subjectBuffer;
        private IFileSpecificTooltipProvider fileSpecificTooltipProvider { get; set; }

        public TestQuickInfoSource(TestQuickInfoSourceProvider provider, ITextBuffer subjectBuffer)
        {
            m_provider = provider;
            m_subjectBuffer = subjectBuffer;
            fileSpecificTooltipProvider = new FileSpecificTooltipProvider();
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

            var fileSpecificTooltips = fileSpecificTooltipProvider.GetFileSpecificTooltipDefinitions(GetFileFullPath());

            ITextSnapshot currentSnapshot = subjectTriggerPoint.Value.Snapshot;
            SnapshotSpan querySpan = new SnapshotSpan(subjectTriggerPoint.Value, 0);

            //look for occurrences of our QuickInfo words in the span
            ITextStructureNavigator navigator = m_provider.NavigatorService.GetTextStructureNavigator(m_subjectBuffer);
            TextExtent extent = navigator.GetExtentOfWord(subjectTriggerPoint.Value);
            string searchText = extent.Span.GetText();

            foreach (TooltipDefinition tooltipDefinition in fileSpecificTooltips)
            {
                int foundIndex = searchText.IndexOf(tooltipDefinition.Input, StringComparison.CurrentCultureIgnoreCase);
                if (foundIndex > -1)
                {
                    var start = extent.Span.Start + foundIndex;
                    applicableToSpan = currentSnapshot.CreateTrackingSpan(start, tooltipDefinition.Input.Length, SpanTrackingMode.EdgeInclusive);
                    qiContent.Add(tooltipDefinition.Description);
                    return;
                }
            }

            applicableToSpan = null;
        }

        private string GetFileFullPath()
        {
            // Move it somewhere else, it's crashing while not in this file
            var dte2 = (DTE2)Package.GetGlobalService(typeof(Microsoft.VisualStudio.Shell.Interop.SDTE));
            return dte2.ActiveDocument.FullName;
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
