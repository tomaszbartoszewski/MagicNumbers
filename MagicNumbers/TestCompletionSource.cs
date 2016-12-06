using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Operations;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using MagicNumbers.Data;

namespace MagicNumbers
{
    internal class TestCompletionSource : ICompletionSource
    {
        private TestCompletionSourceProvider m_sourceProvider;
        private ITextBuffer m_textBuffer;
        private List<Completion> m_compList;
        private IFileSpecificTooltipProvider fileSpecificTooltipProvider { get; set; }

        public TestCompletionSource(TestCompletionSourceProvider sourceProvider, ITextBuffer textBuffer)
        {
            m_sourceProvider = sourceProvider;
            m_textBuffer = textBuffer;
            fileSpecificTooltipProvider = new FileSpecificTooltipProvider();
        }

        void ICompletionSource.AugmentCompletionSession(ICompletionSession session, IList<CompletionSet> completionSets)
        {
            TooltipDefinition[] tooltipDefinitions = fileSpecificTooltipProvider.GetFileSpecificTooltipDefinitions(GetFileFullPath());
            m_compList = new List<Completion>();
            foreach (TooltipDefinition tooltipDefinition in tooltipDefinitions)
            {
                m_compList.Add(new Completion(tooltipDefinition.Description, tooltipDefinition.Input, tooltipDefinition.Input, null, null));
                m_compList.Add(new Completion(tooltipDefinition.Input, tooltipDefinition.Input, tooltipDefinition.Description, null, null));
            }

            completionSets.Add(new CompletionSet(
                "Tokens",    //the non-localized title of the tab
                "Tokens",    //the display title of the tab
                FindTokenSpanAtPosition(session.GetTriggerPoint(m_textBuffer),
                    session),
                m_compList,
                null));
        }

        private ITrackingSpan FindTokenSpanAtPosition(ITrackingPoint point, ICompletionSession session)
        {
            SnapshotPoint currentPoint = (session.TextView.Caret.Position.BufferPosition) - 1;
            ITextStructureNavigator navigator = m_sourceProvider.NavigatorService.GetTextStructureNavigator(m_textBuffer);
            TextExtent extent = navigator.GetExtentOfWord(currentPoint);
            return currentPoint.Snapshot.CreateTrackingSpan(extent.Span, SpanTrackingMode.EdgeInclusive);
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
