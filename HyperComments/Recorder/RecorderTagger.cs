﻿using System.IO;
using System.Text.RegularExpressions;

using EnvDTE;
using EnvDTE80;

using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Text.Classification;


namespace HyperComments.Recorder
{
    public class RcorderTagger : RegexTagger<RecorderTag>
    {
        private readonly ITextBuffer _buffer;
        private readonly SVsServiceProvider _serviceProvider;
        private readonly IAccessFiles _fileAccess;
        
        public RcorderTagger(SVsServiceProvider serviceProvider, ITextBuffer buffer, IClassifier classifier) : 
            this(new FileSystemAdapter(), serviceProvider, buffer, classifier)
        {            
        }


        public RcorderTagger(IAccessFiles fileAccess, SVsServiceProvider serviceProvider, ITextBuffer buffer, IClassifier classifier) : 
            base(buffer, new[] { new Regex("// {recorder}", RegexOptions.Compiled |  RegexOptions.IgnoreCase) })
        {
            _buffer = buffer;
            _serviceProvider = serviceProvider;
            _fileAccess = fileAccess;
        }

        protected override RecorderTag TryCreateTagForMatch(Match match)
        {
            return new RecorderTag(GetRecordingDirectory(), GetActiveDocument());            
        }

        private string GetRecordingDirectory()
        {
            var dte2 = (DTE2)_serviceProvider.GetService(typeof(DTE));
            var solution = (Solution2)dte2.Solution;

            string recordingDirectory = Path.Combine(Path.GetDirectoryName(solution.FileName), "Audio Comments");

            if(!_fileAccess.DirectoryExists(recordingDirectory))
            {
                _fileAccess.CreateDirectory(recordingDirectory);
            }

            return recordingDirectory;
        }

        private string GetActiveDocument()
        {
            var dte2 = (DTE2) _serviceProvider.GetService(typeof (DTE));
            return dte2.ActiveDocument.Name;
        }
    }
}
